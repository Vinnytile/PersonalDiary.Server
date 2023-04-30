using BusinessLogic.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using SharedData.Models.User;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using SharedData.Models;
using System;

namespace PersonalDiary.Server.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class UserProfileController : ControllerBase
    {
        private readonly IUserProfileService _userProfileService;

        public UserProfileController(IUserProfileService userProfileService)
        {
            _userProfileService = userProfileService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUserProfiles()
        {
            List<UserProfile> notes = await _userProfileService.GetAllUserProfiles();

            return Ok(notes);
        }

        [HttpPost("subscribeUser")]
        public async Task<IActionResult> SubscribeUser(SubscriptionDTO subscriptionDTO)
        {
            if (subscriptionDTO == null)
            {
                return BadRequest();
            }

            try
            {
                await _userProfileService.SubscribeUserAsync(subscriptionDTO);
                return Ok(subscriptionDTO);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("unsubscribeUser")]
        public async Task<IActionResult> UnsubscribeUser(SubscriptionDTO subscriptionDTO)
        {
            if (subscriptionDTO == null)
            {
                return BadRequest();
            }

            try
            {
                await _userProfileService.UnsubscribeUserAsync(subscriptionDTO);
                return Ok(subscriptionDTO);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
