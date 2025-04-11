using OrleansApp.Application.Common.Interfaces;
using OrleansApp.Common.DTOs;

namespace OrleansApp.Application.User.Queries;

public record GetUserByIdQuery(string Id) : IQuery<UserDto>;