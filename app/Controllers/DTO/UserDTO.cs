namespace server_dotnet.Controllers.DTO
{
    public class UserDTO
    {
        public int Id { get; set; }

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public int OrganizationId { get; set; }

        public DateTime DateCreated { get; set; }
    }
}
