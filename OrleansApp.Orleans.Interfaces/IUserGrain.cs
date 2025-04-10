using OrleansApp.Common.DTOs;

namespace OrleansApp.Orleans.Interfaces;

public interface IUserGrain : IGrainWithStringKey
{
    Task<UserDto> GetUserAsync();
    Task UpdateUserAsync(UserDto user);
    Task DeleteUserAsync();
    Task AddUserAsync(UserDto user);
}