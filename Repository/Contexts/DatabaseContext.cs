
using Microsoft.EntityFrameworkCore;
using Repository.Models;

namespace Repository.Contexts
{
    public class DatabaseContext : DbContext
    {
        public DbSet<EfUser> Users { get; set; }
        public DbSet<EFGroup> Groups { get; set; }
        public DbSet<EfGroupUser> GroupUser { get; set; }

        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuración de Usuario
            modelBuilder.Entity<EfUser>(entity =>
            {
                entity.ToTable("Users");
                entity.HasKey(e => e.ID);

                entity.Property(e => e.PublicId)
                    .HasDefaultValueSql("NEWID()")
                    .IsRequired();

                entity.HasIndex(e => e.PublicId)
                    .IsUnique()
                    .HasDatabaseName("UQ_Users_PublicId");

                entity.Property(e => e.Username)
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(e => e.Email)
                    .HasMaxLength(255)
                    .IsRequired();

                entity.Property(e => e.PasswordHash)
                    .HasMaxLength(512)
                    .IsRequired();

                entity.Property(e => e.PasswordSalt)
                    .HasMaxLength(512)
                    .IsRequired();

                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("GETDATE()");

                entity.Property(e => e.IsActive)
                    .HasDefaultValue(true);

                entity.HasIndex(e => e.Username)
                    .IsUnique()
                    .HasDatabaseName("UQ_Users_Username");

                entity.HasIndex(e => e.Email)
                    .IsUnique()
                    .HasDatabaseName("UQ_Users_Email");

                entity.HasOne(e => e.PrimaryGroup)
                    .WithMany()
                    .HasForeignKey(e => e.PrimaryGroupID)
                    .HasConstraintName("FK_Users_PrimaryGroupID");

                entity.HasOne(e => e.Creator)
                    .WithMany()
                    .HasForeignKey(e => e.CreatedBy)
                    .HasConstraintName("FK_Users_CreatedBy");

                // Configuración de la relación directa con UserGroups
                entity.HasMany(u => u.GroupUsers)
                    .WithOne(ug => ug.User)
                    .HasForeignKey(ug => ug.UserID);
            });

            // Configuración de Grupo
            modelBuilder.Entity<EFGroup>(entity =>
            {
                entity.ToTable("Groups");
                entity.HasKey(e => e.ID);

                entity.Property(e => e.PublicId)
                    .HasDefaultValueSql("NEWID()")
                    .IsRequired();

                entity.HasIndex(e => e.PublicId)
                    .IsUnique()
                    .HasDatabaseName("UQ_Groups_PublicId");

                entity.Property(e => e.UniqueKey)
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(e => e.Description)
                    .HasMaxLength(300);

                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("GETDATE()");

                entity.Property(e => e.IsActive)
                    .HasDefaultValue(true);

                entity.HasIndex(e => e.UniqueKey)
                    .IsUnique()
                    .HasDatabaseName("UQ_Groups_UniqueKey");

                entity.HasOne(e => e.Creator)
                    .WithMany()
                    .HasForeignKey(e => e.CreatedBy)
                    .HasConstraintName("FK_Groups_CreatedBy");

                // Relaciones
                entity.HasOne(e => e.Creator)
                    .WithMany()
                    .HasForeignKey(e => e.CreatedBy)
                    .HasConstraintName("FK_Groups_CreatedBy");

                // Usuarios que tienen este grupo como primario
                entity.HasMany(g => g.PrimaryGroupUsers)
                    .WithOne(u => u.PrimaryGroup)
                    .HasForeignKey(u => u.PrimaryGroupID);

                // Relación con la tabla intermedia
                entity.HasMany(g => g.GroupUsers)
                    .WithOne(gu => gu.Group)
                    .HasForeignKey(gu => gu.GroupID);
            });

            // Configuración de Usuario-Grupo
            modelBuilder.Entity<EfGroupUser>(entity =>
            {
                entity.ToTable("GroupUser");
                entity.HasKey(e => new { e.UserID, e.GroupID });

                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("GETDATE()");

                entity.Property(e => e.IsActive)
                    .HasDefaultValue(true);

                entity.Property(e => e.CreatedBy)
                    .IsRequired();

                entity.HasOne(e => e.User)
                    .WithMany(u => u.GroupUsers)
                    .HasForeignKey(e => e.UserID)
                    .HasConstraintName("FK_GroupUser_UserID");

                entity.HasOne(e => e.Group)
                    .WithMany(g => g.GroupUsers)
                    .HasForeignKey(e => e.GroupID)
                    .HasConstraintName("FK_GroupUser_GroupID");

                entity.HasOne(e => e.Creator)
                    .WithMany()
                    .HasForeignKey(e => e.CreatedBy)
                    .HasConstraintName("FK_GroupUser_CreatedBy");
            });
        }
    }
}
