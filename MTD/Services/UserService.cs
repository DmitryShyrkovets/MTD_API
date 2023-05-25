using AutoMapper;
using Models.DbModels;
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

    public async Task<List<UserModel>> GetUsers()
    {
        var users = await _repository.GetUsers();
        
        return _mapper.Map<List<UserModel>>(users);
    }

    public async Task<UserModel> GetUserByEmail(string email)
    {
        var user = await _repository.GetUserByEmail(email);

        return _mapper.Map<UserModel>(user);
    }

    public async Task<bool> UserVerification(UserModel model)
    {
        var user = _mapper.Map<User>(model);
        
        return await _repository.UserVerification(user);
    }

    public async Task TryAddUser(UserModel model)
    {
        var user = _mapper.Map<User>(model);

        await _repository.AddUser(user);
    }

    public async Task TryModifyUser(UserModel model, string email)
    {
        await UserDataChangeCheck(model, email);
        
        var user = _mapper.Map<User>(model);

        await _repository.ModifyUser(user);
    }

    public async Task TryChangeEmail(UserModel model, string email)
    {
        await UserDataChangeCheck(model, email);
        
        var user = _mapper.Map<User>(model);

        await _repository.ChangeEmail(user.Id, user.Email);
    }

    private async Task UserDataChangeCheck(UserModel user, string email)
    {
        var userCheck = await GetUserByEmail(email);

        if (user.Id != userCheck.Id)
        {
            throw new Exception("You can't change someone else's account information");
        }
    }

    public async Task TryDeleteUser(string email)
    {
        var user = await GetUserByEmail(email);

        await _repository.DeleteUser(user.Id);
    }
}