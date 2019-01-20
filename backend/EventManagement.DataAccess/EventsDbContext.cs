using EventManagement.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace EventManagement.DataAccess
{
    public class EventsDbContext : DbContext
    {
        public EventsDbContext(DbContextOptions<EventsDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Event> Events { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Username).IsRequired().HasMaxLength(300);
                entity.HasIndex(e => e.Username).IsUnique();
                entity.Property(e => e.EmailAddress).IsRequired().HasMaxLength(300);
                entity.HasIndex(e => e.EmailAddress).IsUnique();
                entity.Property(e => e.Name).IsRequired().HasMaxLength(300);
                entity.Property(e => e.Role).IsRequired().HasMaxLength(300);
                entity.Property(e => e.Enabled).HasDefaultValue(true);
            });

            modelBuilder.Entity<Event>(entity =>
            {
                entity.Property(e => e.Name).IsRequired().HasMaxLength(300);
                entity.Property(e => e.Location).HasMaxLength(300);
            });
        }
    }
}
