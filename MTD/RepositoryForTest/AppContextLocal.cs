using Models.DbModels;

namespace RepositoryForTest;

public class AppContextLocal
{
    public List<User> Users;
    public List<Note> Notes;

    public AppContextLocal()
    {
        Users = new List<User>{
            new User { Id = 1, Email = "testemail@mail.ru", Password = "qweasdzxc1" },
            new User { Id = 2, Email = "testemail2@mail.ru", Password = "qwerty1" },
            new User { Id = 3, Email = "testemail3@mail.ru", Password = "qweewqqwe1" }
        };

        Notes = new List<Note>
        {
            new Note { Id = 1, Name = "TestName", Description = "TestText", CreateAt = DateTime.Now, UserId = 1, User = Users.Find(u => u.Id == 1) },
            new Note { Id = 2, Name = "TestName2", Description = "TestText2", IsDone = true,
                CreateAt = new DateTime(2023, 5, 20, 18, 30, 25), DoneAt = new DateTime(2023, 5, 20, 20, 30, 25),
                UserId = 1, User = Users.Find(u => u.Id == 1) },
            new Note { Id = 3, Name = "TestName3", Description = "TestText3", IsDone = true, 
                CreateAt = new DateTime(2023, 5, 21, 17, 30, 25), DoneAt = new DateTime(2023, 5, 21, 23, 30, 25), 
                UserId = 1, User = Users.Find(u => u.Id == 1) },
            new Note { Id = 4, Name = "TestName4", Description = "TestText4", IsDone = true,
                CreateAt = new DateTime(2023, 5, 20, 19, 30, 25), 
                UserId = 2, User = Users.Find(u => u.Id == 2) },
            new Note { Id = 5, Name = "TestName5", Description = "TestText5", CreateAt = DateTime.Now, UserId = 2, User = Users.Find(u => u.Id == 2) },
            new Note { Id = 6, Name = "TestName6", Description = "TestText6", IsDone = true,
                CreateAt = new DateTime(2023, 5, 22, 20, 00, 25), DoneAt = new DateTime(2023, 5, 22, 23, 30, 25), 
                UserId = 2, User = Users.Find(u => u.Id == 2) },
            new Note { Id = 7, Name = "TestName7", Description = "TestText7", CreateAt = DateTime.Now, UserId = 3, User = Users.Find(u => u.Id == 3) },
            new Note { Id = 8, Name = "TestName8", Description = "TestText8", IsDone = true,
                CreateAt = new DateTime(2023, 5, 23, 21, 10, 25), DoneAt = new DateTime(2023, 5, 24, 15, 30, 25), 
                UserId = 3, User = Users.Find(u => u.Id == 3) },
            new Note { Id = 9, Name = "TestName9", Description = "TestText9", IsDone = true,
                CreateAt = new DateTime(2023, 5, 24, 22, 20, 25), DoneAt = new DateTime(2023, 5, 27, 11, 10, 25), 
                UserId = 3, User = Users.Find(u => u.Id == 3) }
        };
    }
}