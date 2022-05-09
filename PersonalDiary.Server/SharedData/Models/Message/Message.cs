using System;

namespace SharedData.Models
{
    public class Message
    {
        public Guid UserId { get; set; }
        public MessageTypes Type { get; set; }
        public string Content { get; set; }
        public bool ShouldSave { get; set; }

        public bool Succeed { get; set; }
    }
}
