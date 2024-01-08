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
            // �������� ������ ����������� �� ����� ������������
            string connection = builder.Configuration.GetConnectionString("DefaultConnection");

            // ��������� �������� ApplicationContext � �������� ������� � ����������
            builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer(connection));

            var app = builder.Build();

            // ��������� ���������� ��� ������ � �������
            app.MapGet("/books", BookController.GetAllBooks); // ��������� ������� �� ��������� ���� ����
            app.MapGet("book/{id}", BookController.GetBookById); // ��������� ������� �� ��������� ����� �� Id
            //app.MapPost("", CreateBook); // ��������� ������� �� �������� ����� �����
            //app.MapPut("/{id}", UpdateBook); // ��������� ������� �� ���������� ����� �� Id
            //app.MapDelete("/{id}", DeleteBook); // ��������� ������� �� �������� ����� �� Id



            app.Run();
        }

        public static async Task GetBook(HttpContext context, Book book)
        {
            context.Response.WriteAsJsonAsync(book);
        }
    }
}