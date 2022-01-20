using DataAccess.Context;
using SharedData.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationContext _context;

        public AuthService(ApplicationContext context)
        {
            _context = context;
        }

        public async Task RegisterAsync(UserRegisterDTO userRegisterDTO)
        {
            var user = new User
            {
                Name = userRegisterDTO.Name,
                Email = userRegisterDTO.Email,
                Password = userRegisterDTO.Password
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            await Task.CompletedTask;
        }

        public User Login(UserLoginDTO userLoginDTO)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == userLoginDTO.Email);

            if (user is null)
            {
                throw new Exception("Email is incorrect");
            }

            if (userLoginDTO.Password != user.Password)
            {
                throw new Exception("Password is incorrect");
            }

            return user;
        }
    }
}
