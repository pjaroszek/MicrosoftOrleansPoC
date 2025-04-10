using OrleansApp.Common.DTOs;
using System;
using System.Collections.Generic;
using Orleans;

namespace OrleansApp.Domain.Entities
{
    [GenerateSerializer]
    [Serializable]
    public class UserState
    {
        [Id(0)]
        public string Id { get; set; } = string.Empty;

        [Id(1)]
        public string FirstName { get; set; } = string.Empty;

        [Id(2)]
        public string LastName { get; set; } = string.Empty;

        [Id(3)]
        public string Email { get; set; } = string.Empty;

        [Id(4)]
        public string PasswordHash { get; set; } = string.Empty;

        [Id(5)]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Id(6)]
        public DateTime? UpdatedAt { get; set; }

        [Id(7)]
        public bool IsActive { get; set; } = true;

        [Id(8)]
        public List<string> Roles { get; set; } = new();

        public UserDto ToDto()
        {
            return new UserDto
            {
                Id = Id,
                FirstName = FirstName,
                LastName = LastName,
                Email = Email,
                CreatedAt = CreatedAt,
                UpdatedAt = UpdatedAt,
                IsActive = IsActive,
                Roles = new List<string>(Roles)
            };
        }

        public static UserState FromDto(UserDto dto)
        {
            return new UserState
            {
                Id = dto.Id,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                CreatedAt = dto.CreatedAt,
                UpdatedAt = DateTime.UtcNow,
                IsActive = dto.IsActive,
                Roles = new List<string>(dto.Roles)
            };
        }
    }
}