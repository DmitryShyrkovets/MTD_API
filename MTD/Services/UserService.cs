using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using Models.DbModels;
using Models.RepositoryInterfaces;
using Services.ServiceInterfaces;
using Services.ViewModels;

namespace Services;

public class UserService : IUserService
{
    private readonly IUserRepository _repository;
    private readonly IMapper _mapper;
    private IMemoryCache _cache;
    
    private readonly MemoryCacheEntryOptions options;
    
    public UserService(IUserRepository repository, IMapper mapper, IMemoryCache cache = null)
    {
        _repository = repository;
        _mapper = mapper;
        _cache = cache;
        
        options = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5));
    }

    public async Task<UserModel> GetUserByEmail(string email)
    {
        var user = _cache?.Get<UserModel>(email);

        if (user == null)
        {
            var userDb = await _repository.GetUserByEmail(email);
            var userModel = _mapper.Map<UserModel>(userDb);
            
            _cache?.Set(email, userModel, options);
            
            return userModel;
        }

        return _mapper.Map<UserModel>(user);
    }

    public async Task<bool> UserVerification(UserModel model)
    { 
        var userDb = _mapper.Map<User>(model);
        
        return await _repository.UserVerification(userDb);
    }

    public async Task TryAddUser(UserModel model)
    {
        if (model.Email == null || model.Password == null || model.Nickname == null)
            throw new Exception("You must fill in the data!");
        
        var user = _mapper.Map<User>(model);

        await _repository.AddUser(user);
        
        user.Password = null;
        
        var userModel = _mapper.Map<UserModel>(user);
        
        _cache?.Set(user.Email, userModel, options);
    }

    public async Task TryModifyUser(UserModel model, string? oldPassword, string? oldEmail)
    {
        await UserDataChangeCheck(model, oldEmail ?? model.Email);
        
        var user = _mapper.Map<User>(model);

        await _repository.ModifyUser(user, oldPassword, oldEmail);

        _cache?.Remove(oldEmail ?? model.Email);
        
        model = _mapper.Map<UserModel>( await GetUserByEmail(user.Email));
        
        _cache?.Set(model.Email, model, options);
    }

    private async Task UserDataChangeCheck(UserModel user, string email)
    {
        var userObj = await GetUserByEmail(email);

        if (user.Id != userObj.Id)
        {
            throw new Exception("You can't change someone else's account information");
        }
    }

    public async Task TryDeleteUser(string email)
    {
        var user = await GetUserByEmail(email);

        await _repository.DeleteUser(user.Id);
        
        _cache?.Remove(email);
    }
}