using Services.DtoModels;

namespace Services.ServiceInterfaces;

public interface IUserService
{
    public Task<UserDto> GetUserByEmail(string email);
    public Task<bool> UserVerification(UserDto dto);
    public Task TryAddUser(UserDto dto);
    public Task TryUpdateUser(UserDto dto, string? oldEmail, string? oldPassword);
    public Task TryDeleteUser(string email);
}