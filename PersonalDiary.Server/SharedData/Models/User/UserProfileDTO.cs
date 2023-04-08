using System;

namespace SharedData.Models.User
{
    public class UserProfileDTO
    {
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public DateTime DateOfBirth { get; set; }

        public Guid UserIdentityFID { get; set; }
    }
}
