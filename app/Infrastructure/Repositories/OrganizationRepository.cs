using Microsoft.EntityFrameworkCore;
using server_dotnet.Domain.Entities;
using server_dotnet.Infrastructure.Data;

namespace server_dotnet.Infrastructure.Repositories
{
    public class OrganizationRepository : IRepository<Organization>
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<IRepository<Organization>> _logger;

        public OrganizationRepository(ApplicationDbContext context,
            ILogger<IRepository<Organization>> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<int> AddAsync(Organization entity)
        {
            _logger.LogInformation("Adding organization: {OrganizationName}", entity.Name);

            _context.Organizations.Add(entity);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Organization added with ID: {OrganizationId}", entity.Id);
            return entity.Id;
        }

        public async Task DeleteAsync(int id)
        {
            _logger.LogInformation("Deleting organization with ID: {OrganizationId}", id);
            var organization = await _context.Organizations.FindAsync(id);
            if (organization != null)
            {
                _context.Organizations.Remove(organization);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Organization {OrganizationName} deleted successfully", organization.Name);
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Organizations.AnyAsync(o => o.Id == id);
        }

        public async Task<IEnumerable<Organization>> GetAllAsync()
        {
            return await _context.Organizations.ToListAsync();
        }

        public async Task<Organization?> GetByIdAsync(int id)
        {
            return await _context.Organizations.FindAsync(id);
        }

        public async Task UpdateAsync(Organization entity)
        {
            _logger.LogInformation("Updating organization with ID: {OrganizationId}", entity.Id);
            _context.Organizations.Update(entity);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Organization updated: {OrganizationId}", entity.Id);
        }
    }
}
