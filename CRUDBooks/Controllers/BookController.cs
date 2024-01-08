using CRUDBooks.Data;
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
    }
}
