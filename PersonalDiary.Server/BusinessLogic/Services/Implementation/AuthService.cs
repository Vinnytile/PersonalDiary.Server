using AutoMapper;
using DataAccess.Context;
using Microsoft.EntityFrameworkCore;
using SharedData.Models;
using SharedData.Models.User;
using System;
using System.Linq;
using System.Threading.Tasks;
using BCryptNet = BCrypt.Net.BCrypt;

namespace BusinessLogic.Services
{
    public class AuthService : IAuthService
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;
        private readonly IJwtService _jwtService;

        public AuthService(DataContext context, IMapper mapper, IJwtService jwtService)
        {
            _dataContext = context;
            _mapper = mapper;
            _jwtService = jwtService;
        }

        public async Task<AuthenticationResult> RegisterAsync(UserIdentityRegisterDTO useridentityRegisterDTO)
        {
            UserIdentity existingUser = await _dataContext.UserIdentities.FirstOrDefaultAsync(u => u.Email == useridentityRegisterDTO.Email);

            if (existingUser != null)
            {
                return new AuthenticationResult
                {
                    Errors = new[] { "User with this email already exists" }
                };
            }

            UserIdentity newUserIdentity = _mapper.Map<UserIdentity>(useridentityRegisterDTO);
            newUserIdentity.Password = BCryptNet.HashPassword(newUserIdentity.Password);

            var createdUser = await _dataContext.UserIdentities.AddAsync(newUserIdentity);
            await _dataContext.SaveChangesAsync();

            if (createdUser == null)
            {
                return new AuthenticationResult
                {
                    Errors = new[] { "Could not add user to db" }
                };
            }

            return new AuthenticationResult { Success = true, UserId = newUserIdentity.Id };
        }

        public async Task<bool> RegisterFaceAsync(Guid userId)
        {
            var users = await _dataContext.UserIdentities.ToListAsync();
            var maxFaceId = users.Select(x => x.FaceId).Max();

            var user = await _dataContext.UserIdentities.FirstOrDefaultAsync(u => u.Id == userId);
            user.FaceId = maxFaceId + 1;

            _dataContext.UserIdentities.Update(user);
            var updated = await _dataContext.SaveChangesAsync();

            return updated > 0;
        }

        public async Task<AuthenticationResult> LoginAsync(UserIdentityLoginDTO userIdentityLoginDTO)
        {
            UserIdentity user = await _dataContext.UserIdentities.FirstOrDefaultAsync(u => u.Email == userIdentityLoginDTO.Email);

            if (user == null)
            {
                return new AuthenticationResult
                {
                    Errors = new[] { "User does not exist" }
                };
            }

            bool isValidPassword = BCryptNet.Verify(userIdentityLoginDTO.Password, user.Password);

            if (!isValidPassword)
            {
                return new AuthenticationResult
                {
                    Errors = new[] { "User/password combination is wrong" }
                };
            }

            string jwtToken = _jwtService.GenerateJwtTokenForUser(user);

            return new AuthenticationResult 
            { 
                UserId = user.Id,
                Token = jwtToken,
                Success = true 
            };
        }

        public async Task<AuthenticationResult> LoginFaceAsync(UserIdentityLoginDTO userIdentityLoginDTO)
        {
            UserIdentity user = await _dataContext.UserIdentities.FirstOrDefaultAsync(u => u.Email == userIdentityLoginDTO.Email);

            if (user == null)
            {
                return new AuthenticationResult
                {
                    Errors = new[] { "User does not exist" }
                };
            }

            return new AuthenticationResult
            {
                UserId = user.Id,
                Success = true
            };
        }

        public async Task<string> GetJwtTokenAsync(Guid userId)
        {
            UserIdentity user = await _dataContext.UserIdentities.FirstOrDefaultAsync(u => u.Id == userId);
            string jwtToken = _jwtService.GenerateJwtTokenForUser(user);

            return jwtToken;
        }
    }
}
