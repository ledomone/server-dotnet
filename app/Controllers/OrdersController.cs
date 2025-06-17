using Microsoft.AspNetCore.Mvc;
using server_dotnet.Controllers.DTO;
using server_dotnet.Services;

namespace server_dotnet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        // GET: api/Orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDTO>>> GetOrders()
        {
            var orders = await _orderService.GetAllAsync();
            return Ok(orders);
        }

        // GET: api/Orders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FullOrderDTO>> GetOrder(int id)
        {
            var order = await _orderService.GetByIdAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }

        // PUT: api/Orders/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(int id, OrderDTO orderDTO)
        {
            try
            {
                await _orderService.UpdateAsync(id, orderDTO);
                return NoContent();
            }
            catch (ArgumentException)
            {
                return BadRequest("Order ID mismatch.");
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        // POST: api/Orders
        [HttpPost]
        public async Task<ActionResult<OrderDTO>> PostOrder(OrderDTO orderDTO)
        {
            var createdOrder = await _orderService.CreateAsync(orderDTO);

            return CreatedAtAction(nameof(GetOrder), new { id = createdOrder.Id }, createdOrder);
        }

        // DELETE: api/Orders/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {

            try
            {
                await _orderService.DeleteAsync(id);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
