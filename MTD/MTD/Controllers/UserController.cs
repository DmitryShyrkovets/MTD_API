using Microsoft.AspNetCore.Mvc;
using Services.ServiceInterfaces;
using Services.ViewModels;

namespace MTD.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _service;
    public UserController(IUserService service)
    {
        _service = service;
    }
    
    [HttpGet("GetData")]
    public async Task<List<UserCli>> GetData()
    {
        return await _service.GetUsers();
    }
    
}