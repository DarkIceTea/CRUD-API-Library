using CRUDBooks.Models;
using Microsoft.EntityFrameworkCore;
using CRUDBooks.Data;
using CRUDBooks.Controllers;
using Azure.Core;
using Azure;
using System.Text.RegularExpressions;
using Microsoft.Extensions.FileSystemGlobbing.Internal;

namespace CRUDBooks
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            // получаем строку подключения из файла конфигурации
            string connection = builder.Configuration.GetConnectionString("DefaultConnection");

            // добавляем контекст ApplicationContext в качестве сервиса в приложение
            builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer(connection));

            var app = builder.Build();

            // Добавляем контроллер для работы с книгами
            app.MapGet("/books", BookController.GetAllBooks); // Обработка запроса на получение всех книг
            app.MapGet("book/{id}", BookController.GetBookById); // Обработка запроса на получение книги по Id
            //app.MapPost("", CreateBook); // Обработка запроса на создание новой книги
            //app.MapPut("/{id}", UpdateBook); // Обработка запроса на обновление книги по Id
            //app.MapDelete("/{id}", DeleteBook); // Обработка запроса на удаление книги по Id



            app.Run();
        }

        public static async Task GetBook(HttpContext context, Book book)
        {
            context.Response.WriteAsJsonAsync(book);
        }
    }
}