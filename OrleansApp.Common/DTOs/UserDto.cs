using System;
using System.Collections.Generic;
using Orleans;


namespace OrleansApp.Common.DTOs
{
    [GenerateSerializer]
    public class UserDto
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
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Id(5)]
        public DateTime? UpdatedAt { get; set; }

        [Id(6)]
        public bool IsActive { get; set; } = true;

        [Id(7)]
        public List<string> Roles { get; set; } = new();

        // Właściwości wyliczane (computed) nie muszą być serializowane
        public string FullName => $"{FirstName} {LastName}".Trim();

        public UserDto() { }

        public UserDto(string id, string firstName, string lastName, string email)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
        }
    }
}