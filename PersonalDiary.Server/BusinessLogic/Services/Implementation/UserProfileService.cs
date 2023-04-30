using AutoMapper;
using BusinessLogic.Services.Interfaces;
using DataAccess.Context;
using Microsoft.EntityFrameworkCore;
using SharedData.Models;
using SharedData.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace BusinessLogic.Services.Implementation
{
    public class UserProfileService : IUserProfileService
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public UserProfileService(DataContext dataContext, IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }

        public async Task<List<UserProfile>> GetAllUserProfiles() =>
            await _dataContext.UserProfiles.ToListAsync();

        public async Task<UserProfile> GetUserProfileByIdAsync(Guid userProfileId) =>
            await _dataContext.UserProfiles.FirstOrDefaultAsync(u => u.Id == userProfileId);

        public async Task<bool> CreateUserProfileAsync(UserProfileDTO UserProfileDTO)
        {
            UserProfile userProfile = _mapper.Map<UserProfile>(UserProfileDTO);

            await _dataContext.UserProfiles.AddAsync(userProfile);
            var created = await _dataContext.SaveChangesAsync();

            return created > 0;
        }

        public async Task<bool> UpdateUserProfileAsync(UserProfile userProfile)
        {
            _dataContext.UserProfiles.Update(userProfile);
            var updated = await _dataContext.SaveChangesAsync();

            return updated > 0;
        }

        public async Task<bool> SubscribeUserAsync(SubscriptionDTO subscriptionDTO)
        {
            Subscription subscription = _mapper.Map<Subscription>(subscriptionDTO);

            var observableIdentitityFID = _dataContext.UserProfiles.FirstOrDefault(up => up.Id == subscriptionDTO.ObservableFID).UserIdentityFID;
            subscription.ObservableFID = observableIdentitityFID;

            await _dataContext.Subscriptions.AddAsync(subscription);
            var subscribed = await _dataContext.SaveChangesAsync();

            return subscribed > 0;
        }

        public async Task<bool> UnsubscribeUserAsync(SubscriptionDTO subscriptionDTO)
        {
            Subscription subscription = _mapper.Map<Subscription>(subscriptionDTO);

            var observableIdentitityFID = _dataContext.UserProfiles.FirstOrDefault(up => up.Id == subscriptionDTO.ObservableFID).UserIdentityFID;
            subscription.ObservableFID = observableIdentitityFID;

            var deleteSubscription = _dataContext.Subscriptions.FirstOrDefault(s => s.SubscriberFID == subscription.SubscriberFID && s.ObservableFID == subscription.ObservableFID);

            var subscriptionList = _dataContext.Subscriptions.Remove(deleteSubscription);
            var unsubscribed = await _dataContext.SaveChangesAsync();

            return unsubscribed > 0;
        }
    }
}
