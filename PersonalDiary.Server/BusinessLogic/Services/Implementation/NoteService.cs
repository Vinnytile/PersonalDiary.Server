using AutoMapper;
using DataAccess.Context;
using Microsoft.EntityFrameworkCore;
using SharedData.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public class NoteService : INoteService
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public NoteService(DataContext dataContext, IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }

        public async Task<List<Note>> GetAllNotesAsync() =>
            await _dataContext.Notes.ToListAsync();

        public async Task<Note> GetNoteByIdAsync(Guid noteId) =>
            await _dataContext.Notes.FirstOrDefaultAsync(n => n.Id == noteId);

        public async Task<bool> CreateNoteAsync(NoteDTO noteDTO)
        {
            Note note = _mapper.Map<Note>(noteDTO);

            await _dataContext.Notes.AddAsync(note);
            var created = await _dataContext.SaveChangesAsync();

            return created > 0;
        }

        public async Task<bool> UpdateNoteAsync(Note note)
        {
            _dataContext.Notes.Update(note);
            var updated = await _dataContext.SaveChangesAsync();

            return updated > 0;
        }

        public async Task<bool> DeleteNoteAsync(Guid noteId)
        {
            Note note = await _dataContext.Notes.FirstOrDefaultAsync(n => n.Id == noteId);

            if (note == null)
                return false;

            _dataContext.Notes.Remove(note);
            var deleted = await _dataContext.SaveChangesAsync();

            return deleted > 0;
        }
    }
}
