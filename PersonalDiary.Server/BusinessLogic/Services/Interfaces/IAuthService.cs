using SharedData.Models;
using System;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public interface IAuthService
    {
        Task<AuthenticationResult> RegisterAsync(UserRegisterDTO userRegisterDTO);
        Task<bool> RegisterFaceAsync(Guid userId);
        Task<AuthenticationResult> LoginAsync(UserLoginDTO userLoginDTO);
        Task<AuthenticationResult> LoginFaceAsync(UserLoginDTO userLoginDTO);
        Task<string> GetJwtTokenAsync(Guid userId);
    }
}
