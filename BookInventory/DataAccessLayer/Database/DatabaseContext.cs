using BookInventory.DataAccess.Entities;
using BookInventory.DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookInventory.DataAccess.Database
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Author>()
                .HasMany(a => a.Books)
                .WithOne(a => a.Author)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Author>()
                .HasOne(e => e.Publisher)
                .WithOne(e => e.Author)
                .HasForeignKey<Publisher>(e => e.AuthorId)
                .IsRequired();

            modelBuilder.Entity<RolePermission>()
                .HasKey(x => new { x.RoleId, x.PermissionId });

            modelBuilder.Entity<RoleUser>()
                .HasKey(x => new { x.UserId, x.RoleId });

            modelBuilder.Entity<Role>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<User>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<Permission>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<RoleUser>()
                .HasOne(_ => _.User)
                .WithMany(_ => _.RoleUsers)
                .HasForeignKey(_ => _.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<RoleUser>()
                .HasOne(_ => _.Role)
                .WithMany(_ => _.RoleUsers)
                .HasForeignKey(_ => _.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<RolePermission>()
                .HasOne(_ => _.Permission)
                .WithMany(_ => _.RolePermissions)
                .HasForeignKey(_ => _.PermissionId);

            modelBuilder.Entity<RolePermission>()
                .HasOne(_ => _.Role)
                .WithMany(_ => _.RolePermissions)
                .HasForeignKey(_ => _.RoleId);

        }


        public DbSet<Book> Books { get; set;}
        public DbSet<Author> Authors { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<RoleUser> RoleUsers { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<User> Users { get; set; }


    }
}
