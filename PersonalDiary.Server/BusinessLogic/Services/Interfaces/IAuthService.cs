using SharedData.Models;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public interface IAuthService
    {
        Task RegisterAsync(UserRegisterDTO userRegisterDTO);
        User Login(UserLoginDTO userLoginDTO);
    }
}
