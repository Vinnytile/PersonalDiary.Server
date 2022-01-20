using BusinessLogic.Services;
using Microsoft.AspNetCore.Mvc;
using SharedData.Models;
using System;
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
        public IActionResult Get()
        {
            var notes = _noteService.GetAllNotes();
            return Ok(notes);
        }

        // GET api/notes/5
        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            var note = _noteService.GetNoteById(id);

            if (note == null)
                return NotFound();
            return Ok(note);
        }

        // POST api/notes
        [HttpPost]
        public async Task<IActionResult> Post(Note note)
        {
            if (note == null)
            {
                return BadRequest();
            }

            try
            {
                await _noteService.SetNote(note);
                return Ok(note);
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
            {
                return BadRequest();
            }

            try
            {
                await _noteService.UpdateNote(note);
                return Ok(note);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // DELETE api/notes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _noteService.DeleteNote(id);
                return Ok(id);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
