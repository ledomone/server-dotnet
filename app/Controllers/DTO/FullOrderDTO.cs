namespace server_dotnet.Controllers.DTO
{
    public class FullOrderDTO : OrderDTO
    {
        public UserDTO User { get; set; } = new UserDTO();
        public OrganizationDTO Organization { get; set; } = new OrganizationDTO();
    }
}
