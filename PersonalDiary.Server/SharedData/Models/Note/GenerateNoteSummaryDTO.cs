using System;

namespace SharedData.Models
{
    public class GenerateNoteSummaryDTO
    {
        public Guid UserIdentityFID { get; set; }
        public Guid NoteId{ get; set; }
    }
}
