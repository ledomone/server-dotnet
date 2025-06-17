using server_dotnet.Controllers.DTO;

namespace server_dotnet.Services
{
    public interface IOrganizationService
    {
        Task<IEnumerable<OrganizationDTO>> GetAllAsync();
        Task<OrganizationDTO?> GetByIdAsync(int id);
        Task<OrganizationDTO> CreateAsync(OrganizationDTO organizationDTO);
        Task UpdateAsync(int id, OrganizationDTO organizationDTO);
        Task DeleteAsync(int id);
    }
}