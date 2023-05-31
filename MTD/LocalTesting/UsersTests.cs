using AutoMapper;
using Mapper;
using RepositoryForTest;
using Services;
using Services.DtoModels;

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
        var newUser = new UserDto
        {
            Id = 4,
            Email = "test@mail.ru",
            Password = "zxcasd"
        };
        
        await _service.TryAddUser(newUser);
        
        var user = await _service.GetUserByEmail(newUser.Email);
        
        Assert.NotNull(user);
        Assert.NotNull(user.Id);
    }
    
    [Test]
    public async Task ChangePassword()
    {
        var modifyUser = new UserDto
        {
            Id = 1,
            Password = "zxcasd",
            Email = "testemail@mail.ru"
        };
        
        await _service.TryUpdateUser(modifyUser,  null,"qweasdzxc1");
        
        var user = await _service.GetUserByEmail(modifyUser.Email);
        
        Assert.NotNull(user);
        Assert.NotNull(user.Id);
    }
    
    [Test]
    public async Task ChangeEmail()
    {
        var modifyUser = new UserDto
        {
            Id = 2,
            Password = "qwerty1",
            Email = "testemail23@mail.ru"
        };
        
        await _service.TryUpdateUser(modifyUser,  "testemail2@mail.ru", null);
        
        var user = await _service.GetUserByEmail(modifyUser.Email);
        
        Assert.NotNull(user);
        Assert.NotNull(user.Id);
    }
    
    [Test]
    public async Task TryChangeEmail()
    {
        string email = "testemail@mail.ru";
        var modifyUser = new UserDto
        {
            Id = 1, 
            Email = "123testemail@mail.ru",
            Password = "qweasdzxc1"
        };
        
        await _service.TryUpdateUser(modifyUser, email, null);
        
        var user = await _service.GetUserByEmail(modifyUser.Email);
        
        Assert.NotNull(user);
        Assert.NotNull(user.Id);
    }
    
}