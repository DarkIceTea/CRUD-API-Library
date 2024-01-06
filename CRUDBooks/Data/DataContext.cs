using Microsoft.EntityFrameworkCore;
using CRUDBooks.Models;

namespace CRUDBooks.Data
{
    public class DataContext : DbContext
    {
        public DbSet<Book> Books { get; set; } = null!;
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
