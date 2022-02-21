using SharedData.Models;

namespace BusinessLogic.Services
{
    public interface IJwtService
    {
        AuthenticationResult GenerateAuthenticationResultForUser(User user);
    }
}
