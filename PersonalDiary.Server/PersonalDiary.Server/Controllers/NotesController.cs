using BusinessLogic.Services;
using Microsoft.AspNetCore.Mvc;
using SharedData.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PersonalDiary.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private readonly INoteService _noteService;

        public NotesController(INoteService noteService)
        {
            _noteService = noteService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            List<Note> notes = await _noteService.GetAllNotesAsync();
            
            return Ok(notes);
        }

        // GET api/notes/5
        [HttpGet("{id}")]
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

        // PUT api/notes/
        [HttpPut]
        public async Task<IActionResult> Put(Note note)
        {
            if (note == null)
                return BadRequest();

            var existingNote = await _noteService.GetNoteByIdAsync(note.Id);
            
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
