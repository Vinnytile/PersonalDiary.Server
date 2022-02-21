using AutoMapper;
using DataAccess.Context;
using Microsoft.EntityFrameworkCore;
using SharedData.Models;
using System.Threading.Tasks;

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
            var createdUser = await _dataContext.Users.AddAsync(newUser);
            await _dataContext.SaveChangesAsync();

            if (createdUser == null)
            {
                return new AuthenticationResult
                {
                    Errors = new[] { "Could not add user to db" }
                };
            }

            return _jwtService.GenerateAuthenticationResultForUser(newUser);
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

            bool isValidPassword = user.Password == userLoginDTO.Password;

            if (!isValidPassword)
            {
                return new AuthenticationResult
                {
                    Errors = new[] { "User/password combination is wrong" }
                };
            }

            return _jwtService.GenerateAuthenticationResultForUser(user);
        }
    }
}
