using Harmonizer.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TaskEnitity = Harmonizer.Model.TaskEnitity;


namespace Harmonizer.Data
{
    public class ApplicationDBContext : IdentityDbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<TaskEnitity> Tasks { get; set; }
        public DbSet<Status> Statuses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Task
            modelBuilder.Entity<TaskEnitity>()
                .HasKey(t => t.Id);

            modelBuilder.Entity<TaskEnitity>()
                .HasOne(t => t.User)
                .WithMany(u => u.Tasks)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TaskEnitity>()
                .HasOne(t => t.Status)
                .WithMany(s => s.Tasks)
                .HasForeignKey(t => t.StatusId);

            // Configure Status
            modelBuilder.Entity<Status>()
                .HasKey(s => s.Id);

            // Seed Statuses
            modelBuilder.Entity<Status>().HasData(
                new Status { Id = 1, Name = "Todo" },
                new Status { Id = 2, Name = "In Progress" },
                new Status { Id = 3, Name = "Done" }

            );

            _ = modelBuilder.Entity<User>().HasData(
        new User
        {
            UserId = 1,
            Email = "user@example.com",
            Name = "User",
            Role = "User",
            CreatedDate = DateTime.UtcNow
        }
    );
        }


    }
}
