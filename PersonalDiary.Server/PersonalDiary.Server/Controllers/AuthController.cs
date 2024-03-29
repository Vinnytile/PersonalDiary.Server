﻿using BusinessLogic.Services;
using BusinessLogic.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using SharedData.Models;
using SharedData.Models.Auth;
using SharedData.Models.User;
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
        private readonly IUserProfileService _userProfileService;
        private readonly NeuralNetworkService _neuralNetworkService;

        public AuthController(IAuthService authService, IUserProfileService userProfileService, NeuralNetworkService neuralNetworkService)
        {
            _authService = authService;
            _userProfileService = userProfileService;
            _neuralNetworkService = neuralNetworkService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserIdentityRegisterDTO userRegisterDTO)
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

        [HttpPost("registerProfile")]
        public async Task<IActionResult> RegisterProfile(UserProfileDTO userProfileDTO)
        {
            if (userProfileDTO == null)
            {
                return BadRequest();
            }

            try
            {
                await _userProfileService.CreateUserProfileAsync(userProfileDTO);
                return Ok(userProfileDTO);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserIdentityLoginDTO userLoginDTO)
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
        public async Task<IActionResult> LoginFace(UserIdentityLoginDTO userLoginDTO)
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
