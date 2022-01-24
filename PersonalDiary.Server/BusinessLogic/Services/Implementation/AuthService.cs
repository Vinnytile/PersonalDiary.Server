using AutoMapper;
using DataAccess.Context;
using SharedData.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public class AuthService : IAuthService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public AuthService(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task RegisterAsync(UserRegisterDTO userRegisterDTO)
        {
            User user = _mapper.Map<User>(userRegisterDTO);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            await Task.CompletedTask;
        }

        public User Login(UserLoginDTO userLoginDTO)
        {
            User user = _context.Users.FirstOrDefault(u => u.Email == userLoginDTO.Email);

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
