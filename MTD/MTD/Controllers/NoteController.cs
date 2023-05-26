using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.ServiceInterfaces;
using Services.ViewModels;

namespace MTD.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class NoteController: ControllerBase
{
    private readonly INoteService _noteService;
    private readonly IUserService _userService;
    
    public NoteController(INoteService noteService,IUserService userService)
    {
        _noteService = noteService;
        _userService = userService;
    }
    
    [HttpGet("GetNotes")]
    public async Task<List<NoteModel>> GetNotes()
    {
        UserModel user = await _userService.GetUserByEmail(User.Identity.Name);
        return await _noteService.TryGetNotes(user.Id ?? 0);
    }
    
    [HttpPost("AddNote")]
    public async Task<IActionResult> AddNote([FromBody] NoteModel model)
    {
        try
        {
            await _noteService.TryAddNote(model);
            
            return Ok("Note added successfully!");
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
    [HttpPut("ModifyNote")]
    public async Task<IActionResult> ModifyNote([FromBody] NoteModel model)
    {
        try
        {
            await _noteService.TryModifyNote(model);
            return Ok("Note modified successfully!");
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
    [HttpDelete("DeleteNote")]
    public async Task<IActionResult> DeleteNote([FromBody] NoteModel model)
    {
        try
        {
            await _noteService.TryDeleteNote(model);
            return Ok("Note deleted successfully!");
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}