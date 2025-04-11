using OrleansApp.Application.Common.Interfaces;

namespace OrleansApp.Application.Common;

public interface IMediator
{
    Task<TResponse> SendQueryAsync<TResponse>(IQuery<TResponse> query, CancellationToken cancellationToken = default);
    Task SendCommandAsync(ICommand command, CancellationToken cancellationToken = default);
    Task<TResult> SendCommandAsync<TResult>(ICommandWithResult<TResult> command, CancellationToken cancellationToken = default);
    Task PublishEventAsync(IEvent @event, CancellationToken cancellationToken = default);
    Task<TResponse> SendRequestAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default);
}