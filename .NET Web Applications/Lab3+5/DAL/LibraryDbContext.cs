using Microsoft.EntityFrameworkCore;
using DAL.Model;

namespace DAL
{
    public class LibraryDbContext : DbContext
    {
        public LibraryDbContext(DbContextOptions<LibraryDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<UserBook> UserBooks { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Genre> Genres { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // make name fileds unique
            modelBuilder.Entity<Genre>()
                .HasIndex(e => e.Name)
                .IsUnique();

            modelBuilder.Entity<Author>()
                .HasIndex(e => e.Name)
                .IsUnique();
        }
    }
}