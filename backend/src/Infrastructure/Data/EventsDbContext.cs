using EventManagement.ApplicationCore.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace EventManagement.Infrastructure.Data
{
    public class EventsDbContext : DbContext
    {
        public EventsDbContext(DbContextOptions<EventsDbContext> options) : base(options)
        {
        }

        public DbSet<Client> Clients { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<MasterQrCode> MasterQrCodes { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<TicketType> TicketTypes { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<MailSettings> MailSettings { get; set; }
        public DbSet<AuditEvent> AuditEventLog { get; set; }

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
                entity.Property(e => e.Location).IsRequired().HasMaxLength(300);
                entity.Property(e => e.HomepageUrl).IsRequired().HasMaxLength(2083);
                entity.Property(e => e.Host).IsRequired().HasMaxLength(300);
                entity.Property(e => e.Address).IsRequired().HasMaxLength(300);
                entity.Property(e => e.ZipCode).IsRequired().HasMaxLength(300);
                entity.Property(e => e.City).IsRequired().HasMaxLength(300);
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
                entity.HasQueryFilter(e => !e.IsDeleted);

                entity.HasAlternateKey(e => e.TicketNumber);
                entity.HasAlternateKey(e => e.TicketSecret);

                entity.Property(e => e.TicketNumber).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Mail).HasMaxLength(254);
                entity.Property(e => e.Phone).HasMaxLength(100);
                entity.Property(e => e.LastName).HasMaxLength(300);
                entity.Property(e => e.FirstName).HasMaxLength(300);
                entity.Property(e => e.Address).HasMaxLength(1000);
                entity.Property(e => e.RoomNumber).HasMaxLength(300);
                entity.Property(e => e.AmountPaid).HasColumnType("decimal(5, 2)");

                entity.Property(e => e.PaymentStatus)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasConversion(
                        value => value.GetStringValue(),
                        value => (PaymentStatus)Enum.Parse(typeof(PaymentStatus), value, true));

                entity.Property(e => e.DeliveryType)
                    .HasMaxLength(100)
                    .HasConversion(
                        value => value.Value.GetStringValue(),
                        value => (TicketDeliveryType)Enum.Parse(typeof(TicketDeliveryType), value, true));

                entity.HasOne(e => e.Event)
                    .WithMany(e => e.Tickets)
                    .HasForeignKey(e => e.EventId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.TicketType)
                    .WithMany(e => e.Tickets)
                    .HasForeignKey(e => e.TicketTypeId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Creator)
                    .WithMany()
                    .HasForeignKey(e => e.CreatorId);

                entity.HasOne(e => e.Editor)
                    .WithMany()
                    .HasForeignKey(e => e.EditorId);
            });

            modelBuilder.Entity<MasterQrCode>(entity =>
            {
                entity.ToTable("MasterQrCodes");

                entity.HasOne(e => e.Owner)
                    .WithMany(e => e.MasterQrCodes)
                    .HasForeignKey(e => e.OwnerId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Event)
                    .WithMany()
                    .HasForeignKey(e => e.EventId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Client>(entity =>
            {
                entity.Property(e => e.Name).IsRequired().HasMaxLength(1000);
                entity.Property(e => e.Secret).IsRequired().HasMaxLength(1000);
            });

            modelBuilder.Entity<MailSettings>(entity =>
            {
                entity.ToTable("MailSettings");

                entity.Property(e => e.SmtpHost).IsRequired().HasMaxLength(300);
                entity.Property(e => e.SmtpUsername).HasMaxLength(300);
                entity.Property(e => e.SmtpPassword).HasMaxLength(300);
                entity.Property(e => e.SenderAddress).IsRequired().HasMaxLength(300);
                entity.Property(e => e.Subject).IsRequired().HasMaxLength(300);

                entity.HasOne(e => e.Event)
                    .WithOne(e => e.MailSettings)
                    .HasForeignKey<Event>(e => e.MailSettingsId);

                entity.HasMany(e => e.DemoEmailRecipients)
                      .WithOne()
                      .HasForeignKey(e => e.MailSettingsId);
            });

            modelBuilder.Entity<DemoEmailRecipient>(entity =>
            {
                entity.ToTable("DemoEmailRecipients");
                entity.Property(e => e.EmailAddress).IsRequired().HasMaxLength(300);
            });

            modelBuilder.Entity<AuditEvent>(entity =>
            {
                entity.ToTable("AuditEventLog");
                entity.Property(e => e.Action).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Detail).HasMaxLength(1000);

                entity.HasOne(e => e.Ticket)
                      .WithMany()
                      .HasForeignKey(e => e.TicketId);
            });
        }
    }
}