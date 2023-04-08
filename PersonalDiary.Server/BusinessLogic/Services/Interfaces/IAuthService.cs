using SharedData.Models;
using SharedData.Models.User;
using System;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public interface IAuthService
    {
        Task<AuthenticationResult> RegisterAsync(UserIdentityRegisterDTO userRegisterDTO);
        Task<bool> RegisterFaceAsync(Guid userId);
        Task<AuthenticationResult> LoginAsync(UserIdentityLoginDTO userLoginDTO);
        Task<AuthenticationResult> LoginFaceAsync(UserIdentityLoginDTO userLoginDTO);
        Task<string> GetJwtTokenAsync(Guid userId);
    }
}
