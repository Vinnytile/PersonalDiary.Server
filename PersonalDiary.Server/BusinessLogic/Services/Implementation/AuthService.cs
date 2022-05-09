using AutoMapper;
using DataAccess.Context;
using Microsoft.EntityFrameworkCore;
using SharedData.Models;
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

        public async Task<AuthenticationResult> RegisterAsync(UserRegisterDTO userRegisterDTO)
        {
            User existingUser = await _dataContext.Users.FirstOrDefaultAsync(u => u.Email == userRegisterDTO.Email);

            if (existingUser != null)
            {
                return new AuthenticationResult
                {
                    Errors = new[] { "User with this email already exists" }
                };
            }

            User newUser = _mapper.Map<User>(userRegisterDTO);
            newUser.Password = BCryptNet.HashPassword(newUser.Password);
            var createdUser = await _dataContext.Users.AddAsync(newUser);
            await _dataContext.SaveChangesAsync();

            if (createdUser == null)
            {
                return new AuthenticationResult
                {
                    Errors = new[] { "Could not add user to db" }
                };
            }

            return new AuthenticationResult { Success = true, UserId = newUser.Id };
        }

        public async Task<bool> RegisterFaceAsync(Guid userId)
        {
            var users = await _dataContext.Users.ToListAsync();
            var maxFaceId = users.Select(x => x.FaceId).Max();

            var user = await _dataContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
            user.FaceId = maxFaceId + 1;

            _dataContext.Users.Update(user);
            var updated = await _dataContext.SaveChangesAsync();

            return updated > 0;
        }

        public async Task<AuthenticationResult> LoginAsync(UserLoginDTO userLoginDTO)
        {
            User user = await _dataContext.Users.FirstOrDefaultAsync(u => u.Email == userLoginDTO.Email);

            if (user == null)
            {
                return new AuthenticationResult
                {
                    Errors = new[] { "User does not exist" }
                };
            }

            bool isValidPassword = BCryptNet.Verify(userLoginDTO.Password, user.Password);

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

        public async Task<AuthenticationResult> LoginFaceAsync(UserLoginDTO userLoginDTO)
        {
            User user = await _dataContext.Users.FirstOrDefaultAsync(u => u.Email == userLoginDTO.Email);

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
            User user = await _dataContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
            string jwtToken = _jwtService.GenerateJwtTokenForUser(user);

            return jwtToken;
        }
    }
}
