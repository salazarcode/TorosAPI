using Microsoft.EntityFrameworkCore;
using Repository.Models;

namespace Repository
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }
        public DbSet<XClass> XClasses { get; set; }
        public DbSet<XProperty> XProperties { get; set; }
        public DbSet<XAncestry> XAncestries { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<XProperty>()
                   .HasOne(p => p.XClass)
                   .WithMany(c => c.XProperties)
                   .HasForeignKey(p => p.ClassID);

            modelBuilder.Entity<XProperty>()
                   .HasOne(p => p.PropertyClass)
                   .WithMany(c => c.XOthersProperties)
                   .HasForeignKey(p => p.PropertyClassID);

            modelBuilder.Entity<XAncestry>()
                .HasOne(a => a.XClass)
                .WithMany(c => c.XAncestries)
                .HasForeignKey(a => a.XClassID);

            modelBuilder.Entity<XAncestry>()
                .HasKey(x => new { x.XClassID, x.ParentID });
        }
    }
}
