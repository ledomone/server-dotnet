using Microsoft.EntityFrameworkCore;
using server_dotnet.Domain.Entities;
using server_dotnet.Infrastructure.Data;

namespace server_dotnet.Infrastructure.Repositories
{
    public class OrderRepository : IRepository<Order>
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<IRepository<Order>> _logger;

        public OrderRepository(ApplicationDbContext context,
            ILogger<IRepository<Order>> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<int> AddAsync(Order entity)
        {
            _logger.LogInformation("Adding order for User ID: {UserId}, Organization ID: {OrganizationId}", entity.UserId, entity.OrganizationId);
            await CheckDependencies(entity);

            _context.Orders.Add(entity);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Order added with ID: {OrderId}", entity.Id);
            return entity.Id;
        }
        public async Task DeleteAsync(int id)
        {
            _logger.LogInformation("Deleting order with ID: {OrderId}", id);
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Order with ID {OrderId} deleted successfully", id);
            }
        }
        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Orders.AnyAsync(o => o.Id == id);
        }
        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            return await _context.Orders.ToListAsync();
        }
        public async Task<Order?> GetByIdAsync(int id)
        {
            return await _context.Orders
                .Include(o => o.User)
                .Include(o => o.Organization)
                .FirstOrDefaultAsync(o => o.Id == id);
        }
        public async Task UpdateAsync(Order entity)
        {
            _logger.LogInformation("Updating order with ID: {OrderId}", entity.Id);
            await CheckDependencies(entity);

            _context.Orders.Update(entity);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Order with ID {OrderId} updated successfully", entity.Id);
        }

        private async Task CheckDependencies(Order entity)
        {
            await CheckIfOrganizationExists(entity);
            await CheckIfUserExists(entity);
        }

        private async Task CheckIfOrganizationExists(Order entity)
        {
            var organizationExists = await _context.Organizations.AnyAsync(o => o.Id == entity.OrganizationId);
            if (!organizationExists)
            {
                _logger.LogError("Organization with ID {OrganizationId} does not exist for order with ID {OrderId}", entity.OrganizationId, entity.Id);
                throw new InvalidOperationException($"Organization with ID {entity.OrganizationId} does not exist.");
            }
        }

        private async Task CheckIfUserExists(Order entity)
        {
            var userExists = await _context.Users.AnyAsync(u => u.Id == entity.UserId);
            if (!userExists)
            {
                _logger.LogError("User with ID {UserId} does not exist for order with ID {OrderId}", entity.UserId, entity.Id);
                throw new InvalidOperationException($"User with ID {entity.UserId} does not exist.");
            }
        }
    }
}
