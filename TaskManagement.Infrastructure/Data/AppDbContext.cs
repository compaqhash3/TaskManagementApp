using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Domain.Entities;

namespace TaskManagement.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<TaskItem> Tasks => Set<TaskItem>();
        public DbSet<User> Users => Set<User>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // TaskItem configuration
            modelBuilder.Entity<TaskItem>(builder =>
            {
                builder.ToTable("Tasks");
                builder.HasKey(t => t.Id);
                builder.Property(t => t.Title)
                    .IsRequired()
                    .HasMaxLength(200);
                builder.Property(t => t.IsCompleted)
                    .HasDefaultValue(false);
            });

            // User configuration
            modelBuilder.Entity<User>(builder =>
            {
                builder.ToTable("Users");
                builder.HasKey(u => u.Id);
                builder.Property(u => u.Username)
                    .IsRequired()
                    .HasMaxLength(100);
                builder.HasIndex(u => u.Username).IsUnique();
                builder.Property(u => u.PasswordHash)
                    .IsRequired();
            });

            

            base.OnModelCreating(modelBuilder);
        }
    }
}
