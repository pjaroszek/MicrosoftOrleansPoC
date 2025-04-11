using OrleansApp.Application.Common.Interfaces;

namespace OrleansApp.Application.Common.Handlers;

public interface IRequestHandler<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    Task<TResponse> HandleAsync(TRequest request, CancellationToken cancellationToken = default);
}