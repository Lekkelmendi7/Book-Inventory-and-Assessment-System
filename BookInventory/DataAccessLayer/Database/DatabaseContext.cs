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

        }


        public DbSet<Book> Books { get; set;}
        public DbSet<Author> Authors { get; set; }
        public DbSet<Publisher> Publishers { get; set; }

        
    }
}
