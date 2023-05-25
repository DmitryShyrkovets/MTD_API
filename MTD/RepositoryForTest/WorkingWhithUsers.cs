using Models.DbModels;
using Models.RepositoryInterfaces;

namespace RepositoryForTest;

public class WorkingWhitsUsers: IUserRepository
{
    private AppContextLocal context;
    public WorkingWhitsUsers()
    {
        context = new AppContextLocal();
    }

    public async Task<List<User>> GetUsers()
    {
        return  context.Users;
    }

    public Task<User> GetUserByEmail(string email)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UserVerification(User obj)
    {
        throw new NotImplementedException();
    }

    public Task AddUser(User model)
    {
        throw new NotImplementedException();
    }

    public Task ModifyUser(User model)
    {
        throw new NotImplementedException();
    }

    public Task ChangeEmail(int userId, string email)
    {
        throw new NotImplementedException();
    }

    public Task DeleteUser(int? id)
    {
        throw new NotImplementedException();
    }
}