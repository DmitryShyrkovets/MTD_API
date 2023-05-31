using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using Models.DbModels;
using Models.RepositoryInterfaces;
using Services.Models.User;
using Services.Models.User.Requests;
using Services.ServiceInterfaces;

namespace Services;

public class UserService : IUserService
{
    private readonly IUserRepository _repository;
    private readonly IMapper _mapper;
    private IMemoryCache _cache;
    
    private readonly MemoryCacheEntryOptions options;
    
    public UserService(IUserRepository repository, IMapper mapper, IMemoryCache? cache)
    {
        _repository = repository;
        _mapper = mapper;
        _cache = cache;
        
        options = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5));
    }

    public async Task<UserModel> GetUserByEmail(string email)
    {
        var user = _cache?.Get<UserModel>(email);

        if (user is null)
        {
            var userDb = await _repository.GetUserByEmail(email);
            
            if (userDb is null)
                throw new Exception("User is not found!!");
            
            user = _mapper.Map<UserModel>(userDb);
            
            _cache?.Set(email, user, options);
        }

        return user;
    }

    public async Task<RecoveryModel> GetUserForRecovery(string email)
    {
        var userDb = await _repository.GetUserForRecovery(email);
            
        if (userDb is null)
            throw new Exception("User is not found!!");
            
        return _mapper.Map<RecoveryModel>(userDb);
    }

    public async Task<bool> UserVerification(AuthUserRequest authUserRequest)
    {
        return await _repository.UserVerification(authUserRequest.Email, authUserRequest.Password);
    }

    public async Task TryAddUser(AuthUserRequest authUserRequest)
    {
        if (string.IsNullOrEmpty(authUserRequest.Email) || string.IsNullOrEmpty(authUserRequest.Password))
            throw new Exception("You must fill in the data!");
        
        if (!await _repository.IsEmailUnique(authUserRequest.Email))
            throw new Exception("Email is not unique!");
        
        if (authUserRequest.Password.Length < 6)
            throw new Exception("Password must be more than 5 characters!");

        
        var userDb = _mapper.Map<User>(authUserRequest);

        await _repository.AddUser(userDb);

        var user = _mapper.Map<UserModel>(userDb);
        
        _cache?.Set(user.Email, user, options);
    }

    public async Task TryUpdateEmail(UpdateEmailRequest updateEmailRequest, string oldEmail)
    {
        await DataCheckForEmailUpdate(updateEmailRequest, oldEmail);

        var userDb = _mapper.Map<User>(updateEmailRequest);
        
        await _repository.UpdateEmail(userDb);
        
        _cache?.Remove(oldEmail);
        
        var user = _mapper.Map<UserModel>( await GetUserByEmail(userDb.Email));
        
        _cache?.Set(user.Email, user, options);
    }
    
    private async Task DataCheckForEmailUpdate(UpdateEmailRequest updateEmailRequest, string oldEmail)
    {
        if (updateEmailRequest.Id == 0 || string.IsNullOrEmpty(updateEmailRequest.Email) || string.IsNullOrEmpty(updateEmailRequest.Password))
            throw new Exception("User data is empty!");

        if (!await _repository.IsEmailUnique(updateEmailRequest.Email))
            throw new Exception("Email is not Unique!");

        if (!await _repository.UserVerification(oldEmail, updateEmailRequest.Password))
            throw new Exception("Wrong password!");

        var user = await GetUserByEmail(oldEmail);

        if (updateEmailRequest.Id != user.Id)
            throw new Exception("Wrong user id! You can't change someone else's account information!");
    }

    public async Task TryUpdatePassword(UpdatePasswordRequest updatePasswordRequest, string email)
    {
        await DataCheckForPasswordUpdate(updatePasswordRequest, email);
        
        var userDb = _mapper.Map<User>(updatePasswordRequest);
        
        await _repository.UpdatePassword(userDb);
        
        _cache?.Remove(email);
        
        var user = _mapper.Map<UserModel>( await GetUserByEmail(email));
        
        _cache?.Set(user.Email, user, options);
    }
    
    private async Task DataCheckForPasswordUpdate(UpdatePasswordRequest updatePasswordRequest, string email)
    {
        if (updatePasswordRequest.Id == 0 || string.IsNullOrEmpty(updatePasswordRequest.Password) || string.IsNullOrEmpty(updatePasswordRequest.OldPassword))
            throw new Exception("User data is empty!");
        
        if (updatePasswordRequest.Password == updatePasswordRequest.OldPassword)
            throw new Exception("Passwords are the same!");
        
        if (updatePasswordRequest.Password.Length < 6)
            throw new Exception("Password must be more than 5 characters!");

        if (!await _repository.UserVerification(email, updatePasswordRequest.OldPassword))
            throw new Exception("Wrong password!");

        var user = await GetUserByEmail(email);

        if (updatePasswordRequest.Id != user.Id)
            throw new Exception("Wrong user id! You can't change someone else's account information!");
    }

    public async Task TryDeleteUser(string email)
    {
        var user = await GetUserByEmail(email);

        await _repository.DeleteUser(user.Id);
        
        _cache?.Remove(email);
    }
}