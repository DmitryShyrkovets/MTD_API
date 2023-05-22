using Models.DbModels;

namespace Models.RepositoryInterfaces;

public interface IUserRepository
{
    public Task<List<User>> GetUsers();
}