﻿using AutoMapper;
using DataAccess.Context;
using Microsoft.EntityFrameworkCore;
using SharedData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<List<Note>> GetAllNotesByUserIdentityIdAsync(Guid userIdentityId) =>
            await _dataContext.Notes.Where(x => x.UserIdentityFID == userIdentityId).ToListAsync();

        public async Task<List<Note>> GetObservedNotesByUserIdentityIdAsync(Guid userIdentityId)
        {
            var observedUserIdentititesId = await _dataContext.Subscriptions.Where(s => s.SubscriberFID == userIdentityId).Select(s => s.ObservableFID).ToListAsync();

            var observedNotes = await _dataContext.Notes.Where(n => observedUserIdentititesId.Contains(n.UserIdentityFID)).ToListAsync();

            return observedNotes;
        }

        public async Task<Note> GetNoteByIdAsync(Guid noteId) =>
            await _dataContext.Notes.FirstOrDefaultAsync(n => n.Id == noteId);

        public async Task<Note> GetNoteByIdAsNoTrackingAsync(Guid noteId) =>
            await _dataContext.Notes.AsNoTracking().FirstOrDefaultAsync(n => n.Id == noteId);

        public async Task<bool> CreateNoteAsync(NoteDTO noteDTO)
        {
            Note note = _mapper.Map<Note>(noteDTO);
            note.CreatedAt = DateTime.Now;

            await _dataContext.Notes.AddAsync(note);
            var created = await _dataContext.SaveChangesAsync();

            return created > 0;
        }

        public async Task<bool> UpdateNoteAsync(Note note)
        {
            note.ChangedAt = DateTime.Now;
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
