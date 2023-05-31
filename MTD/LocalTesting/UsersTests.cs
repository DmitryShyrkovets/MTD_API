using AutoMapper;
using Mapper;
using RepositoryForTest;
using Services;
using Services.Models.User.Requests;

namespace LocalTesting;

public class UsersTests
{
    private UserService _service;
    private UserRepository repository;

    [SetUp]
    public void Setup()
    {
        MapperConfiguration mappingConfig = new MapperConfiguration(mc =>
        {
            mc.AddProfile(new MappingUser());
        });

        IMapper mapper = mappingConfig.CreateMapper();

        repository = new UserRepository();

        _service = new UserService(repository, mapper, null);
    }
    
    
    [Test]
    public async Task GetUser()
    {
        string email = "testemail@mail.ru";
        
        var user = await _service.GetUserByEmail(email);
        
        Assert.NotNull(user);
        Assert.NotNull(user.Id);
    }
    
    [Test]
    public async Task CreateUser()
    {
        var authUserRequest = new AuthUserRequest
        {
            Email = "test@mail.ru",
            Password = "zxcasd"
        };
        
        await _service.TryAddUser(authUserRequest);
        
        var user = await _service.GetUserByEmail(authUserRequest.Email);
        
        Assert.NotNull(user);
        Assert.NotNull(user.Id);
    }
    
    [Test]
    public async Task UpdatePassword()
    {
        var updatePasswordRequest = new UpdatePasswordRequest
        {
            Id = 1,
            Password = "zxcasd",
            OldPassword = "qweasdzxc1"
        };

        var email = "testemail@mail.ru";
        
        await _service.TryUpdatePassword(updatePasswordRequest,  email);
        
        var user = await _service.GetUserByEmail(email);
        
        Assert.NotNull(user);
        
        var authUserRequest = new AuthUserRequest
        {
            Email = "testemail@mail.ru",
            Password = "zxcasd"
        };
        
        Assert.AreEqual(true, await _service.UserVerification(authUserRequest));
    }
    
    [Test]
    public async Task UpdateEmail()
    {
        var updateEmailRequest = new UpdateEmailRequest
        {
            Id = 2,
            Password = "qwerty1",
            Email = "testemail23@mail.ru"
        };
        
        var oldEmail = "testemail2@mail.ru";
        
        await _service.TryUpdateEmail(updateEmailRequest,  oldEmail);
        
        var user = await _service.GetUserByEmail(updateEmailRequest.Email);
        
        Assert.NotNull(user);
        
        var authUserRequest = new AuthUserRequest
        {
            Email = "testemail23@mail.ru",
            Password = "qwerty1"
        };
        
        Assert.AreEqual(true, await _service.UserVerification(authUserRequest));
    }
    
    
}