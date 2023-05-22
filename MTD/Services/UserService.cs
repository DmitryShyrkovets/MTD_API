using AutoMapper;
using Models.RepositoryInterfaces;
using Services.ServiceInterfaces;
using Services.ViewModels;

namespace Services;

public class UserService : IUserService
{
    private readonly IUserRepository _repository;
    private readonly IMapper _mapper;
    public UserService(IUserRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<List<UserCli>> GetUsers()
    {
        List<UserCli> result = new List<UserCli>();

        var users = await _repository.GetUsers();

        foreach (var item in users)
        {
            result.Add(_mapper.Map<UserCli>(item));
        }

        return result;
    }
}