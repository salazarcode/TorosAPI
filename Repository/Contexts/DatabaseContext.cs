
using Microsoft.EntityFrameworkCore;
using Repository.Models;

namespace Repository.Contexts
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Identifier> Identifiers { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<IdentifierGroup> IdentifierGroups { get; set; }

        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Identifier>(entity =>
            {
                entity.ToTable("Identifiers");
                entity.HasKey(e => e.ID);

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
                    .HasDatabaseName("UQ_Identifiers_Username");

                entity.HasIndex(e => e.Email)
                    .IsUnique()
                    .HasDatabaseName("UQ_Identifiers_Email");

                entity.HasOne(e => e.PrimaryGroup)
                    .WithMany(g => g.PrimaryGroupIdentifiers)
                    .HasForeignKey(e => e.PrimaryGroupID)
                    .HasConstraintName("FK_Identifiers_PrimaryGroupID");

                entity.HasOne(e => e.Creator)
                    .WithMany(i => i.CreatedIdentifiers)
                    .HasForeignKey(e => e.CreatedBy)
                    .HasConstraintName("FK_Identifiers_CreatedBy");

                // Username format check constraint
                entity.ToTable(tb => tb.HasCheckConstraint(
                    "CK_Identifiers_Username_Format",
                    "dbo.IsValidUsername(Username) = 1"));
            });

            modelBuilder.Entity<Group>(entity =>
            {
                entity.ToTable("Groups");
                entity.HasKey(e => e.ID);

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
                    .WithMany(i => i.CreatedGroups)
                    .HasForeignKey(e => e.CreatedBy)
                    .HasConstraintName("FK_Groups_CreatedBy");

                // UniqueKey format check constraint
                entity.ToTable(tb => tb.HasCheckConstraint(
                    "CK_Groups_UniqueKey_Format",
                    "dbo.IsKebabCase(UniqueKey) = 1"));
            });

            modelBuilder.Entity<IdentifierGroup>(entity =>
            {
                entity.ToTable("IdentifierGroups");
                entity.HasKey(e => new { e.IdentifierID, e.GroupID });

                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("GETDATE()");

                entity.Property(e => e.IsActive)
                    .HasDefaultValue(true);

                entity.HasOne(e => e.Identifier)
                    .WithMany(i => i.IdentifierGroups)
                    .HasForeignKey(e => e.IdentifierID)
                    .HasConstraintName("FK_IdentifierGroup_IdentifierID");

                entity.HasOne(e => e.Group)
                    .WithMany(g => g.IdentifierGroups)
                    .HasForeignKey(e => e.GroupID)
                    .HasConstraintName("FK_IdentifierGroup_GroupID");

                entity.HasOne(e => e.Creator)
                    .WithMany()
                    .HasForeignKey(e => e.CreatedBy)
                    .OnDelete(DeleteBehavior.NoAction)
                    .HasConstraintName("FK_IdentifierGroup_CreatedBy");
            });
        }
    }
}
