// W projekcie OrleansApp.Orleans.Grains

using Microsoft.Extensions.Logging;
using Orleans.Runtime;
using OrleansApp.Common.DTOs;
using OrleansApp.Common.Exceptions;
using OrleansApp.Domain.Entities;
using OrleansApp.Orleans.Interfaces;

namespace OrleansApp.Orleans.Grains;

public class UserGrain : Grain, OrleansApp.Orleans.Interfaces.IUserGrain
{
    private readonly IPersistentState<UserState> _state;
    private readonly ILogger<UserGrain> _logger;

    public UserGrain(
        [PersistentState("user", "SqlStateStore")] IPersistentState<UserState> state,
        ILogger<UserGrain> logger)
    {
        _state = state;
        _logger = logger;
    }

    public Task<UserDto> GetUserAsync()
    {
        _logger.LogInformation("Pobieranie użytkownika {UserId}", this.GetPrimaryKeyString());
        
        if (_state.State == null || string.IsNullOrEmpty(_state.State.Id))
        {
            throw new NotFoundException($"Użytkownik o ID {this.GetPrimaryKeyString()} nie istnieje");
        }

        return Task.FromResult(_state.State.ToDto());
    }

    public async Task UpdateUserAsync(UserDto user)
    {
        _logger.LogInformation("Aktualizacja użytkownika {UserId}", this.GetPrimaryKeyString());
        
        _state.State = UserState.FromDto(user);
        await _state.WriteStateAsync();
    }

    public async Task DeleteUserAsync()
    {
        _logger.LogInformation("Usuwanie użytkownika {UserId}", this.GetPrimaryKeyString());
        
        await _state.ClearStateAsync();
    }
    
    public async Task AddUserAsync(UserDto user)
    {
        _logger.LogInformation("Dodawanie użytkownika {UserId}", this.GetPrimaryKeyString());
        _state.State = UserState.FromDto(user);
        await _state.WriteStateAsync();
    }
}