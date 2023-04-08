using BusinessLogic.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using SharedData.Models.User;
using System.Threading.Tasks;
using System;

namespace PersonalDiary.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserProfileController : ControllerBase
    {
        private readonly IUserProfileService _userProfileService;

        public UserProfileController(IUserProfileService userProfileService)
        {
            _userProfileService = userProfileService;
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
    }
}
