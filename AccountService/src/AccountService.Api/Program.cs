using Microsoft.OpenApi;
using AccountService.Application;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddCors(options => options.AddDefaultPolicy(
        policy => policy.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod()));

builder.Services.AddSwaggerGen(c => c.SwaggerDoc("v1", new OpenApiInfo { Title = "Mirama Account Service API", Version = "v1" }));

builder.Services.AddProblemDetails();

builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration);

builder.Services.AddHealthChecks();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Enable middleware to serve generated Swagger as a JSON endpoint.
app.UseSwagger();

// Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    options.RoutePrefix = string.Empty;
});

app.UseCors();

if (app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/error-development");
}
else
{
    app.UseHsts();
    app.UseHttpsRedirection();
    app.UseExceptionHandler("/error");
}

app.UseAuthorization();
app.MapControllers();

app.Run();
