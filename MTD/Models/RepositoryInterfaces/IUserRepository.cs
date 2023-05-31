using Models.DbModels;

namespace Models.RepositoryInterfaces;

public interface IUserRepository
{
    public Task<User> GetUserByEmail(string email);
    public Task<User> GetUserForRecovery(string email);
    public Task<bool> UserVerification(string email, string password);
    public Task<bool> IsEmailUnique(string email);
    public Task AddUser(User user);
    public Task UpdateEmail(User user);
    public Task UpdatePassword(User user);
    public Task DeleteUser(int? id);
}