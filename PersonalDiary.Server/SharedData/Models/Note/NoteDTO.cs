using System;

namespace SharedData.Models
{
    public class NoteDTO
    {
        public string Description { get; set; }
        public string Text { get; set; }
        public string Summary { get; set; }
        public SentimentsTypes? Sentiments { get; set; }
        public string NamedEntities { get; set; }

        public Guid UserIdentityFID { get; set; }
    }
}
