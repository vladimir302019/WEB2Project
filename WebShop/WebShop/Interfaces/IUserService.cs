using WebShop.DTO;
using WebShop.Models;

namespace WebShop.Interfaces
{
    public interface IUserService
    {
        TokenDTO Login(UserLoginDTO user);
        UserDTO Register(UserRegisterDTO user);
        List<UserDTO> GetAllUsers();
        List<UserDTO> GetAllUnactivatedUsers();
        UserRegisterDTO GetUser(long id);
        UserRegisterDTO UpdateUser(long id,UserRegisterDTO user);
        UserDTO ActivateUser(long id);
    }
}
