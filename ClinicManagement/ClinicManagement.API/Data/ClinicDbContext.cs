
using ClinicManagement.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagement.Api.Data
{
    public class ClinicDbContext : DbContext
    {
        public ClinicDbContext(
            DbContextOptions<ClinicDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users => Set<User>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users");

                entity.HasKey(u => u.UserId);

                entity.Property(u => u.UserId)
                    .HasColumnName("UserId");

                entity.Property(u => u.FullName)
                    .HasColumnName("FullName")
                    .HasMaxLength(50)
                    .IsRequired();

                entity.Property(u => u.Username)
                    .HasColumnName("UserName")
                    .HasMaxLength(50)
                    .IsRequired();

                entity.Property(u => u.PasswordHash)
                    .HasColumnName("PasswordHash")
                    .HasMaxLength(256)
                    .IsRequired();

                entity.Property(u => u.Role)
                    .HasColumnName("Role")
                    .IsRequired();

                entity.Property(u => u.IsActive)
                    .HasColumnName("isActive")
                    .IsRequired();

                entity.Property(u => u.CreatedAt)
                    .HasColumnName("CreatedDate")
                    .HasColumnType("datetime")
                    .IsRequired();

                entity.HasIndex(u => u.Username)
                    .IsUnique();
            });
        }
    }
}
