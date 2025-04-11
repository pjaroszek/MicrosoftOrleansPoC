using OrleansApp.Application.Common.Handlers;
using OrleansApp.Application.User.Queries;
using OrleansApp.Common.DTOs;
using OrleansApp.Common.Exceptions;
using OrleansApp.Orleans.Interfaces;

namespace OrleansApp.Application.User.QueryHandlers;

public class GetUserByIdQueryHandler(IClusterClient clusterClient) : IQueryHandler<GetUserByIdQuery, UserDto>
{
    private readonly IClusterClient clusterClient = clusterClient;

    public async Task<UserDto> HandleAsync(GetUserByIdQuery query, CancellationToken cancellationToken = default)
    {
        var userGrain = clusterClient.GetGrain<IUserGrain>(query.Id);
    
        try
        {
            var user = await userGrain.GetUserAsync();
            return user;
        }
        catch (NotFoundException ex)
        {
            throw new OrleansException($"User {query.Id} not found", ex);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}