using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using Models.DbModels;
using Models.RepositoryInterfaces;
using Services.ServiceInterfaces;
using Services.DtoModels;

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

    public async Task<UserDto> GetUserByEmail(string email)
    {
        var user = _cache?.Get<UserDto>(email);

        if (user is null)
        {
            var userDb = await _repository.GetUserByEmail(email);
            
            if (userDb is null)
                throw new Exception("User is not found!!");
            
            var userModel = _mapper.Map<UserDto>(userDb);
            
            _cache?.Set(email, userModel, options);
            
            return userModel;
        }

        return _mapper.Map<UserDto>(user);
    }

    public async Task<bool> UserVerification(UserDto dto)
    {
        return await _repository.UserVerification(dto.Email, dto.Password);
    }

    public async Task TryAddUser(UserDto dto)
    {
        if (string.IsNullOrEmpty(dto.Email) || string.IsNullOrEmpty(dto.Password))
            throw new Exception("You must fill in the data!");
        if (! await _repository.IsEmailUnique(dto.Email))
            throw new Exception("Email is not unique!");
        
        var user = _mapper.Map<User>(dto);

        await _repository.AddUser(user);
        
        user.Password = null;
        
        var userModel = _mapper.Map<UserDto>(user);
        
        _cache?.Set(user.Email, userModel, options);
    }

    public async Task TryUpdateUser(UserDto dto, string? oldEmail, string? oldPassword)
    {
        await UserDataChangeCheck(dto, oldEmail, oldPassword);
        
        var user = _mapper.Map<User>(dto);
        
        await _repository.UpdateUser(user);
        
        _cache?.Remove(oldEmail ?? dto.Email);
        
        dto = _mapper.Map<UserDto>( await GetUserByEmail(user.Email));
        
        _cache?.Set(dto.Email, dto, options);
    }

    private async Task UserDataChangeCheck(UserDto user, string? email, string? oldPassword)
    {
        if (user.Id is null || string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.Password))
            throw new Exception("User data is empty!");
        
        if (!string.IsNullOrEmpty(email) && !await _repository.IsEmailUnique(user.Email))
            throw new Exception("Email is not Unique!");

        if (!string.IsNullOrEmpty(oldPassword) && !await _repository.UserVerification(user.Email, oldPassword))
            throw new Exception("Wrong password!");
        
        if (!string.IsNullOrEmpty(email) && !await _repository.UserVerification(email, user.Password))
            throw new Exception("Wrong password!");
        
        if (!string.IsNullOrEmpty(email) && user.Email == email)
            throw new Exception("Emails are the same!");
        
        if (!string.IsNullOrEmpty(oldPassword) && user.Password == oldPassword)
            throw new Exception("Passwords are the same!");

        var userObj = await GetUserByEmail(email ?? user.Email);

        if (user.Id != userObj.Id)
            throw new Exception("Wrong user id! You can't change someone else's account information!");

    }

    public async Task TryDeleteUser(string email)
    {
        var user = await GetUserByEmail(email);

        await _repository.DeleteUser(user.Id);
        
        _cache?.Remove(email);
    }
}