using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using server_dotnet.Controllers.DTO;
using server_dotnet.Services;

namespace server_dotnet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrganizationsController : ControllerBase
    {
        private readonly IOrganizationService _organizationService;

        public OrganizationsController(IOrganizationService organizationService)
        {
            _organizationService = organizationService;
        }

        // GET: api/Organizations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrganizationDTO>>> GetOrganizations()
        {
            var organizations = await _organizationService.GetAllAsync();

            return Ok(organizations);
        }

        // GET: api/Organizations/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OrganizationDTO>> GetOrganization(int id)
        {
            var organization = await _organizationService.GetByIdAsync(id);

            if (organization == null)
            {
                return NotFound();
            }

            return Ok(organization);
        }

        // PUT: api/Organizations/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrganization(int id, OrganizationDTO organizationDTO)
        {
            try
            {
                await _organizationService.UpdateAsync(id, organizationDTO);
                return NoContent();
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Errors);
            }
            catch (ArgumentException)
            {
                return BadRequest("Organization ID mismatch.");
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        // POST: api/Organizations
        [HttpPost]
        public async Task<ActionResult<OrganizationDTO>> PostOrganization(OrganizationDTO organizationDTO)
        {
            try
            {
                var createdOrganization = await _organizationService.CreateAsync(organizationDTO);
                return CreatedAtAction(nameof(GetOrganization), new { id = createdOrganization.Id }, createdOrganization);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Errors);
            }
        }

        // DELETE: api/Organizations/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrganization(int id)
        {
            try 
            {
                await _organizationService.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
