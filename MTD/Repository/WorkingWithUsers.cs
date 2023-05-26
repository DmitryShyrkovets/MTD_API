using Microsoft.EntityFrameworkCore;
using Models.DbModels;
using Models.RepositoryInterfaces;

namespace Repository;

public class WorkingWithUsers: IUserRepository
{
    private readonly ApplicationContext _context;
    public WorkingWithUsers(ApplicationContext context)
    {
        _context = context;
    }
    

    public async Task<User> GetUserByEmail(string email)
    {
        var user = await _context.Users.Select(s => new User {Id = s.Id, Email = s.Email, Nickname = s.Nickname}).FirstOrDefaultAsync(u => u.Email == email);

        if (user == null)
            throw new Exception("User is not found!");

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
        
        if (user == null)
            throw new Exception("User is not found!");

        user.Email = model.Email;
        user.Password = model.Password;
        user.Nickname = model.Nickname;
        
        await _context.SaveChangesAsync();
    }

    public async Task ChangeEmail(int userId, string email)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        
        if (user == null)
            throw new Exception("User is not found!");

        user.Email = email;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteUser(int? id)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        
        if (user == null)
            throw new Exception("User is not found!");

        _context.Users.Remove(user);
        
        await _context.SaveChangesAsync();
    }
}