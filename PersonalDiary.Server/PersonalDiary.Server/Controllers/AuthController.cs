using BusinessLogic.Services;
using Microsoft.AspNetCore.Mvc;
using SharedData.Models;
using System;
using System.Threading.Tasks;

namespace PersonalDiary.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterDTO userRegisterDTO)
        {
            try
            {
                await _authService.RegisterAsync(userRegisterDTO);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("login")]
        public IActionResult Login(UserLoginDTO userLoginDTO)
        {
            try
            {
                var user = _authService.Login(userLoginDTO);
                return Ok(user);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
