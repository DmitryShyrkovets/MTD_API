using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
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
    
    [HttpPut("ChangeEmail")]
    public async Task<IActionResult> ChangeEmail([FromBody]UserModel model)
    {
        try
        {
            await _service.TryModifyUser(model, null, User.Identity.Name);

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            
            await Authenticate(model.Email);
            
            return Ok("Successfully");
        }
        catch (Exception e)
        {
            return Ok(e.Message);
        }
    }
    
    [NonAction]
    private async Task Authenticate(string userEmail)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimsIdentity.DefaultNameClaimType, userEmail)
        };

        ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
    }

    [HttpPut("ModifyUser")]
    public async Task<IActionResult> ModifyUser([FromBody] UserModel model, string? oldPassword)
    {
        try
        {
            model.Email = User.Identity.Name;
            await _service.TryModifyUser(model, oldPassword, null);
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