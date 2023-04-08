using System.ComponentModel.DataAnnotations;

namespace SharedData.Models.User
{
    public class UserIdentityRegisterDTO
    {
        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
