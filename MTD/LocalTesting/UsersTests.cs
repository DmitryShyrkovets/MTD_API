using AutoMapper;
using Mapper;
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
    public async Task GetUsers()
    {
        var users = await _service.GetUsers();

        Assert.AreEqual(3, users.Count);
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
        string email = "testemail@mail.ru";
        var modifyUser = new UserModel
        {
            Id = 1, 
            Nickname = "testName",
            Password = "zxcasd"
        };
        
        await _service.TryModifyUser(modifyUser, email);
        
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
        };
        
        await _service.TryChangeEmail(modifyUser, email);
        
        var user = await _service.GetUserByEmail(modifyUser.Email);
        
        Assert.NotNull(user);
        Assert.NotNull(user.Id);
    }
    
}