using Models.DbModels;
using Models.RepositoryInterfaces;

namespace RepositoryForTest;

public class UserRepository: IUserRepository
{
    private readonly AppContextLocal _context;
    public UserRepository()
    {
        _context = new AppContextLocal();
    }
    

    public async Task<User> GetUserByEmail(string email)
    {
        return _context.Users.Select(s => new User {Id = s.Id, Email = s.Email}).FirstOrDefault(u => u.Email == email);
    }

    public async Task<bool> UserVerification(string email, string password)
    {
        return _context.Users.FirstOrDefault(u => u.Email == email && u.Password == password) != null;
    }

    public async Task<bool> IsEmailUnique(string email)
    {
        return  _context.Users.FirstOrDefault(u => u.Email == email) is null;
    }

    public async Task AddUser(User user)
    {
        _context.Users.Add(user);
    }
    
    public async Task UpdateEmail(User user)
    {
        var userDb = _context.Users.FirstOrDefault(u => u.Id == user.Id);

        userDb.Email = user.Email;
    }
    
    public async Task UpdatePassword(User user)
    {
        var userDb = _context.Users.FirstOrDefault(u => u.Id == user.Id);
        
        userDb.Password = user.Password;
    }

    public async Task DeleteUser(int? id)
    {
        var userDb =  _context.Users.FirstOrDefault(u => u.Id == id);

        _context.Users.Remove(userDb);
    }
}