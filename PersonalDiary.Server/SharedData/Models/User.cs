using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace SharedData.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        [Required]
        public string Email { get; set; }
        public string Password { get; set; }

        public int FaceId { get; set; }

        public List<Note> Notes { get; set; }
    }
}
