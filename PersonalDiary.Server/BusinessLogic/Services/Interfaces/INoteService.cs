using SharedData.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public interface INoteService
    {
        Task<List<Note>> GetAllNotesAsync();

        Task<Note> GetNoteByIdAsync(Guid noteId);
        Task<Note> GetNoteByIdAsNoTrackingAsync(Guid noteId);

        Task<bool> CreateNoteAsync(NoteDTO noteDTO);

        Task<bool> UpdateNoteAsync(Note note);

        Task<bool> DeleteNoteAsync(Guid noteId);
    }
}
