using WebShop.DTO;
using WebShop.Models;

namespace WebShop.Interfaces
{
    public interface IUserService
    {
        Task<TokenDTO> Login(UserLoginDTO user);
        Task<UserDTO> Register(UserRegisterDTO user);
        Task<List<UserDTO>> GetAllUsers();
        Task<List<UserDTO>> GetAllUnactivatedUsers();
        Task<UserRegisterDTO> GetUser(long id);
        Task<UserRegisterDTO> UpdateUser(long id,UserRegisterDTO user);
        Task<UserDTO> ActivateUser(long id, bool activate);
    }
}
