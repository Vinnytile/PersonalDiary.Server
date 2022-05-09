using SharedData.Models;

namespace BusinessLogic.Services
{
    public interface IJwtService
    {
        string GenerateJwtTokenForUser(User user);
    }
}
