using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Abstractions;
using Microsoft.EntityFrameworkCore;
using AuthService.Server.Common.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using static OpenIddict.Abstractions.OpenIddictConstants;
using AuthService.Server.Infrastructure.BackgroundJobs;
using AuthService.Server.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// Options pattern Configuration
builder.Services.Configure<ApplicationOptions>(builder.Configuration.GetSection(ApplicationOptions.Application));
builder.Services.Configure<GoogleOptions>(builder.Configuration.GetSection(GoogleOptions.Google));


builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    })
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
    {
        options.LoginPath = "/auth/login";
    })
    .AddGoogle(options =>
    {
        var config = builder.Configuration.GetSection(GoogleOptions.Google).Get<GoogleOptions>() 
            ?? throw new InvalidOperationException("Google authentication configuration is missing or invalid.");
        options.ClientId = config.ClientId;
        options.ClientSecret = config.ClientSecret;
        options.Events.OnRedirectToAuthorizationEndpoint = context =>
        {
            context.Response.Redirect(context.RedirectUri + "&prompt=consent");
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
        var config = builder.Configuration.GetSection(ApplicationOptions.Application).Get<ApplicationOptions>() 
            ?? throw new InvalidOperationException("Application option configuration is missing or invalid.");

        options.SetTokenEndpointUris("/connect/token")
               .SetAuthorizationEndpointUris("/connect/authorize");
            // .SetEndSessionEndpointUris("connect/logout")
            //    .SetUserInfoEndpointUris("connect/userinfo");

        // Allow auth for registered Microservice clients and User clients
        // and Token exchange flow for organization/tenant switches
        options.AllowClientCredentialsFlow()
               .AllowAuthorizationCodeFlow()
               .AllowTokenExchangeFlow();
        
        // Adding PKCE
        options.RequireProofKeyForCodeExchange();

        options.UseDataProtection();
        
        options.AddEphemeralEncryptionKey()
               .AddEphemeralSigningKey()
               .DisableAccessTokenEncryption();

        options.RegisterAudiences("api://account", "api://project");

        // Issuer refers to this Auth Server, hardcoded for testing purposes
        options.SetIssuer(new Uri("http://localhost:8085"));

        options.SetAccessTokenLifetime(TimeSpan.FromMinutes(30));
        options.SetRefreshTokenLifetime(TimeSpan.FromDays(30));

        options.UseAspNetCore()
               .EnableTokenEndpointPassthrough()
               .EnableAuthorizationEndpointPassthrough();
            //    .EnableEndSessionEndpointPassthrough()
            //    .EnableStatusCodePagesIntegration()
            //    .EnableUserInfoEndpointPassthrough();

        options.AddSigningKey(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.SigningKey)));

        options.RegisterScopes(Scopes.OpenId, Scopes.Roles, Scopes.OfflineAccess, Scopes.Profile, Scopes.Email, "organization");
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
    options.UseOpenIddict();
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
    app.UseHttpsRedirection();
}


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
