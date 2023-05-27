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

    public async Task ModifyUser(User model, string? oldPassword, string? oldEmail)
    {
        var password = oldPassword ?? model.Password;
        
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == model.Id);
        
        if (user == null)
            throw new Exception("User is not found!");
        
        if(password != null && user.Password != password)
            throw new Exception("Password is wrong");
        
        if(oldEmail != null && user.Email != oldEmail)
            throw new Exception("Email is wrong");

        if (oldPassword != null)
            user.Password = model.Password;
        
        if (model.Password != null && oldEmail != null)
            user.Email = model.Email;
        
        if (model.Nickname != null)
            user.Nickname = model.Nickname;

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