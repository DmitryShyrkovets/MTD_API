using Microsoft.EntityFrameworkCore;
using Models.DbModels;
using Models.RepositoryInterfaces;

namespace Repository;

public class WorkingWhitsUsers: IUserRepository
{
    private ApplicationContext _context;
    public WorkingWhitsUsers(ApplicationContext context)
    {
        _context = context;
    }

    public async Task<List<User>> GetUsers()
    {
        return await _context.Users.ToListAsync();
    }
}