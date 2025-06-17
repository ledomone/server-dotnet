using server_dotnet.Controllers.DTO;

namespace server_dotnet.Services
{
    public interface IUserService
    {
        Task<IEnumerable<UserDTO>> GetAllAsync();
        Task<UserDTO?> GetByIdAsync(int id);
        Task<UserDTO> CreateAsync(UserDTO userDTO);
        Task UpdateAsync(int id, UserDTO userDTO);
        Task DeleteAsync(int id);
    }
}
