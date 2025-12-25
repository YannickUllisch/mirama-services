using Microsoft.OpenApi;
using AccountService.Application;
using AccountService.Application.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using AccountService.Api.Middleware;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;
using AccountService.Api.Auth;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddCors(options => options
    .AddDefaultPolicy(
        policy => policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod()));

builder.Services.AddSwaggerGen(c => c
    .SwaggerDoc("v1", new OpenApiInfo { Title = "Mirama Account Service API", Version = "v1" }));

builder.Services.AddProblemDetails();

builder.Services
    .AddApplication(builder.Configuration)
    .AddInfrastructure(builder.Configuration);

builder.Services.AddLogging();

builder.Services.AddHealthChecks();
builder.Services.AddHttpContextAccessor();

var authSection = builder.Configuration.GetSection("Authentication");

builder.Services.AddAuthentication()
    .AddJwtBearer(options =>
    {
        options.Authority = authSection["Authority"];
        options.Audience = authSection["Audience"];     
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateLifetime = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = authSection["Authority"],
            ValidAudience = authSection["Audience"],
        };
    });

builder.Services.AddSingleton<IAuthorizationHandler, RequireTenantAndOrgHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, RequireTenantOnlyHandler>();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireTenantAndOrg", policy =>
        policy.Requirements.Add(new TenantAndOrgRequirement()));

    options.AddPolicy("RequireTenantOnly", policy =>
        policy.Requirements.Add(new TenantOnlyRequirement()));
});

var app = builder.Build();

// Applying migrations immediately in Development, handled in CICD pipeline for production
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    try
    {
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        logger.LogInformation("Applying database migrations...");
        await db.Database.MigrateAsync();
        logger.LogInformation("Database migrations applied successfully");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "An error occurred while applying database migrations.");
        throw;
    }
}

app.UseExceptionHandler();

// Forcing HTTPS
if (app.Environment.IsProduction())
{
    app.UseHsts();
    app.UseHttpsRedirection();
}

// Swagger Middleware
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    options.RoutePrefix = string.Empty;
});
app.UseRouting();
app.UseCors();
app.UseIdempotency();


// app.UseAuthentication();
// app.UseAuthorization();
app.MapControllers();

app.Run();
