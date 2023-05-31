using Microsoft.EntityFrameworkCore;
using Models.DbModels;

namespace Repository;

public class ApplicationContext : DbContext
{
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Note> Notes { get; set; } = null!;

    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
    {
        //Database.EnsureDeleted();
        Database.EnsureCreated(); 
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        List<User> users = new List<User>()
        {
            new User { Id = 1, Email = "testemail@mail.ru", Password = "qweasdzxc" },
            new User { Id = 2, Email = "testemail2@mail.ru", Password = "qwerty" },
            new User { Id = 3, Email = "testemail3@mail.ru", Password = "qweewqqwe" }
        };

        List<Note> notes = new List<Note>()
        {
            new Note {Id = 1, Name = "TestName", Description = "TestText", CreateAt = DateTime.Now, UserId = 1},
            new Note {Id = 2, Name = "TestName2", Description = "TestText2", Done = true,
                CreateAt = new DateTime(2023, 5, 20, 18, 30, 25), DoneAt = new DateTime(2023, 5, 20, 20, 30, 25), UserId = 1},
            new Note {Id = 3, Name = "TestName3", Description = "TestText3", Done = true, 
                CreateAt = new DateTime(2023, 5, 21, 17, 30, 25), DoneAt = new DateTime(2023, 5, 21, 23, 30, 25), UserId = 1},
            new Note {Id = 4, Name = "TestName4", Description = "TestText4", Done = true,
                CreateAt = new DateTime(2023, 5, 20, 19, 30, 25), UserId = 2},
            new Note {Id = 5, Name = "TestName5", Description = "TestText5", CreateAt = DateTime.Now, UserId = 2},
            new Note {Id = 6, Name = "TestName6", Description = "TestText6", Done = true,
                CreateAt = new DateTime(2023, 5, 22, 20, 00, 25), DoneAt = new DateTime(2023, 5, 22, 23, 30, 25), UserId = 2},
            new Note {Id = 7, Name = "TestName7", Description = "TestText7", CreateAt = DateTime.Now, UserId = 3},
            new Note {Id = 8, Name = "TestName8", Description = "TestText8", Done = true,
                CreateAt = new DateTime(2023, 5, 23, 21, 10, 25), DoneAt = new DateTime(2023, 5, 24, 15, 30, 25), UserId = 3},
            new Note {Id = 9, Name = "TestName9", Description = "TestText9", Done = true,
                CreateAt = new DateTime(2023, 5, 24, 22, 20, 25), DoneAt = new DateTime(2023, 5, 27, 11, 10, 25), UserId = 3}
        };

        modelBuilder.Entity<User>().HasData(users);
        
        modelBuilder.Entity<Note>().HasData(notes);
    }
}