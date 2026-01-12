using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using AuthService.Application;
using AuthService.Application.Infrastructure.OpenIddict;
using AuthService.Application.Infrastructure.Persistence;
using AuthService.Application.Common.Options;
using AuthService.Server.Middleware;
using Serilog;

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

builder.Services.AddControllersWithViews();

builder.Services.AddProblemDetails();
builder.Services.AddHealthChecks();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddLogging();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    })
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
    {
        options.LoginPath = "/auth/login";
        options.Cookie.SameSite = SameSiteMode.None;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    })
    .AddGoogle(options =>
    {
        var config = builder.Configuration.GetSection(GoogleOptions.Google).Get<GoogleOptions>()
            ?? throw new InvalidOperationException("Google authentication configuration is missing or invalid.");
        options.ClientId = config.ClientId;
        options.ClientSecret = config.ClientSecret;
        options.CallbackPath = "/auth/login/callback/google";
        options.CorrelationCookie.SameSite = SameSiteMode.None;
        options.CorrelationCookie.SecurePolicy = CookieSecurePolicy.Always;
        options.Events.OnRedirectToAuthorizationEndpoint = context =>
        {
            context.Response.Redirect(context.RedirectUri + "&prompt=select_account");
            return Task.CompletedTask;
        };
    });

// OpenIddict Configuration
builder.Services.AddOpenIddict()
    .AddCore(options =>
    {
        options.UseEntityFrameworkCore()
               .UseDbContext<OpenIdDbContext>();

    })
    .AddServer((options) =>
    {
        options.SetTokenEndpointUris("/connect/token")
                .SetAuthorizationEndpointUris("/connect/authorize")
                .SetEndSessionEndpointUris("/connect/logout");

        // Allow auth for registered Microservice clients and User clients
        // and Token exchange flow for organization/tenant switches
        options.AllowClientCredentialsFlow()
               .AllowAuthorizationCodeFlow()
               .AllowRefreshTokenFlow()
               .AllowTokenExchangeFlow();

        // TODO: Add Valid Certificate for Production
        if (builder.Environment.IsDevelopment())
        {
            options.AddDevelopmentSigningCertificate()
                .AddDevelopmentEncryptionCertificate();
        }

        options.UseAspNetCore()
               .EnableTokenEndpointPassthrough()
               .EnableAuthorizationEndpointPassthrough();

        // Main configuration of server
        options.ConfigureServer(builder.Configuration);
    });

// CORS Policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

// Application Services DI injection
builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    try
    {
        var db = scope.ServiceProvider.GetRequiredService<OpenIdDbContext>();
        logger.LogInformation("Applying database migrations...");
        if (db.Database.GetPendingMigrations().Any())
        {
            await db.Database.MigrateAsync();
        }
        logger.LogInformation("Database migrations applied successfully");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "An error occurred while applying database migrations.");
        throw;
    }
}

app.UseMiddleware<ExceptionMiddleware>(); // Handles API errors
app.UseExceptionHandler("/Home/Error"); 

// Forcing HTTPS
if (app.Environment.IsProduction())
{
    app.UseHsts();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Required for production, since TLS is terminated on proxy
app.UseForwardedHeaders();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

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
