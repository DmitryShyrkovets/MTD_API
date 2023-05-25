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
            new User { Id = 1, Nickname = "Tom", Email = "testemail@mail.ru", Password = "qweasdzxc" },
            new User { Id = 2, Nickname = "Bob", Email = "testemail2@mail.ru", Password = "qwerty" },
            new User { Id = 3, Nickname = "Tom", Email = "testemail3@mail.ru", Password = "qweewqqwe" }
        };

        List<Note> notes = new List<Note>()
        {
            new Note {Id = 1, Category = "Test", Name = "TestName", Text = "TestText", UserId = 1},
            new Note {Id = 2, Category = "Test2", Name = "TestName2", Text = "TestText2", UserId = 1},
            new Note {Id = 3, Category = "Test3", Name = "TestName3", Text = "TestText3", UserId = 1},
            new Note {Id = 4, Category = "Test4", Name = "TestName4", Text = "TestText4", UserId = 2},
            new Note {Id = 5, Category = "Test5", Name = "TestName5", Text = "TestText5", UserId = 2},
            new Note {Id = 6, Category = "Test6", Name = "TestName6", Text = "TestText6", UserId = 2},
            new Note {Id = 7, Category = "Test7", Name = "TestName7", Text = "TestText7", UserId = 3},
            new Note {Id = 8, Category = "Test8", Name = "TestName8", Text = "TestText8", UserId = 3},
            new Note {Id = 9, Category = "Test9", Name = "TestName9", Text = "TestText9", UserId = 3}
        };

        modelBuilder.Entity<User>().HasData(users);
        
        modelBuilder.Entity<Note>().HasData(notes);
    }
}