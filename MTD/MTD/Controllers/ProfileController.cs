using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Models.User;
using Services.Models.User.Requests;
using Services.ServiceInterfaces;

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
    public async Task<UserModel> GetUser()
    {
        return await _service.GetUserByEmail(User.Identity.Name);
    }
    
    [HttpPut("UpdateEmail")]
    public async Task<IActionResult> UpdateEmail([FromBody]UpdateEmailRequest updateEmailRequest)
    {
        try
        {
            await _service.TryUpdateEmail(updateEmailRequest, User.Identity.Name);

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            
            await Authenticate(updateEmailRequest.Email);
            
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

    [HttpPut("UpdatePassword")]
    public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordRequest updatePasswordRequest, string oldPassword)
    {
        try
        {
            await _service.TryUpdatePassword(updatePasswordRequest, User.Identity.Name);
            return Ok("Password changed successfully!");
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