using System;

namespace SharedData.Models
{
    public class AuthSuccessResponse
    {
        public Guid UserId { get; set; }
        public string Token { get; set; }
    }
}
