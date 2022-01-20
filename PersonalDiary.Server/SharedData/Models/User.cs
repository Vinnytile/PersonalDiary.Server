using System;
using System.ComponentModel.DataAnnotations;


namespace SharedData.Models
{
    public class User
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        public string Password { get; set; }
    }
}
