using Microsoft.Extensions.DependencyInjection;
using OrleansApp.Application.Common.Handlers;
using OrleansApp.Application.Common.Interfaces;

namespace OrleansApp.Application.Common;

public class Mediator : IMediator
{
    private readonly IServiceProvider _serviceProvider;

    public Mediator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<TResponse> SendQueryAsync<TResponse>(IQuery<TResponse> query, CancellationToken cancellationToken = default)
    {
        var handlerType = typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResponse));
        var handler = _serviceProvider.GetService(handlerType);
        
        if (handler == null)
            throw new InvalidOperationException($"Handler for {query.GetType().Name} not found");

        return await (Task<TResponse>)handlerType
            .GetMethod("HandleAsync")
            .Invoke(handler, new object[] { query, cancellationToken });
    }

    public async Task SendCommandAsync(ICommand command, CancellationToken cancellationToken = default)
    {
        var handlerType = typeof(ICommandHandler<>).MakeGenericType(command.GetType());
        var handler = _serviceProvider.GetService(handlerType);
        
        if (handler == null)
            throw new InvalidOperationException($"Handler for {command.GetType().Name} not found");

        await (Task)handlerType
            .GetMethod("HandleAsync")
            .Invoke(handler, new object[] { command, cancellationToken });
    }

    public async Task<TResult> SendCommandAsync<TResult>(ICommandWithResult<TResult> command, CancellationToken cancellationToken = default)
    {
        var handlerType = typeof(ICommandWithResultHandler<,>).MakeGenericType(command.GetType(), typeof(TResult));
        var handler = _serviceProvider.GetService(handlerType);
        
        if (handler == null)
            throw new InvalidOperationException($"Handler for {command.GetType().Name} not found");

        return await (Task<TResult>)handlerType
            .GetMethod("HandleAsync")
            .Invoke(handler, new object[] { command, cancellationToken });
    }

    public async Task PublishEventAsync(IEvent @event, CancellationToken cancellationToken = default)
    {
        var handlerType = typeof(IEventHandler<>).MakeGenericType(@event.GetType());
        var handlers = _serviceProvider.GetServices(handlerType);
        
        var tasks = handlers.Select(handler => 
            (Task)handlerType
                .GetMethod("HandleAsync")
                .Invoke(handler, new object[] { @event, cancellationToken }));
        
        await Task.WhenAll(tasks);
    }

    public async Task<TResponse> SendRequestAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        var handlerType = typeof(IRequestHandler<,>).MakeGenericType(request.GetType(), typeof(TResponse));
        var handler = _serviceProvider.GetService(handlerType);
        
        if (handler == null)
            throw new InvalidOperationException($"Handler for {request.GetType().Name} not found");

        return await (Task<TResponse>)handlerType
            .GetMethod("HandleAsync")
            .Invoke(handler, new object[] { request, cancellationToken });
    }
}