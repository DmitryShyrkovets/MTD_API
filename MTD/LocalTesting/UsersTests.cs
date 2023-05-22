using AutoMapper;
using Mapper;
using RepositoryForTest;
using Services;

namespace LocalTesting;

public class UsersTests
{
    private UserService _service;
    private WorkingWhitsUsers repository;
    
    [SetUp]
    public void Setup()
    {
        MapperConfiguration mappingConfig = new MapperConfiguration(mc =>
        {
            mc.AddProfile(new MappingUser());
        });

        IMapper mapper = mappingConfig.CreateMapper();

        repository = new WorkingWhitsUsers();

        _service = new UserService(repository, mapper);
    }

    [Test]
    public async Task GetUsers()
    {
        var users = await _service.GetUsers();

        Assert.AreEqual(3, users.Count);
    }
}