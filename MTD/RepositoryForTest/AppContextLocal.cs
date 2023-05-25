using Models.DbModels;

namespace RepositoryForTest;

public class AppContextLocal
{
    public List<User> Users;

    public AppContextLocal()
    {
        Users = new List<User>{
            new User { Id = 1, Nickname = "Tom1", Email = "testemail@mail.ru", Password = "qweasdzxc1" },
            new User { Id = 2, Nickname = "Bob1", Email = "testemail2@mail.ru", Password = "qwerty1" },
            new User { Id = 3, Nickname = "Tom1", Email = "testemail3@mail.ru", Password = "qweewqqwe1" }
        };
    }
}