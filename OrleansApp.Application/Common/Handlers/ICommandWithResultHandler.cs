using OrleansApp.Application.Common.Interfaces;

namespace OrleansApp.Application.Common.Handlers;

public interface ICommandWithResultHandler<TCommand, TResult> where TCommand : ICommandWithResult<TResult>
{
    Task<TResult> HandleAsync(TCommand command, CancellationToken cancellationToken = default);
}