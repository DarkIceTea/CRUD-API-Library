using Microsoft.EntityFrameworkCore;
using CRUDBooks.Models;

namespace CRUDBooks.Data
{
    public class DataContext : DbContext
    {
        public DbSet<Book> Books { get; set; } = null!;
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>().HasData(
                    new Book { Id = 1, Title = "Crime and panishment", Author = "Dostoevsky", Description = "about roskolnikov", Genre = "Drama", ISBN = "111", WhenTake = DateTime.Now, WhenReturn = DateTime.Now.AddMonths(1) }
            );
        }
    }
}
