using Models.DbModels;

namespace Models.RepositoryInterfaces;

public interface IUserRepository
{
    public Task<User> GetUserByEmail(string email);
    public Task<bool> UserVerification(string email, string password);
    public Task<bool> IsEmailUnique(string email);
    public Task AddUser(User model);
    public Task UpdateUser(User model);
    public Task DeleteUser(int? id);
}