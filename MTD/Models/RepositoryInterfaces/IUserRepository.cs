using Models.DbModels;

namespace Models.RepositoryInterfaces;

public interface IUserRepository
{
    public Task<User> GetUserByEmail(string email);
    public Task<bool> UserVerification(User obj);
    public Task AddUser(User model);
    public Task ModifyUser(User model, string? oldPassword, string? oldEmail);
    public Task DeleteUser(int? id);
}