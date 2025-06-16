using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using server_dotnet.Controllers.DTO;
using server_dotnet.Domain.Entities;
using server_dotnet.Infrastructure.Repositories;

namespace server_dotnet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IRepository<Order> _orderRepository;

        public OrdersController(IRepository<Order> orderRepository)
        {
            _orderRepository = orderRepository;
        }

        // GET: api/Orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDTO>>> GetOrders()
        {
            var orders = await _orderRepository.GetAllAsync();
            return Ok(orders.ToDTOs().ToList());
        }

        // GET: api/Orders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FullOrderDTO>> GetOrder(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            return Ok(order.ToFullOrderDTO());
        }

        // PUT: api/Orders/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(int id, OrderDTO orderDTO)
        {
            if (id != orderDTO.Id)
            {
                return BadRequest();
            }

            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            order.OrderDate = orderDTO.OrderDate;
            order.TotalAmount = orderDTO.TotalAmount;
            order.UserId = orderDTO.UserId;
            order.OrganizationId = orderDTO.OrganizationId;

            try
            {
                await _orderRepository.UpdateAsync(order);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _orderRepository.ExistsAsync(id))
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }

        // POST: api/Orders
        [HttpPost]
        public async Task<ActionResult<OrderDTO>> PostOrder(OrderDTO orderDTO)
        {
            var order = new Order
            {
                OrderDate = orderDTO.OrderDate,
                TotalAmount = orderDTO.TotalAmount,
                UserId = orderDTO.UserId,
                OrganizationId = orderDTO.OrganizationId
            };
            
            await _orderRepository.AddAsync(order);

            return CreatedAtAction("GetOrder", new { id = order.Id }, order.ToDTO());
        }

        // DELETE: api/Orders/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            
            if (!await _orderRepository.ExistsAsync(id))
            {
                return NotFound();
            }

            await _orderRepository.DeleteAsync(id);

            return NoContent();
        }
    }
}
