using Microsoft.EntityFrameworkCore;
using Models.DbModels;

namespace Repository;

public class ApplicationContext : DbContext
{
    public DbSet<User> Users { get; set; } = null!;

    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
    {
        //Database.EnsureDeleted();
        Database.EnsureCreated(); 
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasData(
            new User { Id = 1, Name = "Tom", SurName = "Walker" },
            new User { Id = 2, Name = "Bob", SurName = "Marly" },
            new User { Id = 3, Name = "Tom", SurName = "Holland" }
        );
    }
}