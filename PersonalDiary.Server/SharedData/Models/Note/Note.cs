using System;
using SharedData.Models.User;

namespace SharedData.Models
{
    public class Note
    {
        // Id
        public Guid Id { get; set; }

        // General
        public string Description { get; set; }
        public string Text { get; set; }

        // UserProfile
        public Guid UserIdentityFID { get; set; }
        public UserIdentity UserIdentity { get; set; }
    }
}
