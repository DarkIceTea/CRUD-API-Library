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
            app.MapGet("book/ISBN/{isbn}", BookController.GetBookByISBN); // ��������� ������� �� ��������� ����� �� ISBN
            app.MapPost("book/add", BookController.AddBook); // ��������� ������� �� �������� ����� �����
            app.MapPut("book/{id}", BookController.EditBook); // ��������� ������� �� ���������� ����� �� Id
            app.MapDelete("book/{id}", BookController.DeleteBook); // ��������� ������� �� �������� ����� �� Id

            app.Run();
        }
    }
}