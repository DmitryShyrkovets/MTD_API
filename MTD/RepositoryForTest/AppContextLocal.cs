using Models.DbModels;

namespace RepositoryForTest;

public class AppContextLocal
{
    public List<User> Users;

    public AppContextLocal()
    {
        Users = new List<User>{
            (new User { Id = 10, SurName = "SurName12", Name = "Name12" }),
            (new User { Id = 11, SurName = "SurName22", Name = "Name22" }),
            (new User { Id = 12, SurName = "SurName32", Name = "Name32" })
        };
    }
}