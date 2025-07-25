﻿namespace server_dotnet.Domain.Entities
{
    public class Organization
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Industry { get; set; } = string.Empty;

        public DateTime DateFounded { get; set; }
    }
}
