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
}