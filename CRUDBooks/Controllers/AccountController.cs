using CRUDBooks.Data;
using CRUDBooks.Models;
using CRUDBooks.Properties;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using Microsoft.Extensions.ObjectPool;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace CRUDBooks.Controllers
{
    [ApiController]
    public class AccountController : Controller
    {
        private readonly HttpContext httpContext;
        private readonly DataContext dataContext;

        public AccountController(IHttpContextAccessor httpContext, DataContext dataContext)
        {
            this.httpContext = httpContext.HttpContext;
            this.dataContext = dataContext;
        }
        [HttpGet("/registration")]
        public async Task Regestration()
        {
            User user = await httpContext.Request.ReadFromJsonAsync<User>();
            if (user == null)
            {
                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;  //Не удалось десериализовать объект User
                return;
            }   
            if(dataContext.Users.FirstOrDefault(u => u.Login == user.Login) is not null)
            {
                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;  //Пользователь уже существует
                return;
            }
            user.Password = HashPassword(user.Password);
            dataContext.Add(user);
            dataContext.SaveChanges();
        }

        [HttpGet("/login")]
        public async Task LoginAuthentication()
        {
            //User userFromClient = new User() { Id = 1, Login = "Konstantin", Password = "12345" };

            User userFromClient = await httpContext.Request.ReadFromJsonAsync<User>();
            User userFromDb = dataContext.Users.FirstOrDefault(u =>  u.Login == userFromClient.Login);
            if (userFromDb == null)
            {
                httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }
            if(!(VerifyPassword(userFromClient.Password, userFromDb.Password)))
            {
                httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }
            
            var claims = new List<Claim> { new Claim(ClaimTypes.Name, userFromClient.Login) };
            JwtSecurityToken jwt = new JwtSecurityToken(
            issuer: AuthOptions.ISSUER,
            audience: AuthOptions.AUDIENCE,
            claims: claims,
            expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(2)), // время действия 2 минуты
            signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

            await httpContext.Response.WriteAsync(new JwtSecurityTokenHandler().WriteToken(jwt));
        }

        private string HashPassword(string password)
        {
            // Хеширование пароля без использования соли
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: new byte[0], // Пустая соль
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            return hashed;
        }

        private bool VerifyPassword(string enteredPassword, string hashedPassword)
        {
            // Проверка введенного пароля с использованием хеша из базы данных
            var actualHash = KeyDerivation.Pbkdf2(
                password: enteredPassword,
                salt: new byte[0], // Пустая соль
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8); 

            // Сравнение хешей
            return actualHash.SequenceEqual(Convert.FromBase64String(hashedPassword));
        }
    }
}
