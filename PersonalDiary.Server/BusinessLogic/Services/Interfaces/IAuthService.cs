using SharedData.Models;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public interface IAuthService
    {
        Task<AuthenticationResult> RegisterAsync(UserRegisterDTO userRegisterDTO);
        Task<AuthenticationResult> LoginAsync(UserLoginDTO userLoginDTO);
    }
}
