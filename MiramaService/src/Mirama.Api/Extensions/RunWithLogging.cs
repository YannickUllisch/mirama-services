
using Serilog;

namespace Mirama.Api.Extensions;

public static class ApplicationExtensions
{
    public static async Task RunWithLoggingAsync(this WebApplication app)
    {
        try
        {
            Log.Information("Starting application {Application} in {Environment}", 
                app.Environment.ApplicationName, 
                app.Environment.EnvironmentName);
            
            Log.Information("Application listening on {Urls}", 
                string.Join(", ", app.Urls));

            await app.RunAsync();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Application terminated unexpectedly");
            throw;
        }
        finally
        {
            Log.Information("Application shutting down");
            await Log.CloseAndFlushAsync();
        }
    }
}
