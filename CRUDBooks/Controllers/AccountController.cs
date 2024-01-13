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

namespace CRUDBooks.Controllers
{
    public class AccountController
    {
        public static async Task LoginAuthentication(HttpContext context, DataContext db)
        {
            User userFromClient = new User() { Id = 1, Login = "Konstantin", Password = "12345" };
            //user = await context.Request.ReadFromJsonAsync<User>();
            User userFromDb = db.Users.FirstOrDefault(u =>  u.Login == userFromClient.Login);
            if (userFromDb == null)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }
            if(!(VerifyPassword(userFromClient.Password, HashPassword(userFromDb.Password))))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }
            
            var claims = new List<Claim> { new Claim(ClaimTypes.Name, userFromClient.Login) };
            JwtSecurityToken jwt = new JwtSecurityToken(
            issuer: AuthOptions.ISSUER,
            audience: AuthOptions.AUDIENCE,
            claims: claims,
            expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(2)), // время действия 2 минуты
            signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

            await context.Response.WriteAsync(new JwtSecurityTokenHandler().WriteToken(jwt));
        }

        private static string HashPassword(string password)
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

        private static bool VerifyPassword(string enteredPassword, string hashedPassword)
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
