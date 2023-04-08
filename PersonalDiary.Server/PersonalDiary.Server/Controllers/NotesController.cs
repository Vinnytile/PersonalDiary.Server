using BusinessLogic.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedData.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PersonalDiary.Server.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private readonly INoteService _noteService;

        public NotesController(INoteService noteService)
        {
            _noteService = noteService;
        }

        [HttpGet("{userIdentityId}")]
        public async Task<IActionResult> GetNotes(Guid userIdentityId)
        {
            List<Note> notes = await _noteService.GetAllNotesByUserIdentityIdAsync(userIdentityId);
            
            return Ok(notes);
        }

        // GET api/notes/5
        [HttpGet("note/{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var note = await _noteService.GetNoteByIdAsync(id);

            if (note is null)
                return NotFound();

            return Ok(note);
        }

        // POST api/notes
        [HttpPost]
        public async Task<IActionResult> Post(NoteDTO noteDTO)
        {
            if (noteDTO == null)
            {
                return BadRequest();
            }

            try
            {
                await _noteService.CreateNoteAsync(noteDTO);
                return Ok(noteDTO);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // PUT api/notes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, Note note)
        {
            if (note == null)
                return BadRequest();

            note.Id = id;

            var existingNote = await _noteService.GetNoteByIdAsNoTrackingAsync(note.Id);
            
            if (existingNote == null)
                return NotFound();

            var updated = await _noteService.UpdateNoteAsync(note);
            
            if (updated)
                return Ok(updated);

            return BadRequest();
        }

        // DELETE api/notes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await _noteService.DeleteNoteAsync(id);

            if (deleted)
                return NoContent();

            return NotFound();
        }
    }
}
