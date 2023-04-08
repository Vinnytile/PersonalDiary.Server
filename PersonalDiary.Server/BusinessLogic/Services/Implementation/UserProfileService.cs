using AutoMapper;
using BusinessLogic.Services.Interfaces;
using DataAccess.Context;
using Microsoft.EntityFrameworkCore;
using SharedData.Models.User;
using System;
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

    }
}
