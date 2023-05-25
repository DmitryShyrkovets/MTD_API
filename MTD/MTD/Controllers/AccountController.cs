using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Services.ServiceInterfaces;
using Services.ViewModels;

namespace MTD.Controllers;

[ApiController]
[Route("[controller]")]
public class AccountController : ControllerBase
{
    private readonly IUserService _service;
    
    public AccountController(IUserService service)
    {
        _service = service;
    }
    
    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody]UserModel model)
    {
        if (await _service.UserVerification(model))
        {
            await Authenticate(model.Email);
            return Ok("Signed in successfully");
        }
        
        return Unauthorized("Wrong data");
    }
    
    [HttpPut("ChangeEmail")]
    public async Task<IActionResult> ChangeEmail([FromBody]UserModel model)
    {
        try
        {
            await _service.TryChangeEmail(model, User.Identity.Name);
            
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
    
    [HttpGet("Logout")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return Ok("You have successfully logged out");
    }
}