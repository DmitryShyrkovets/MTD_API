using Microsoft.EntityFrameworkCore;
using Models.DbModels;
using Models.RepositoryInterfaces;

namespace Repository;

public class WorkingWhitsUsers: IUserRepository
{
    private readonly ApplicationContext _context;
    public WorkingWhitsUsers(ApplicationContext context)
    {
        _context = context;
    }

    public async Task<List<User>> GetUsers()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task<User> GetUserByEmail(string email)
    {
        var user = await _context.Users.Select(s => new User {Id = s.Id, Email = s.Email, Nickname = s.Nickname}).FirstOrDefaultAsync(u => u.Email == email);

        if (user == null)
        {
            throw new Exception("User is not found!");
        }
        
        return user;
    }

    public async Task<bool> UserVerification(User obj)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == obj.Email && u.Password == obj.Password) != null;
    }

    public async Task AddUser(User model)
    {
        _context.Users.Add(model);
        
        await _context.SaveChangesAsync();
    }

    public async Task ModifyUser(User model)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == model.Id);

        user.Email = model.Email;
        user.Password = model.Password;
        user.Nickname = model.Nickname;
        
        await _context.SaveChangesAsync();
    }

    public async Task ChangeEmail(int userId, string email)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

        user.Email = email;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteUser(int? id)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

        _context.Remove(user);
        
        await _context.SaveChangesAsync();
    }
}