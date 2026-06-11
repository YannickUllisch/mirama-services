
using Serilog;
using Serilog.Events;

namespace Mirama.Api.Extensions;

public static class Test
{
    public static IApplicationBuilder UseRequestLogging(this IApplicationBuilder app)
{
    app.UseSerilogRequestLogging(options =>
    {
        // Customize the message template
        options.MessageTemplate = "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";
        
        // Smart log level filtering
        options.GetLevel = (httpContext, elapsed, ex) =>
        {
            if (httpContext.Request.Path.StartsWithSegments("/health"))
                return LogEventLevel.Verbose;
            
            if (ex != null || httpContext.Response.StatusCode >= 500)
                return LogEventLevel.Error;
            
            if (httpContext.Response.StatusCode >= 400)
                return LogEventLevel.Warning;
            
            return elapsed > 1000 ? LogEventLevel.Warning : LogEventLevel.Information;
        };
        
        // Enrich logs with additional context
        options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
        {
            diagnosticContext.Set("ClientIP", httpContext.Connection.RemoteIpAddress?.ToString());
            diagnosticContext.Set("UserAgent", httpContext.Request.Headers["User-Agent"].ToString());
            diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value);
            diagnosticContext.Set("ResponseStatusCode", httpContext.Response.StatusCode);
            
            if (httpContext.Items.TryGetValue("CorrelationId", out var correlationId))
            {
                diagnosticContext.Set("CorrelationId", correlationId);
            }
        };
    });

    return app;
}
}