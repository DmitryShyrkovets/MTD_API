using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Models.Note;
using Services.Models.Note.Requests;
using Services.Models.User;
using Services.ServiceInterfaces;

namespace MTD.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class NotesController: ControllerBase
{
    private readonly INoteService _noteService;
    private readonly IUserService _userService;
    
    public NotesController(INoteService noteService,IUserService userService)
    {
        _noteService = noteService;
        _userService = userService;
    }
    
    [HttpGet]
    public async Task<List<NoteModel>> GetNotes()
    {
        UserModel user = await _userService.GetUserByEmail(User.Identity.Name);
        return await _noteService.GetNotes(user.Id);
    }
    
    [HttpGet("{id}")]
    public async Task<NoteModel> GetNote(int id)
    {
        UserModel user = await _userService.GetUserByEmail(User.Identity.Name);
        return await _noteService.GetNote(user.Id, id);
    }
    
    [HttpPost]
    public async Task<IActionResult> AddNote([FromBody] CreateNoteRequest createNoteRequest)
    {
        try
        {
            await _noteService.TryAddNote(createNoteRequest);
            
            return Ok("Note added successfully!");
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
    [HttpPut]
    public async Task<IActionResult> UpdateNote([FromBody] UpdateNoteRequest updateNoteRequest)
    {
        try
        {
            await _noteService.TryUpdateNote(updateNoteRequest);
            return Ok("Note updated successfully!");
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
    [HttpDelete]
    public async Task<IActionResult> DeleteNote([FromBody] DeleteNoteRequest deleteNoteRequest)
    {
        try
        {
            await _noteService.TryDeleteNote(deleteNoteRequest);
            return Ok("Note deleted successfully!");
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}