
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using AuthService.Server.Common.Options;
using Microsoft.Extensions.Options;
using AuthService.Server.Infrastructure.BackgroundJobs;
using AuthService.Server.Infrastructure.Persistence;
using AuthService.Server.Infrastructure.OpenIddict;
using AuthService.Application;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// Options pattern Configuration
builder.Services.Configure<ApplicationOptions>(builder.Configuration.GetSection(ApplicationOptions.Application));
builder.Services.Configure<GoogleOptions>(builder.Configuration.GetSection(GoogleOptions.Google));
builder.Services.Configure<OAuthClientOptions>(builder.Configuration.GetSection(OAuthClientOptions.Clients));

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

builder.Services.AddHostedService<ClientWorker>();

builder.Services.AddDbContext<OpenIdDbContext>((sp, options) =>
{
    var infra = sp.GetRequiredService<IOptions<ApplicationOptions>>().Value;
    options.UseNpgsql(infra.DatabaseConnection, b => b
        .MigrationsAssembly(typeof(OpenIdDbContext).Assembly.FullName)
        .MigrationsHistoryTable(tableName: "__EFMigrationsHistory", schema: "auth"));
});

// Application Services DI injection
builder.Services.AddApplication();

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

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
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

app.Run();
