using FluentValidation;
using server_dotnet.Controllers.DTO;
using server_dotnet.Domain.Entities;
using server_dotnet.Infrastructure.Repositories;

namespace server_dotnet.Services
{
    public class OrderService : IOrderService
    {
        private readonly IRepository<Order> _orderRepository;
        private readonly IValidator<OrderDTO> _orderValidator;

        public OrderService(IRepository<Order> orderRepository, IValidator<OrderDTO> orderValidator)
        {
            _orderRepository = orderRepository;
            _orderValidator = orderValidator;
        }
        public async Task<OrderDTO> CreateAsync(OrderDTO orderDTO)
        {
            await Validate(orderDTO);

            var order = new Order
            {
                OrderDate = orderDTO.OrderDate,
                TotalAmount = orderDTO.TotalAmount,
                UserId = orderDTO.UserId,
                OrganizationId = orderDTO.OrganizationId
            };

            try
            {
                var id = await _orderRepository.AddAsync(order);
                order.Id = id;
                return order.ToDTO();
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException($"Failed to create order: {ex.Message}");
            }
        }

        public async Task DeleteAsync(int id)
        {
            if (!await _orderRepository.ExistsAsync(id))
            {
                throw new KeyNotFoundException($"Order with ID {id} not found.");
            }
            await _orderRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<OrderDTO>> GetAllAsync()
        {
            var orders = await _orderRepository.GetAllAsync();
            return orders.ToDTOs();
        }

        public async Task<FullOrderDTO?> GetByIdAsync(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            return order?.ToFullOrderDTO();

        }

        public async Task UpdateAsync(int id, OrderDTO orderDTO)
        {
            await Validate(orderDTO);

            if (id != orderDTO.Id)
            {
                throw new ArgumentException("Order ID mismatch.");
            }

            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null)
            {
                throw new KeyNotFoundException($"Order with ID {id} not found.");
            }

            order.OrderDate = orderDTO.OrderDate;
            order.TotalAmount = orderDTO.TotalAmount;
            order.UserId = orderDTO.UserId;
            order.OrganizationId = orderDTO.OrganizationId;

            try
            {
                await _orderRepository.UpdateAsync(order);
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException($"Failed to update order: {ex.Message}");
            }
        }

        private async Task Validate(OrderDTO orderDTO)
        {
            var validationResult = await _orderValidator.ValidateAsync(orderDTO);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
        }
    }
}
