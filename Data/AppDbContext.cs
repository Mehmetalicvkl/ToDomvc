using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;
using ToDomvs.Models;

namespace ToDomvs.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<UserModel> Users { get; set; }
        public DbSet<ProjectModel> Projects { get; set; }
        public DbSet<TaskItemModel> TaskItems { get; set; }

        protected override void OnModelCreating(ModelBuilder mb)
        {
            base.OnModelCreating(mb);
            mb.Entity<ProjectModel>()
                .HasOne(p => p.Owner)
                .WithMany(u => u.Projects)
                .HasForeignKey(p => p.OwnerID)
                .OnDelete(DeleteBehavior.Cascade);

            mb.Entity<TaskItemModel>()
                .HasKey(t => t.TaskID);

            mb.Entity<TaskItemModel>()
                .HasOne(t => t.Project)
                .WithMany(p=> p.Tasks)
                .HasForeignKey(t => t.ProjectID)
                .OnDelete(DeleteBehavior.Cascade);

            mb.Entity<TaskItemModel>()
            .HasOne(t => t.AssignedUser)
            .WithMany(u => u.AssignedTasks)
            .HasForeignKey(t => t.AssignedUserID)
            .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
