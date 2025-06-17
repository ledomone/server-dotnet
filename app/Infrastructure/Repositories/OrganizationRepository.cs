using Microsoft.EntityFrameworkCore;
using server_dotnet.Domain.Entities;
using server_dotnet.Infrastructure.Data;

namespace server_dotnet.Infrastructure.Repositories
{
    public class OrganizationRepository : IRepository<Organization>
    {
        private readonly ApplicationDbContext _context;

        public OrganizationRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<int> AddAsync(Organization entity)
        {
            _context.Organizations.Add(entity);
            await _context.SaveChangesAsync();
            return entity.Id;
        }

        public async Task DeleteAsync(int id)
        {
            var organization = await _context.Organizations.FindAsync(id);
            if (organization != null)
            {
                _context.Organizations.Remove(organization);
                await _context.SaveChangesAsync();
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
            _context.Organizations.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
