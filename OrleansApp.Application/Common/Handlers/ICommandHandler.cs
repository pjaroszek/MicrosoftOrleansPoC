using OrleansApp.Application.Common.Interfaces;

namespace OrleansApp.Application.Common.Handlers;

public interface ICommandHandler<TCommand> where TCommand : ICommand
{
    Task HandleAsync(TCommand command, CancellationToken cancellationToken = default);
}