using SharedData.Models.User;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLogic.Services.Interfaces
{
    public interface IUserProfileService
    {
        Task<List<UserProfile>> GetAllUserProfiles();

        Task<UserProfile> GetUserProfileByIdAsync(Guid userProfileId);

        Task<bool> CreateUserProfileAsync(UserProfileDTO UserProfileDTO);

        Task<bool> UpdateUserProfileAsync(UserProfile userProfile);

        Task<bool> SubscribeUserAsync(SubscriptionDTO subscriptionDTO);

        Task<bool> UnsubscribeUserAsync(SubscriptionDTO subscriptionDTO);
    }
}
