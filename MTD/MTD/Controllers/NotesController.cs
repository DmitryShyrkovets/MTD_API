using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.ServiceInterfaces;
using Services.DtoModels;

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
    public async Task<List<NoteDto>> GetNotes()
    {
        UserDto user = await _userService.GetUserByEmail(User.Identity.Name);
        return await _noteService.TryGetNotes(user.Id ?? 0);
    }
    
    [HttpPost]
    public async Task<IActionResult> AddNote([FromBody] NoteDto dto)
    {
        try
        {
            await _noteService.TryAddNote(dto);
            
            return Ok("Note added successfully!");
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
    [HttpPut]
    public async Task<IActionResult> UpdateNote([FromBody] NoteDto dto)
    {
        try
        {
            await _noteService.TryUpdateNote(dto);
            return Ok("Note modified successfully!");
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
    [HttpDelete]
    public async Task<IActionResult> DeleteNote([FromBody] NoteDto dto)
    {
        try
        {
            await _noteService.TryDeleteNote(dto);
            return Ok("Note deleted successfully!");
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}