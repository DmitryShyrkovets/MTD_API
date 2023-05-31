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

    public async Task AddUser(User model)
    {
        _context.Users.Add(model);
    }

    public async Task UpdateUser(User model)
    {
        var user = _context.Users.FirstOrDefault(u => u.Id == model.Id);

        user.Email = model.Email;
        user.Password = model.Password;
    }

    public async Task DeleteUser(int? id)
    {
        var user =  _context.Users.FirstOrDefault(u => u.Id == id);

        _context.Users.Remove(user);
    }
}