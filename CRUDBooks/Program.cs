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
            builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer(connection));    // ��������� �������� ApplicationContext � �������� ������� � ����������

            var app = builder.Build();

            app.UseAuthentication();
            app.UseAuthorization();
            app.MapGet("/books", BookController.GetAllBooks); // ��������� ������� �� ��������� ���� ����
            app.MapGet("book/{id}", BookController.GetBookById); // ��������� ������� �� ��������� ����� �� Id
            app.MapGet("book/ISBN/{isbn}", BookController.GetBookByISBN); // ��������� ������� �� ��������� ����� �� ISBN
            app.MapPost("book/add", BookController.AddBook); // ��������� ������� �� �������� ����� �����
            app.MapPut("book/{id}", BookController.EditBook); // ��������� ������� �� ���������� ����� �� Id
            app.MapDelete("book/{id}", BookController.DeleteBook); // ��������� ������� �� �������� ����� �� Id
            app.MapGet("/login", async (HttpContext context) =>
            {
                User user = new User() { Id = 1, Login = "Ivan", Password = "123" };
                //user = await context.Request.ReadFromJsonAsync<User>();

                var claims = new List<Claim> { new Claim(ClaimTypes.Name, user.Login) };
                var jwt = new JwtSecurityToken(
                        issuer: AuthOptions.ISSUER,
                        audience: AuthOptions.AUDIENCE,
                        claims: claims,
                        expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(2)), // ����� �������� 2 ������
                        signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

                return new JwtSecurityTokenHandler().WriteToken(jwt);
            });

            app.Run();
        }
    }
}