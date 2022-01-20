using SharedData.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public interface INoteService
    {
        IEnumerable<Note> GetAllNotes();

        Note GetNoteById(Guid noteId);

        Task SetNote(Note note);

        Task UpdateNote(Note note);

        Task DeleteNote(Guid noteId);
    }
}
