using EventManagement.DataAccess.Extensions;
using EventManagement.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace EventManagement.DataAccess
{
    public class EventsDbContext : DbContext
    {
        public EventsDbContext(DbContextOptions<EventsDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<TicketType> TicketTypes { get; set; }
        public DbSet<Ticket> Tickets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Username).IsRequired().HasMaxLength(300);
                entity.HasIndex(e => e.Username).IsUnique();
                entity.Property(e => e.EmailAddress).IsRequired().HasMaxLength(300);
                entity.HasIndex(e => e.EmailAddress).IsUnique();
                entity.Property(e => e.Name).IsRequired().HasMaxLength(300);

                entity.Property(e => e.Role)
                    .IsRequired()
                    .HasMaxLength(300)
                    .HasConversion(
                        value => value.GetStringValue(),
                        value => (UserRole)Enum.Parse(typeof(UserRole), value, true));
            });

            modelBuilder.Entity<Event>(entity =>
            {
                entity.Property(e => e.Name).IsRequired().HasMaxLength(300);
                entity.Property(e => e.Location).HasMaxLength(300);
            });

            modelBuilder.Entity<TicketType>(entity =>
            {
                entity.ToTable("TicketTypes");
                entity.Property(e => e.Name).IsRequired().HasMaxLength(300);
                entity.Property(e => e.Price).HasColumnType("decimal(5, 2)");

                entity.HasOne(e => e.Event)
                    .WithMany(e => e.TicketTypes)
                    .HasForeignKey(e => e.EventId);
            });

            modelBuilder.Entity<Ticket>(entity =>
            {
                entity.Property(e => e.Mail).HasMaxLength(254);
                entity.Property(e => e.Phone).HasMaxLength(100);
                entity.Property(e => e.LastName).HasMaxLength(300);
                entity.Property(e => e.FirstName).HasMaxLength(300);
                entity.Property(e => e.Address).HasMaxLength(1000);
                entity.Property(e => e.RoomNumber).HasMaxLength(300);

                entity.Property(e => e.PaymentStatus)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasConversion(
                        value => value.GetStringValue(),
                        value => (PaymentStatus)Enum.Parse(typeof(PaymentStatus), value, true));

                entity.HasOne(e => e.Event)
                    .WithMany(e => e.Tickets)
                    .HasForeignKey(e => e.EventId);

                entity.HasOne(e => e.Creator)
                    .WithMany()
                    .HasForeignKey(e => e.CreatorId);

                entity.HasOne(e => e.Editor)
                    .WithMany()
                    .HasForeignKey(e => e.EditorId);
            });
        }
    }
}