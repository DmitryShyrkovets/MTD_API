using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.ServiceInterfaces;
using Services.DtoModels;

namespace MTD.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ProfileController : ControllerBase
{
    private readonly IUserService _service;
    public ProfileController(IUserService service)
    {
        _service = service;
    }
    
    
    [HttpGet]
    public async Task<UserDto> GetUser()
    {
        return await _service.GetUserByEmail(User.Identity.Name);
    }
    
    [HttpPut("ChangeEmail")]
    public async Task<IActionResult> ChangeEmail([FromBody]UserDto dto)
    {
        try
        {
            await _service.TryUpdateUser(dto, User.Identity.Name, null);

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            
            await Authenticate(dto.Email);
            
            return Ok("Email changed successfully!");
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

    [HttpPut("ChangePassword")]
    public async Task<IActionResult> ChangePassword([FromBody] UserDto dto, string oldPassword)
    {
        try
        {
            dto.Email = User.Identity.Name;
            await _service.TryUpdateUser(dto, null, oldPassword);
            return Ok("User changed successfully!");
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
    [HttpDelete]
    public async Task<IActionResult> DeleteUser()
    {
        try
        {
            await _service.TryDeleteUser(User.Identity.Name);
            return Ok("User deleted successfully!");
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
}