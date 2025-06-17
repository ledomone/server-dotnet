using Microsoft.AspNetCore.Mvc;
using server_dotnet.Controllers.DTO;
using server_dotnet.Services;

namespace server_dotnet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsers()
        {
            var users = await _userService.GetAllAsync();

            return Ok(users);
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDTO>> GetUser(int id)
        {
            var user = await _userService.GetByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        // PUT: api/Users/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, UserDTO userDTO)
        {
            try
            {
                await _userService.UpdateAsync(id, userDTO);
                return NoContent();
            }
            catch (ArgumentException)
            {
                return BadRequest("User ID mismatch.");
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        // POST: api/Users
        [HttpPost]
        public async Task<ActionResult<UserDTO>> PostUser(UserDTO userDTO)
        {
            var createdUser = await _userService.CreateAsync(userDTO);

            return CreatedAtAction(nameof(GetUser), new { id = createdUser.Id }, createdUser);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                await _userService.DeleteAsync(id);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
