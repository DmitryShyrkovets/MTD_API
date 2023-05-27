using AutoMapper;
using Mapper;
using Microsoft.Extensions.Caching.Memory;
using RepositoryForTest;
using Services;
using Services.ViewModels;

namespace LocalTesting;

public class UsersTests
{
    private UserService _service;
    private WorkingWithUsers repository;

    [SetUp]
    public void Setup()
    {
        MapperConfiguration mappingConfig = new MapperConfiguration(mc =>
        {
            mc.AddProfile(new MappingUser());
        });

        IMapper mapper = mappingConfig.CreateMapper();

        repository = new WorkingWithUsers();

        _service = new UserService(repository, mapper);
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
        var newUser = new UserModel
        {
            Id = 4,
            Email = "test@mail.ru",
            Nickname = "testName",
            Password = "zxcasd"
        };
        
        await _service.TryAddUser(newUser);
        
        var user = await _service.GetUserByEmail(newUser.Email);
        
        Assert.NotNull(user);
        Assert.NotNull(user.Id);
    }
    
    [Test]
    public async Task ModifyUser()
    {
        var modifyUser = new UserModel
        {
            Id = 1, 
            Nickname = "testName",
            Password = "zxcasd",
            Email = "testemail@mail.ru"
        };
        
        await _service.TryModifyUser(modifyUser, "qweasdzxc1", null);
        
        var user = await _service.GetUserByEmail(modifyUser.Email);
        
        Assert.NotNull(user);
        Assert.NotNull(user.Id);
    }
    
    
    [Test]
    public async Task TryChangeEmail()
    {
        string email = "testemail@mail.ru";
        var modifyUser = new UserModel
        {
            Id = 1, 
            Email = "123testemail@mail.ru",
            Password = "qweasdzxc1"
        };
        
        await _service.TryModifyUser(modifyUser,null, email);
        
        var user = await _service.GetUserByEmail(modifyUser.Email);
        
        Assert.NotNull(user);
        Assert.NotNull(user.Id);
    }
    
}