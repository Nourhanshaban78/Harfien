using Harfien.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Harfien.DataAccess
{
    public class HarfienDbContext : IdentityDbContext<ApplicationUser>
    {
        public HarfienDbContext(DbContextOptions<HarfienDbContext> options)
            : base(options)
        {
        }

        #region DbSets
        public DbSet<Service> Services { get; set; } = null!;
        public DbSet<ServiceCategory> ServiceCategories { get; set; } = null!;
        public DbSet<Client> Clients { get; set; } = null!;
        public DbSet<Craftsman> Craftsmen { get; set; } = null!;
        public DbSet<Order> Orders { get; set; } = null!;
        public DbSet<Payment> Payments { get; set; } = null!;
        public DbSet<City> Cities { get; set; } = null!;
        public DbSet<Area> Areas { get; set; } = null!;
        public DbSet<Complaint> Complaints { get; set; } = null!;
        public DbSet<Review> Reviews { get; set; } = null!;
        public DbSet<ChatMessage> ChatMessages { get; set; } = null!;
        public DbSet<CraftsmanAvailability> CraftsmanAvailabilities { get; set; } = null!;
        public DbSet<Notification> Notifications { get; set; } = null!;
        public DbSet<Wallet> Wallet { get; set; } = null!;
        #endregion

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(typeof(HarfienDbContext).Assembly);

            #region User Relations

            builder.Entity<Client>()
                .HasOne(c => c.User)
                .WithOne()
                .HasForeignKey<Client>(c => c.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Craftsman>()
                .HasOne(c => c.User)
                .WithOne()
                .HasForeignKey<Craftsman>(c => c.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Wallet>()
                .HasOne(w => w.User)
                .WithOne(u => u.Wallet)
                .HasForeignKey<Wallet>(w => w.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Notification>()
                .HasOne(n => n.ApplicationUser)
                .WithMany(u => u.Notifications)
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            #endregion

            #region Order Relations

            builder.Entity<Order>()
                .HasOne(o => o.Client)
                .WithMany(c => c.Orders)
                .HasForeignKey(o => o.ClientId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Order>()
                .HasOne(o => o.Craftsman)
                .WithMany(c => c.Orders)
                .HasForeignKey(o => o.CraftsmanId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Order>()
                .HasOne(o => o.Service)
                .WithMany(s => s.Orders)
                .HasForeignKey(o => o.ServiceId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Order>()
                .HasOne(o => o.Payment)
                .WithOne(p => p.Order)
                .HasForeignKey<Payment>(p => p.OrderId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Order>()
                .Property(o => o.Amount)
                .HasPrecision(18, 2);

            #endregion

            #region Service Relations

            builder.Entity<Service>()
                .HasOne(s => s.ServiceCategory)
                .WithMany(c => c.Services)
                .HasForeignKey(s => s.ServiceCategoryId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Service>()
                .HasOne(s => s.Craftsman)
                .WithMany(c => c.CraftsmanServices)
                .HasForeignKey(s => s.CraftsmanId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Service>()
                .Property(s => s.Price)
                .HasPrecision(18, 2);

            #endregion

            #region Chat

            builder.Entity<ChatMessage>(entity =>
            {
                entity.HasKey(m => m.Id);

                entity.Property(m => m.Content)
                      .IsRequired()
                      .HasMaxLength(1000);

                entity.Property(m => m.SentAt)
                      .IsRequired();

                entity.HasIndex(m => new { m.SenderId, m.ReceiverId });
            });

            #endregion

            #region Decimal Fixes

            builder.Entity<Payment>()
                .Property(p => p.Amount)
                .HasPrecision(18, 2);

            builder.Entity<Wallet>()
                .HasOne(w => w.User)
                .WithOne(u => u.Wallet)
                .HasForeignKey<Wallet>(w => w.UserId);
            builder.Entity<Notification>()
                 .HasOne(n => n.ApplicationUser)
                 .WithMany(u => u.Notifications)
                 .HasForeignKey(n => n.UserId);

            builder.Entity<ChatMessage>()
        .HasOne(m => m.Sender)
        .WithMany()
        .HasForeignKey(m => m.SenderId)
        .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ChatMessage>()
                .HasOne(m => m.Receiver)
                .WithMany()
                .HasForeignKey(m => m.ReceiverId)
                .OnDelete(DeleteBehavior.Restrict);


            builder.Entity<WalletTransaction>()
                .Property(w => w.Amount)
                .HasPrecision(18, 2);

            builder.Entity<SubscriptionPlan>()
                .Property(s => s.Price)
                .HasPrecision(18, 2);

            #endregion

            #region Seed Data

            var fixedDate = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            builder.Entity<IdentityRole>().HasData(
                new IdentityRole { Id = "1", Name = "Admin", NormalizedName = "ADMIN" },
                new IdentityRole { Id = "2", Name = "Craftsman", NormalizedName = "CRAFTSMAN" },
                new IdentityRole { Id = "3", Name = "Client", NormalizedName = "CLIENT" }
            );

            var hasher = new PasswordHasher<ApplicationUser>();

            var adminUser = new ApplicationUser
            {
                Id = "ADMIN_ID",
                UserName = "Admin@gmail.com",
                NormalizedUserName = "ADMIN@GMAIL.COM",
                Email = "Admin@gmail.com",
                NormalizedEmail = "ADMIN@GMAIL.COM",
                PhoneNumber = "1234567890",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                LockoutEnabled = false,
                FullName = "Admin",
                Address = "Cairo",
                CreatedAt = fixedDate,
                IsActive = true
            };

            adminUser.PasswordHash = hasher.HashPassword(adminUser, "Admin123456");

            builder.Entity<ApplicationUser>().HasData(adminUser);

            builder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    RoleId = "1",
                    UserId = "ADMIN_ID"
                });

            #endregion
        }
    }
}
