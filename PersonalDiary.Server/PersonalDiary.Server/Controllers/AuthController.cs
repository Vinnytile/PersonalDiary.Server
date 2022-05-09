using BusinessLogic.Services;
using Microsoft.AspNetCore.Mvc;
using SharedData.Models;
using SharedData.Models.Auth;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalDiary.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly NeuralNetworkService _neuralNetworkService;

        public AuthController(IAuthService authService, NeuralNetworkService neuralNetworkService)
        {
            _authService = authService;
            _neuralNetworkService = neuralNetworkService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterDTO userRegisterDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new AuthFailedResponse
                {
                    Errors = ModelState.Values.SelectMany(x => x.Errors.Select(xx => xx.ErrorMessage))
                });
            }

            var authResponse = await _authService.RegisterAsync(userRegisterDTO);

            if (!authResponse.Success)
            {
                return BadRequest(new AuthFailedResponse
                {
                    Errors = authResponse.Errors
                });
            }

            return Ok(new AuthSuccessResponse
            {
                UserId = authResponse.UserId
            });
        }

        [HttpGet("registerFace/{userId}")]
        public async Task<IActionResult> RegisterFace(Guid userId)
        {
            var updated = await _authService.RegisterFaceAsync(userId);

            if (updated)
            {
                await _neuralNetworkService.TrainFaceAsync(userId);
                return Ok(updated);
            }

            return BadRequest();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDTO userLoginDTO)
        {
            var authResult = await _authService.LoginAsync(userLoginDTO);

            if (!authResult.Success)
            {
                return BadRequest(new AuthFailedResponse
                {
                    Errors = authResult.Errors
                });
            }

            return Ok(new AuthSuccessResponse
            {
                UserId = authResult.UserId,
                Token = authResult.Token
            });
        }

        [HttpPost("loginFace")]
        public async Task<IActionResult> LoginFace(UserLoginDTO userLoginDTO)
        {
            var authResult = await _authService.LoginFaceAsync(userLoginDTO);

            if (!authResult.Success)
            {
                return BadRequest(new AuthFailedResponse
                {
                    Errors = authResult.Errors
                });
            }

            await _neuralNetworkService.LoadUsersAsync();

            return Ok(new AuthSuccessResponse
            {
                UserId = authResult.UserId
            });
        }

        [HttpGet("jwtToken/{userId}")]
        public async Task<IActionResult> GenerateJwtToken(Guid userId)
        {
            var jwtToken = await _authService.GetJwtTokenAsync(userId);

            if (jwtToken == null)
            {
                return BadRequest();
            }

            _neuralNetworkService.ClearRegisterData(userId.ToString());
            _neuralNetworkService.ClearLoginData(userId.ToString());

            return Ok(jwtToken);
        }
    }
}
