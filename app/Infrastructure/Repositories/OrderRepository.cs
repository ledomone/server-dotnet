using Microsoft.EntityFrameworkCore;
using server_dotnet.Domain.Entities;
using server_dotnet.Infrastructure.Data;

namespace server_dotnet.Infrastructure.Repositories
{
    public class OrderRepository : IRepository<Order>
    {
        private readonly ApplicationDbContext _context;
        public OrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<int> AddAsync(Order entity)
        {
            _context.Orders.Add(entity);
            await _context.SaveChangesAsync();
            return entity.Id;
        }
        public async Task DeleteAsync(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
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
            _context.Orders.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
