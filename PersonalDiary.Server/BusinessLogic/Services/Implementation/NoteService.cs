using DataAccess.Context;
using SharedData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public class NoteService : INoteService
    {
        private readonly ApplicationContext _context;

        public NoteService(ApplicationContext context)
        {
            _context = context;
            if (!_context.Notes.Any())
            {
                _context.Notes.Add(new Note { Desciption = "Desc1", Text = "Text1" });
                _context.Notes.Add(new Note { Desciption = "Desc2", Text = "Text2" });
                _context.SaveChangesAsync();
            }
        }

        public IEnumerable<Note> GetAllNotes() =>
            _context.Notes.ToList();

        public Note GetNoteById(Guid noteId) =>
            _context.Notes.FirstOrDefault(x => x.Id == noteId);

        public async Task SetNote(Note note)
        {
            _context.Notes.Add(note);
            await _context.SaveChangesAsync();

            await Task.CompletedTask;
        }

        public async Task UpdateNote(Note note)
        {
            _context.Update(note);

            await _context.SaveChangesAsync();

            await Task.CompletedTask;
        }

        public async Task DeleteNote(Guid noteId)
        {
            var note = _context.Notes.FirstOrDefault(x => x.Id == noteId);

            _context.Notes.Remove(note);

            await _context.SaveChangesAsync();

            await Task.CompletedTask;
        }
    }
}
