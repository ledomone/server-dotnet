using server_dotnet.Controllers.DTO;
using server_dotnet.Domain.Entities;
using server_dotnet.Infrastructure.Repositories;

namespace server_dotnet.Services
{
    public class OrganizationService : IOrganizationService
    {
        private readonly IRepository<Organization> _organizationRepository;

        public OrganizationService(IRepository<Organization> organizationRepository)
        {
            _organizationRepository = organizationRepository;
        }
        public async Task<OrganizationDTO> CreateAsync(OrganizationDTO organizationDTO)
        {
            var organization = new Organization
            {
                Name = organizationDTO.Name,
                Industry = organizationDTO.Industry,
                DateFounded = organizationDTO.DateFounded
            };

            var id = await _organizationRepository.AddAsync(organization);
            organization.Id = id;
            return organization.ToDTO();
        }

        public async Task DeleteAsync(int id)
        {
            if (!await _organizationRepository.ExistsAsync(id))
            {
                throw new KeyNotFoundException($"Organization with ID {id} not found.");
            }

            await _organizationRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<OrganizationDTO>> GetAllAsync()
        {
            var organizations = await _organizationRepository.GetAllAsync();
            return organizations.ToDTOs();
        }

        public async Task<OrganizationDTO?> GetByIdAsync(int id)
        {
            var organization = await _organizationRepository.GetByIdAsync(id);
            return organization?.ToDTO();
        }

        public async Task UpdateAsync(int id, OrganizationDTO organizationDTO)
        {
            if (id != organizationDTO.Id)
            {
                throw new ArgumentException("Organization ID mismatch.");
            }

            var organization = await _organizationRepository.GetByIdAsync(organizationDTO.Id);
            if (organization == null)
            {
                throw new KeyNotFoundException($"Organization with ID {id} not found.");
            }

            organization.Name = organizationDTO.Name;
            organization.Industry = organizationDTO.Industry;
            organization.DateFounded = organizationDTO.DateFounded;

            await _organizationRepository.UpdateAsync(organization);
        }
    }
}
