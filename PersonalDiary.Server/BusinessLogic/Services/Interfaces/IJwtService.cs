using SharedData.Models.User;

namespace BusinessLogic.Services
{
    public interface IJwtService
    {
        string GenerateJwtTokenForUser(UserIdentity user);
    }
}
