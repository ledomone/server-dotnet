namespace server_dotnet.Controllers.DTO
{
    public class OrganizationDTO
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Industry { get; set; } = string.Empty;

        public DateTime DateFounded { get; set; }
    }
}
