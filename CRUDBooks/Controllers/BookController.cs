using CRUDBooks.Data;
using CRUDBooks.Models;
using Microsoft.EntityFrameworkCore;

namespace CRUDBooks.Controllers
{
    public class BookController
    {
        private readonly DataContext db;
        public BookController(DataContext context)
        {
            db = context;
        }

        // Пример метода контроллера для получения всех книг
        public static async Task GetAllBooks(HttpContext context, DataContext dbContext)
        {
            var books = await dbContext.Books.ToListAsync();
            await context.Response.WriteAsJsonAsync(books);
        }

        public static async Task GetBookById(int id, HttpContext context, DataContext dbContext)
        {
            Book book = await dbContext.Books.FindAsync(id);
            if (book == null)
            {
                context.Response.StatusCode = StatusCodes.Status404NotFound;
            }
            else
            {
                await context.Response.WriteAsJsonAsync(book);
            }
        }
        public static async Task GetBookByISBN(string isbn, HttpContext context, DataContext dbContext)
        {
            Book book = await dbContext.Books.FirstAsync(b => b.ISBN == isbn);
            if (book == null)
            {
                context.Response.StatusCode = StatusCodes.Status404NotFound;
            }
            else
            {
                await context.Response.WriteAsJsonAsync(book);
            }
        }
    }
}
