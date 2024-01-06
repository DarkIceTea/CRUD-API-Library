using CRUDBooks.Models;

namespace CRUDBooks
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var app = builder.Build();

            //app.MapGet("/", () => "Hello World!");
            app.Run(GetAllBooks);
            

            app.Run();
        }
        public static async Task GetAllBooks(HttpContext context)
        {
            List<Book> books = new List<Book>();
            context.Response.WriteAsJsonAsync(books);
        }
    }
}