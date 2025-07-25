﻿using System.ComponentModel.DataAnnotations;

namespace server_dotnet.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        [Required]
        public int OrganizationId { get; set; }

        public Organization Organization { get; set; } = null!;

        public DateTime DateCreated { get; set; }
    }
}
