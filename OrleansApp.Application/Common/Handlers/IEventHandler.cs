using OrleansApp.Application.Common.Interfaces;

namespace OrleansApp.Application.Common.Handlers;

public interface IEventHandler<TEvent> where TEvent : IEvent
{
    Task HandleAsync(TEvent @event, CancellationToken cancellationToken = default);
}