using Microsoft.EntityFrameworkCore;
using server_dotnet.Domain.Entities;
using server_dotnet.Infrastructure.Data;

namespace server_dotnet.Infrastructure.Repositories
{
    public class UserRepository : IRepository<User>
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<IRepository<User>> _logger;

        public UserRepository(ApplicationDbContext context,
            ILogger<IRepository<User>> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<int> AddAsync(User entity)
        {
            _logger.LogInformation("Adding user: {UserName}", entity.FirstName + " " + entity.LastName);
            await CheckIfOrganizationExists(entity);

            _context.Users.Add(entity);
            await _context.SaveChangesAsync();
            _logger.LogInformation("User added with ID: {UserId}", entity.Id);
            return entity.Id;
        }

        private async Task CheckIfOrganizationExists(User entity)
        {
            var organizationExists = await _context.Organizations.AnyAsync(o => o.Id == entity.OrganizationId);
            if (!organizationExists)
            {
                _logger.LogError("Organization with ID {OrganizationId} does not exist for user {UserName}", entity.OrganizationId, entity.FirstName + " " + entity.LastName);
                throw new InvalidOperationException($"Organization with ID {entity.OrganizationId} does not exist.");
            }
        }

        public async Task DeleteAsync(int id)
        {
            _logger.LogInformation("Deleting user with ID: {UserId}", id);
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                var userOrders  = await _context.Orders.Where(o => o.UserId == id).ToListAsync();
                if (userOrders.Any())
                {
                    _logger.LogInformation("Deleting orders associated with user {UserName}", user.FirstName + " " + user.LastName);
                    _context.Orders.RemoveRange(userOrders);
                }

                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                _logger.LogInformation("User {UserName} deleted successfully", user.FirstName + " " + user.LastName);
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Users.AnyAsync(u => u.Id == id);
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task UpdateAsync(User entity)
        {
            _logger.LogInformation("Updating user with ID: {UserId}", entity.Id);
            await CheckIfOrganizationExists(entity);

            _context.Users.Update(entity);
            await _context.SaveChangesAsync();
            _logger.LogInformation("User {UserName} updated successfully", entity.FirstName + " " + entity.LastName);
        }
    }
}
