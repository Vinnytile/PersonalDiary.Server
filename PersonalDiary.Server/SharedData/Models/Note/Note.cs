using System;

namespace SharedData.Models
{
    public class Note
    {
        public Guid Id { get; set; }
        public string Desciption { get; set; }
        public string Text { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}
