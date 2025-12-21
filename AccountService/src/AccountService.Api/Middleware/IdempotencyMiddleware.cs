

using Microsoft.Extensions.Caching.Distributed;

namespace AccountService.Api.Middleware;

public class IdempotencyMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        // Only apply Idempotency handling to Post requests
        if (context.Request.Method != HttpMethods.Post)
        {
            await _next(context);
            return;
        }

        if (!context.Request.Headers.TryGetValue("X-Idempotency-Key", out var key))
        {
            context.Response.StatusCode = 400;
            await context.Response.WriteAsync("Missing X-Idempotency-Key header");
            return;
        }

        // TODO: Idempotency Handling
        var cache = context.RequestServices.GetRequiredService<IDistributedCache>();
        
        await _next(context);
    }

}

public static class IdempotencyMiddlewareExtensions
{
    public static IApplicationBuilder UseIdempotency(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<IdempotencyMiddleware>();
    }
}