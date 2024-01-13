using CRUDBooks.Models;
using Microsoft.EntityFrameworkCore;
using CRUDBooks.Data;
using CRUDBooks.Controllers;
using System.Text.RegularExpressions;
using Microsoft.Extensions.FileSystemGlobbing.Internal;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using CRUDBooks.Properties;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CRUDBooks
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            // получаем строку подключения из файла конфигурации
            string connection = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddAuthorization();
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    // указывает, будет ли валидироваться издатель при валидации токена
                    ValidateIssuer = true,
                    // строка, представляющая издателя
                    ValidIssuer = AuthOptions.ISSUER,
                    // будет ли валидироваться потребитель токена
                    ValidateAudience = true,
                    // установка потребителя токена
                    ValidAudience = AuthOptions.AUDIENCE,
                    // будет ли валидироваться время существования
                    ValidateLifetime = true,
                    // установка ключа безопасности
                    IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
                    // валидация ключа безопасности
                    ValidateIssuerSigningKey = true,
                };
            });    // подключение аутентификации с помощью jwt-токенов
            builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer(connection));    // добавляем контекст ApplicationContext в качестве сервиса в приложение

            var app = builder.Build();

            app.UseAuthentication();
            app.UseAuthorization();
            app.MapGet("/books", BookController.GetAllBooks); // Обработка запроса на получение всех книг
            app.MapGet("book/{id}", BookController.GetBookById); // Обработка запроса на получение книги по Id
            app.MapGet("book/ISBN/{isbn}", BookController.GetBookByISBN); // Обработка запроса на получение книги по ISBN
            app.MapPost("book/add", BookController.AddBook); // Обработка запроса на создание новой книги
            app.MapPut("book/{id}", BookController.EditBook); // Обработка запроса на обновление книги по Id
            app.MapDelete("book/{id}", BookController.DeleteBook); // Обработка запроса на удаление книги по Id
            app.MapPost("/login", AccountController.LoginAuthentication);
            app.MapPost("/registration", AccountController.Regestration);

            app.Run();
        }
    }
}