using OrleansApp.Application.Common.Interfaces;

namespace OrleansApp.Application.Common.Handlers;

public interface IQueryHandler<TQuery, TResponse> where TQuery : IQuery<TResponse>
{
    Task<TResponse> HandleAsync(TQuery query, CancellationToken cancellationToken = default);
}