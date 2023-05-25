using Services.ViewModels;

namespace Services.ServiceInterfaces;

public interface IUserService
{
    public Task<List<UserModel>> GetUsers();
    public Task<UserModel> GetUserByEmail(string email);
    public Task<bool> UserVerification(UserModel model);
    public Task TryAddUser(UserModel model);
    public Task TryModifyUser(UserModel model, string email);
    public Task TryChangeEmail(UserModel model, string email);
    public Task TryDeleteUser(string email);
}