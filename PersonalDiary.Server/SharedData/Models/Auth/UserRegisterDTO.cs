using System.ComponentModel.DataAnnotations;

namespace SharedData.Models
{
    public class UserRegisterDTO
    {
        [EmailAddress]
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
