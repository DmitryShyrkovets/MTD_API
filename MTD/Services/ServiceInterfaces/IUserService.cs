using Services.ViewModels;

namespace Services.ServiceInterfaces;

public interface IUserService
{
    public Task<List<UserCli>> GetUsers();
}