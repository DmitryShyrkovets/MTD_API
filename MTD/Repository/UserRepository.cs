using Microsoft.EntityFrameworkCore;
using Models.DbModels;
using Models.RepositoryInterfaces;

namespace Repository;

public class UserRepository: IUserRepository
{
    private readonly ApplicationContext _context;
    public UserRepository(ApplicationContext context)
    {
        _context = context;
    }
    

    public async Task<User> GetUserByEmail(string email)
    {
        return await _context.Users.Select(s => new User {Id = s.Id, Email = s.Email}).FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<bool> UserVerification(string email, string password)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email && u.Password == password) is not null;
    }
    
    public async Task<bool> IsEmailUnique(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email) is null;
    }

    public async Task AddUser(User user)
    {
        _context.Users.Add(user);
        
        await _context.SaveChangesAsync();
    }

    public async Task UpdateEmail(User user)
    {
        var userDb = await _context.Users.FirstOrDefaultAsync(u => u.Id == user.Id);
        
        userDb.Email = user.Email;
        
        await _context.SaveChangesAsync();
    }
    
    public async Task UpdatePassword(User user)
    {
        var userDb = await _context.Users.FirstOrDefaultAsync(u => u.Id == user.Id);
        
        userDb.Password = user.Password;
        
        await _context.SaveChangesAsync();
    }

    public async Task DeleteUser(int? id)
    {
        var userDb = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        
        if (userDb is null)
            throw new Exception("User is not found!");

        _context.Users.Remove(userDb);
        
        await _context.SaveChangesAsync();
    }
}