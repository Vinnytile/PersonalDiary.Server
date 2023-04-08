﻿using System;
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
        public int Age { get; set; }
        public DateTime DateOfBirth { get; set; }

        // UserIdentity
        public Guid UserIdentityFID { get; set; }
        public UserIdentity UserIdentity { get; set; }
    }
}