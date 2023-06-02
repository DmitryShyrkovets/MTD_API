using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Services.Models.User.Requests;
using Services.ServiceInterfaces;
using MailKit.Net.Smtp;
using MimeKit;

namespace MTD.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly IUserService _service;
    private IMemoryCache _cache;
    private IConfiguration _configuration;
    
    public AccountController(IUserService service, IMemoryCache cache, IConfiguration configuration)
    {
        _service = service;
        _cache = cache;
        _configuration = configuration;
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
    
    [HttpPost("RecoveryData")]
    public async Task<IActionResult> RecoveryData([FromBody]RecoveryRequest recoveryRequest)
    {
        try
        {
            var user = await _service.GetUserForRecovery(recoveryRequest.Email);
            
            var emailMessage = new MimeMessage();
            
            emailMessage.From.Add(new MailboxAddress("Администрация сайта", "mytodomtd@gmail.com"));
            emailMessage.To.Add(new MailboxAddress("", recoveryRequest.Email));
            emailMessage.Subject = "Данные авторизации вашего аккаунта";
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = "<h2>Данны вашего аккаунта: </h2>" +
                       "<p>Логин: " + user.Email + "</p>" +
                       "<p>Пароль: " + user.Password + "</p>" +
                       "<h5>В целях безопасности после прочтения удалите письмо</h5>"
            };

            var userName = _configuration.GetValue<string>("Mail:Name");
            var password = _configuration.GetValue<string>("Mail:Password");

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.gmail.com", 465, true);
                await client.AuthenticateAsync(userName, password);
                await client.SendAsync(emailMessage);

                await client.DisconnectAsync(true);
            }

            return Ok("Data sent by email");
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }

    }
    
    [HttpPost("Logout")]
    public async Task<IActionResult> Logout()
    {
        _cache.Remove(User.Identity.Name);
        
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        
        return Ok("You have successfully logged out");
    }
}