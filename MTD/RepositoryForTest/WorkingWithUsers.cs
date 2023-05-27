using Models.DbModels;
using Models.RepositoryInterfaces;

namespace RepositoryForTest;

public class WorkingWithUsers: IUserRepository
{
    private readonly AppContextLocal _context;
    public WorkingWithUsers()
    {
        _context = new AppContextLocal();
    }
    

    public async Task<User> GetUserByEmail(string email)
    {
        var user =  _context.Users.Select(s => new User {Id = s.Id, Email = s.Email, Nickname = s.Nickname}).FirstOrDefault(u => u.Email == email);

        if (user == null)
        {
            throw new Exception("User is not found!");
        }
        
        return user;
    }

    public async Task<bool> UserVerification(User obj)
    {
        return _context.Users.FirstOrDefault(u => u.Email == obj.Email && u.Password == obj.Password) != null;
    }

    public async Task AddUser(User model)
    {
        _context.Users.Add(model);
    }

    public async Task ModifyUser(User model, string? oldPassword, string? oldEmail)
    {
        var password = oldPassword ?? model.Password;
        
        var user = _context.Users.FirstOrDefault(u => u.Id == model.Id);
        
        if (user == null)
            throw new Exception("User is not found!");
        
        if(password != null && user.Password != password)
            throw new Exception("Password is wrong");
        
        if(oldEmail != null && user.Email != oldEmail)
            throw new Exception("Email is wrong");

        if (oldPassword != null)
            user.Password = oldPassword;
        
        if (model.Password != null && oldEmail != null)
            user.Email = model.Email;
        
        if (model.Nickname != null)
            user.Nickname = model.Nickname;
    }

    public async Task ChangeEmail(int userId, string email)
    {
        var user = _context.Users.FirstOrDefault(u => u.Id == userId);

        user.Email = email;
    }

    public async Task DeleteUser(int? id)
    {
        var user =  _context.Users.FirstOrDefault(u => u.Id == id);

        _context.Users.Remove(user);
    }
}