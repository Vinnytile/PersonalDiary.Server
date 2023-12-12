using System;
using SharedData.Models.User;

namespace SharedData.Models
{
    public class Note
    {
        // Id
        public Guid Id { get; set; }

        // Time settings
        public DateTime CreatedAt { get; set; }
        public DateTime ChangedAt { get; set; }

        // General
        public string Description { get; set; }
        public string Text { get; set; }

        // UserIdentity
        public Guid UserIdentityFID { get; set; }
        public UserIdentity UserIdentity { get; set; }

        // Summary
        public string Summary { get; set; }

        // Sentiments
        public int? Sentiments { get; set; }

        // Named Entities
        public string NamedEntities { get; set; }
    }
}
