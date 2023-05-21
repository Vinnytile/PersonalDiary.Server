using System;
using System.Collections.Generic;

namespace SharedData.Models.User
{
    public class UserProfile
    {
        // Id
        public Guid Id { get; set; }

        // General
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }

        // Time settings
        public DateTime CreatedAt { get; set; }
        public DateTime ChangedAt { get; set; }

        // UserIdentity
        public Guid UserIdentityFID { get; set; }
        public UserIdentity UserIdentity { get; set; }
    }
}
