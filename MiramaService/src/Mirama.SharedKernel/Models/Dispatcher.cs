
using Microsoft.Extensions.DependencyInjection;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;

namespace Mirama.SharedKernel.Models;

public class Dispatcher(IServiceProvider serviceProvider) : IDispatcher
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public async Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        var handlerType = typeof(IRequestHandler<,>).MakeGenericType(request.GetType(), typeof(TResponse));
        dynamic handler = _serviceProvider.GetRequiredService(handlerType);
        return await handler.HandleAsync((dynamic)request, cancellationToken);
    }

    public async Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default)
        where TNotification : INotification
    {
        ArgumentNullException.ThrowIfNull(notification);

        var handlerType = typeof(INotificationHandler<>).MakeGenericType(notification.GetType());
        var handlers = _serviceProvider.GetServices(handlerType).OfType<object>();

        foreach (dynamic handler in handlers)
        {
            await handler.HandleAsync((dynamic)notification, cancellationToken);
        }
    }
}
