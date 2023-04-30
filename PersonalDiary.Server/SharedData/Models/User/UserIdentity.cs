using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SharedData.Models.User
{
    public class UserIdentity
    {
        // Id
        public Guid Id { get; set; }

        // General
        [Required]
        public string Email { get; set; }
        public string Password { get; set; }

        // FaceId
        public int FaceId { get; set; }

        // UserProfile
        public UserProfile? UserProfile { get; set; } = null!;

        // Notes
        public List<Note> Notes { get; set; }

        // Subscription
        public List<Subscription> Subscribers { get; set; }
        public List<Subscription> Observables { get; set; }
    }
}
