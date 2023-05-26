using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.ServiceInterfaces;
using Services.ViewModels;

namespace MTD.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _service;
    public UserController(IUserService service)
    {
        _service = service;
    }
    
    
    [HttpGet("GetUser")]
    public async Task<UserModel> GetUser()
    {
        return await _service.GetUserByEmail(User.Identity.Name);
    }
    
    [HttpPost("CreateUser")]
    public async Task<IActionResult> CreateUser([FromBody] UserModel model)
    {
        try
        {
            await _service.TryAddUser(model);
            return Ok("User added successfully");
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
    [HttpPut("ModifyUser")]
    public async Task<IActionResult> ModifyUser([FromBody] UserModel model)
    {
        try
        {
            model.Email = User.Identity.Name;
            await _service.TryModifyUser(model);
            return Ok("User modified successfully");
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
    [HttpDelete("DeleteUser")]
    public async Task<IActionResult> DeleteUser()
    {
        try
        {
            await _service.TryDeleteUser(User.Identity.Name);
            return Ok("User deleted successfully");
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
}