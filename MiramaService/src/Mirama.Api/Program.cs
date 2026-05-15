using Microsoft.OpenApi;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Mirama.Api.Auth;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Mirama.SharedKernel;
using System.Security.Cryptography;
using System.Text;
using Mirama.Modules.Clients.Infrastructure;
using Mirama.Modules.Identity;
using Mirama.Modules.Identity.Infrastructure.Persistence;
using Mirama.Modules.Identity.Infrastructure.Persistence.Seeding;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .CreateBootstrapLogger();

builder.Host.UseSerilog((context, services, configuration) =>
{
    configuration
        .ReadFrom.Configuration(context.Configuration)
        .Enrich.FromLogContext();
});
builder.Services.AddLogging();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHealthChecks();

var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>()
    ?? throw new InvalidOperationException("Cors:AllowedOrigins is not configured.");

builder.Services.AddCors(options => options
    .AddDefaultPolicy(
        policy => policy
            .WithOrigins(allowedOrigins)
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()));

builder.Services.AddSwaggerGen(c => c
    .SwaggerDoc("v1", new OpenApiInfo { Title = "Mirama API", Version = "v1" }));

builder.Services.AddProblemDetails();

builder.Services
    .AddIdentityModule(builder.Configuration)
    .AddClientsModule(builder.Configuration)
    .AddSharedServices(builder.Configuration);

builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ReportApiVersions = true;
});

builder.Services.AddHealthChecks();
builder.Services.AddHttpContextAccessor();

builder.Services.AddAuthentication()
    .AddJwtBearer(options =>
    {
        var authSection = builder.Configuration.GetSection("Authentication");
        var rawSecret = authSection["NextAuthSecret"] ?? throw new InvalidOperationException("Authentication:NextAuthSecret is not configured.");

        var derivedKeyBytes = HKDF.DeriveKey(
            HashAlgorithmName.SHA256,
            ikm: Encoding.UTF8.GetBytes(rawSecret),
            outputLength: 32,
            salt: [],
            info: Encoding.UTF8.GetBytes("NextAuth.js Generated Encryption Key")
        );
        var tokenDecryptionKey = new SymmetricSecurityKey(derivedKeyBytes);

        // Inner JWS is signed with raw secret (HS256).
        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(rawSecret));

        options.Audience = authSection["Audience"];
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                // Read JWT from NextAuth session cookie (prod uses __Secure- prefix).
                var cookie = context.Request.Cookies["__Secure-authjs.session-token"]
                    ?? context.Request.Cookies["authjs.session-token"];
                if (!string.IsNullOrEmpty(cookie))
                    context.Token = cookie;
                return Task.CompletedTask;
            }
        };
        options.MapInboundClaims = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            TokenDecryptionKey = tokenDecryptionKey,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = signingKey,
            ValidateIssuer = true,
            ValidateLifetime = true,
            ValidateAudience = true,
            ValidIssuer = authSection["Authority"],
            ValidAudience = authSection["Audience"],
            ClockSkew = TimeSpan.Zero,
            NameClaimType = "name",
        };
    });

builder.Services.AddSingleton<IAuthorizationHandler, RequireTenantAndOrgHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, RequireTenantOnlyHandler>();

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("RequireTenantAndOrg", policy =>
        policy.Requirements.Add(new TenantAndOrgRequirement()))
    .AddPolicy("RequireTenantOnly", policy =>
        policy.Requirements.Add(new TenantOnlyRequirement()));

var app = builder.Build();

// TODO: Make this work for all modules
// Applying migrations immediately in Development, handled in CICD pipeline for production
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    try
    {
        var db = scope.ServiceProvider.GetRequiredService<IdentityDbContext>();
        logger.LogInformation("Applying database migrations...");
        await db.Database.MigrateAsync();
        logger.LogInformation("Database migrations applied successfully");

        logger.LogInformation("Seeding database...");
        await PolicySeed.SeedDataAsync(db);
        await RoleSeed.SeedDataAsync(db);
        logger.LogInformation("Database seeding complete");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "An error occurred while applying database migrations.");
        throw;
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });
}

app.UseExceptionHandler();
app.UseForwardedHeaders();

// Forcing HTTPS
if (app.Environment.IsProduction())
{
    app.UseHsts();
}
app.UseHttpsRedirection();

app.UseRouting();
app.UseCors();
// app.UseIdempotency();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/health");
try
{
    Log.Information("Starting host...");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
