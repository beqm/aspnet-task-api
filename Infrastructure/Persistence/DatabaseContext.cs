using Domain.Models;
using Microsoft.EntityFrameworkCore;
using TaskModel = Domain.Models.Task;

namespace Infrastructure.Persistence;

public class DatabaseContext : DbContext
{
    public DbSet<User> Users => Set<User>();
    public DbSet<TaskModel> Tasks => Set<TaskModel>();

    public DatabaseContext(DbContextOptions<DatabaseContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<TaskModel>()
            .HasOne(t => t.User)
            .WithMany(u => u.Tasks)
            .HasForeignKey(t => t.UserId)
            .OnDelete(DeleteBehavior.Cascade);

    }
}
