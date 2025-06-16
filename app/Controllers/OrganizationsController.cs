using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using server_dotnet.Controllers.DTO;
using server_dotnet.Domain.Entities;
using server_dotnet.Infrastructure.Repositories;

namespace server_dotnet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrganizationsController : ControllerBase
    {
        private readonly IRepository<Organization> _organizationRepository;

        public OrganizationsController(IRepository<Organization> organizationRepository)
        {
            _organizationRepository = organizationRepository;
        }

        // GET: api/Organizations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrganizationDTO>>> GetOrganizations()
        {
            var organizations = await _organizationRepository.GetAllAsync();

            return Ok(organizations.ToDTOs().ToList());
        }

        // GET: api/Organizations/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OrganizationDTO>> GetOrganization(int id)
        {
            var organization = await _organizationRepository.GetByIdAsync(id);

            if (organization == null)
            {
                return NotFound();
            }

            return Ok(organization.ToDTO());
        }

        // PUT: api/Organizations/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrganization(int id, OrganizationDTO organizationDTO)
        {
            if (id != organizationDTO.Id)
            {
                return BadRequest();
            }

            var organization = await _organizationRepository.GetByIdAsync(id);
            if (organization == null)
            {
                return NotFound();
            }

            organization.Name = organizationDTO.Name;
            organization.Industry = organizationDTO.Industry;
            organization.DateFounded = organizationDTO.DateFounded;

            try
            {
                await _organizationRepository.UpdateAsync(organization);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _organizationRepository.ExistsAsync(id))
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }

        // POST: api/Organizations
        [HttpPost]
        public async Task<ActionResult<OrganizationDTO>> PostOrganization(OrganizationDTO organizationDTO)
        {
            var organization = new Organization
            {
                Name = organizationDTO.Name,
                Industry = organizationDTO.Industry,
                DateFounded = organizationDTO.DateFounded
            };

            await _organizationRepository.AddAsync(organization);

            return CreatedAtAction("GetOrganization", new { id = organization.Id }, organization.ToDTO());
        }

        // DELETE: api/Organizations/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrganization(int id)
        {
            
            if (!await _organizationRepository.ExistsAsync(id))
            {
                return NotFound();
            }

            await _organizationRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}
