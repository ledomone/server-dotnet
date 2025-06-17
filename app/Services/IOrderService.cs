using server_dotnet.Controllers.DTO;

namespace server_dotnet.Services
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderDTO>> GetAllAsync();
        Task<FullOrderDTO?> GetByIdAsync(int id);
        Task<OrderDTO> CreateAsync(OrderDTO orderDTO);
        Task UpdateAsync(int id, OrderDTO orderDTO);
        Task DeleteAsync(int id);
    }
}
