using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedData.Models
{
    public class UserRegisterDTO
    {
        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
