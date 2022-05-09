using System;
using System.Collections.Generic;

namespace SharedData.Models
{
    public class AuthenticationResult
    {
        public Guid UserId { get; set; }
        public string Token { get; set; }
        public bool Success { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }
}
