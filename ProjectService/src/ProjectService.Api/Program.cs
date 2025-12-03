using System.Net;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.IdentityModel.Tokens;
using ProjectService.Application;
using ProjectService.Infrastructure;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
// Configuration
builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables();

builder.Services.Configure<ProjectService.Infrastructure.Config.DbSettings>(builder.Configuration.GetSection("ConnectionStrings"));

// JWT Validation, source 
// https://stackoverflow.com/questions/53244446/how-to-validate-aws-cognito-jwt-in-net-core-web-api-using-addjwtbearer
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            // ValidIssuer = $"https://cognito-idp.{region}.amazonaws.com/{userPoolId}",
            ValidateAudience = false,
            ValidAudience = "{Cognito AppClientId}",
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            // Map cognito claims to .NET claims
            NameClaimType = "cognito:username", // or JwtClaimTypes.Name / ClaimTypes.Name if present
            RoleClaimType = "cognito:groups" // Cognito groups ->  roles
        };
        // OIDC discovery: pulls JWKS and validation settings automatically
        options.MetadataAddress =
            "https://cognito-idp.{region}.amazonaws.com/{userPoolId}/.well-known/openid-configuration";
        // Alternatively:
        // options.Authority = $"https://cognito-idp.{region}.amazonaws.com/{userPoolId}";
    });

// We want all HTTP Endpoints to require the JWT, to avoid the [Authorize] attribute we add policy
builder.Services.AddControllers(opt =>
{
    var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
    opt.Filters.Add(new AuthorizeFilter(policy));
});

// Versioning

builder.Services.AddApiVersioning(options =>
{
    options.ReportApiVersions = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    // API version read from the URL segment i.e. /v1/project
    options.ApiVersionReader = new UrlSegmentApiVersionReader();
});

// OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Serilog
builder.Host.UseSerilog((ctx, config) =>
    config.ReadFrom.Configuration(ctx.Configuration));

// Services
builder.Services.AddControllers();
builder.Services
    .AddInfrastructureServices(builder.Configuration)
    .AddApplicationServices();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
    app.UseHttpsRedirection();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();
app.UseRouting();
app.MapControllers();

app.Run();