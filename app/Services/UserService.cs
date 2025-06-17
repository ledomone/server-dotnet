using server_dotnet.Controllers.DTO;
using server_dotnet.Domain.Entities;
using server_dotnet.Infrastructure.Repositories;

namespace server_dotnet.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> _userRepository;

        public UserService(IRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<UserDTO> CreateAsync(UserDTO userDTO)
        {
            var user = new User
            {
                FirstName = userDTO.FirstName,
                LastName = userDTO.LastName,
                Email = userDTO.Email,
                OrganizationId = userDTO.OrganizationId
            };

            var id = await _userRepository.AddAsync(user);
            user.Id = id;
            return user.ToDTO();
        }

        public async Task DeleteAsync(int id)
        {
            if (!await _userRepository.ExistsAsync(id))
            {
                throw new KeyNotFoundException($"User with ID {id} not found.");
            }

            await _userRepository.DeleteAsync(id);  
        }

        public async Task<IEnumerable<UserDTO>> GetAllAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return users.ToDTOs();
        }

        public async Task<UserDTO?> GetByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            return user?.ToDTO();
        }

        public async Task UpdateAsync(int id, UserDTO userDTO)
        {
            if (id != userDTO.Id)
            {
                throw new ArgumentException("User ID mismatch.");
            }

            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                throw new KeyNotFoundException($"User with ID {id} not found.");
            }

            user.FirstName = userDTO.FirstName;
            user.LastName = userDTO.LastName;
            user.Email = userDTO.Email;
            user.OrganizationId = userDTO.OrganizationId;
            user.DateCreated = userDTO.DateCreated;

            await _userRepository.UpdateAsync(user);
        }
    }
}
