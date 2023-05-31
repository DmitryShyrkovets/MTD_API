using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Services.Models.User.Requests;
using Services.ServiceInterfaces;

namespace MTD.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly IUserService _service;
    private IMemoryCache _cache;
    
    public AccountController(IUserService service, IMemoryCache cache)
    {
        _service = service;
        _cache = cache;
    }
    
    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody]AuthUserRequest authUserRequest)
    {
        try
        {
            if (!await _service.UserVerification(authUserRequest))
                throw new Exception("Wrong data!");
                
            await Authenticate(authUserRequest.Email);

            var user = await _service.GetUserByEmail(authUserRequest.Email);
            _cache.Set(user.Email, user, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
            
            return Ok("Signed in successfully");

        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }

    }
    
    [HttpPost("Registration")]
    public async Task<IActionResult> Registration([FromBody]AuthUserRequest authUserRequest)
    {
        try
        {
            await _service.TryAddUser(authUserRequest);
            
            await Authenticate(authUserRequest.Email);

            var user = await _service.GetUserByEmail(authUserRequest.Email);
            _cache.Set(user.Email, user, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
            
            return Ok("Registration is successfully");
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
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
        _cache.Remove(User.Identity.Name);
        
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        
        return Ok("You have successfully logged out");
    }
}