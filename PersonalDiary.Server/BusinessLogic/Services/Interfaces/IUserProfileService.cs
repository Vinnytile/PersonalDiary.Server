using SharedData.Models.User;
using System;
using System.Threading.Tasks;

namespace BusinessLogic.Services.Interfaces
{
    public interface IUserProfileService
    {
        Task<UserProfile> GetUserProfileByIdAsync(Guid userProfileId);

        Task<bool> CreateUserProfileAsync(UserProfileDTO UserProfileDTO);

        Task<bool> UpdateUserProfileAsync(UserProfile userProfile);
    }
}
