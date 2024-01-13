using CRUDBooks.Models;
using CRUDBooks.Properties;
using Microsoft.Extensions.ObjectPool;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CRUDBooks.Controllers
{
    public class AccountController
    {
        public static async Task LoginAuthentication(HttpContext context)
        {
            User user = new User() { Id = 1, Login = "Ivan", Password = "123" };
            //user = await context.Request.ReadFromJsonAsync<User>();

            var claims = new List<Claim> { new Claim(ClaimTypes.Name, user.Login) };
            JwtSecurityToken jwt = new JwtSecurityToken(
            issuer: AuthOptions.ISSUER,
            audience: AuthOptions.AUDIENCE,
            claims: claims,
            expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(2)), // время действия 2 минуты
            signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

            await context.Response.WriteAsync(new JwtSecurityTokenHandler().WriteToken(jwt));
        }
    }
}
