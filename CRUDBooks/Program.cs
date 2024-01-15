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
using CRUDBooks.Handlers;
using CRUDBooks.Queries;
using CRUDBooks.Commands;

namespace CRUDBooks
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            // �������� ������ ����������� �� ����� ������������
            string connection = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddAuthorization();
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    // ���������, ����� �� �������������� �������� ��� ��������� ������
                    ValidateIssuer = true,
                    // ������, �������������� ��������
                    ValidIssuer = AuthOptions.ISSUER,
                    // ����� �� �������������� ����������� ������
                    ValidateAudience = true,
                    // ��������� ����������� ������
                    ValidAudience = AuthOptions.AUDIENCE,
                    // ����� �� �������������� ����� �������������
                    ValidateLifetime = true,
                    // ��������� ����� ������������
                    IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
                    // ��������� ����� ������������
                    ValidateIssuerSigningKey = true,
                };
            });    // ����������� �������������� � ������� jwt-�������
            builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer(connection), ServiceLifetime.Singleton);    // ��������� �������� ApplicationContext � �������� ������� � ����������
            builder.Services.AddControllers();
            builder.Services.AddHttpContextAccessor();

            //����������� ������������ ��������
            builder.Services.AddTransient<IQueryHandler<GetAllBooksQuery, List<Book>>, GetAllBooksQueryHandler>();
            builder.Services.AddTransient<IQueryHandler<GetBookByIdQuery, Book>, GetBookByIdQueryHandler>();
            builder.Services.AddTransient<IQueryHandler<GetBookByISBNQuery, Book>, GetBookByISBNQueryHandler>();
            builder.Services.AddSingleton<IQueryDispatcher, QueryDispatcher>();

            // ����������� ������������ ������
            builder.Services.AddTransient<ICommandHandler<AddBookCommand>, AddBookCommandHandler>();
            builder.Services.AddTransient<ICommandHandler<EditBookCommand>, EditBookCommandHandler>();
            builder.Services.AddTransient<ICommandHandler<DeleteBookCommand>, DeleteBookCommandHandler>();
            builder.Services.AddSingleton<ICommandDispatcher, CommandDispatcher>();

            var app = builder.Build();

            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => endpoints.MapControllers());
            

            //app.MapGet("/books", BookController.GetAllBooks); // ��������� ������� �� ��������� ���� ����
            //app.MapGet("book/{id}", BookController.GetBookById); // ��������� ������� �� ��������� ����� �� Id
            //app.MapGet("book/ISBN/{isbn}", BookController.GetBookByISBN); // ��������� ������� �� ��������� ����� �� ISBN
            //app.MapPost("book/add", BookController.AddBook); // ��������� ������� �� �������� ����� �����
            //app.MapPut("book/{id}", BookController.EditBook); // ��������� ������� �� ���������� ����� �� Id
            //app.MapDelete("book/{id}", BookController.DeleteBook); // ��������� ������� �� �������� ����� �� Id
            //app.MapPost("/login", AccountController.LoginAuthentication);
            //app.MapPost("/registration", AccountController.Regestration);

            app.Run();
        }
    }
}