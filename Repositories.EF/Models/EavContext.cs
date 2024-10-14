using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repositories.EF.Models;

public partial class EavContext : DbContext
{
    public EavContext()
    {
    }

    public EavContext(DbContextOptions<EavContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Class> Classes { get; set; }

    public virtual DbSet<DeletionType> DeletionTypes { get; set; }

    public virtual DbSet<Object> Objects { get; set; }

    public virtual DbSet<Property> Properties { get; set; }

    public virtual DbSet<RelationDetail> RelationDetails { get; set; }

    public virtual DbSet<StringValue> StringValues { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=(local);Database=eav;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Class>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Types__3214EC27154A2D2D");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Key).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(100);

            entity.HasMany(d => d.Classes).WithMany(p => p.Parents)
                .UsingEntity<Dictionary<string, object>>(
                    "Ancestry",
                    r => r.HasOne<Class>().WithMany()
                        .HasForeignKey("ClassId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Ancestrie__TypeI__7CD98669"),
                    l => l.HasOne<Class>().WithMany()
                        .HasForeignKey("ParentId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Ancestrie__Paren__7DCDAAA2"),
                    j =>
                    {
                        j.HasKey("ClassId", "ParentId").HasName("PK__Ancestri__CF8260B6E108D3A6");
                        j.ToTable("Ancestries");
                        j.IndexerProperty<int>("ClassId").HasColumnName("ClassID");
                        j.IndexerProperty<int>("ParentId").HasColumnName("ParentID");
                    });

            entity.HasMany(d => d.Parents).WithMany(p => p.Classes)
                .UsingEntity<Dictionary<string, object>>(
                    "Ancestry",
                    r => r.HasOne<Class>().WithMany()
                        .HasForeignKey("ParentId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Ancestrie__Paren__7DCDAAA2"),
                    l => l.HasOne<Class>().WithMany()
                        .HasForeignKey("ClassId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Ancestrie__TypeI__7CD98669"),
                    j =>
                    {
                        j.HasKey("ClassId", "ParentId").HasName("PK__Ancestri__CF8260B6E108D3A6");
                        j.ToTable("Ancestries");
                        j.IndexerProperty<int>("ClassId").HasColumnName("ClassID");
                        j.IndexerProperty<int>("ParentId").HasColumnName("ParentID");
                    });
        });

        modelBuilder.Entity<DeletionType>(entity =>
        {
            entity.HasKey(e => e.Name).HasName("PK__Deletion__737584F761214E1A");

            entity.Property(e => e.Name).HasMaxLength(20);
        });

        modelBuilder.Entity<Object>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Entities__3214EC2769EAA14C");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ClassId).HasColumnName("ClassID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Class).WithMany(p => p.Objects)
                .HasForeignKey(d => d.ClassId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Entities__TypeID__0A338187");
        });

        modelBuilder.Entity<Property>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Relation__3214EC27248D1913");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ClassId).HasColumnName("ClassID");
            entity.Property(e => e.Key).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.PropertyClassId).HasColumnName("PropertyClassID");

            entity.HasOne(d => d.Class).WithMany(p => p.PropertyClasses)
                .HasForeignKey(d => d.ClassId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Relations__Origi__019E3B86");

            entity.HasOne(d => d.PropertyClass).WithMany(p => p.PropertyPropertyClasses)
                .HasForeignKey(d => d.PropertyClassId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Relations__Desti__02925FBF");
        });

        modelBuilder.Entity<RelationDetail>(entity =>
        {
            entity.HasKey(e => e.PropertyId).HasName("PK__Relation__70C9A755AD2F208E");

            entity.Property(e => e.PropertyId)
                .ValueGeneratedNever()
                .HasColumnName("PropertyID");
            entity.Property(e => e.OnDelete).HasMaxLength(20);

            entity.HasOne(d => d.OnDeleteNavigation).WithMany(p => p.RelationDetails)
                .HasForeignKey(d => d.OnDelete)
                .HasConstraintName("FK__RelationD__OnDel__0539C240");

            entity.HasOne(d => d.Property).WithOne(p => p.RelationDetail)
                .HasForeignKey<RelationDetail>(d => d.PropertyId)
                .HasConstraintName("FK__RelationD__OnDel__04459E07");
        });

        modelBuilder.Entity<StringValue>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__StringVa__3214EC2753E7643C");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ObjectId).HasColumnName("ObjectID");
            entity.Property(e => e.PropertyId).HasColumnName("PropertyID");
            entity.Property(e => e.Value).HasMaxLength(255);

            entity.HasOne(d => d.Object).WithMany(p => p.StringValues)
                .HasForeignKey(d => d.ObjectId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__StringVal__Entit__1F2E9E6D");

            entity.HasOne(d => d.Property).WithMany(p => p.StringValues)
                .HasForeignKey(d => d.PropertyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_StringValues_Attribute");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
