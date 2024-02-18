using CRUDBooks.Data;
using CRUDBooks.Models;
using CRUDBooks.Services.ServiceInterfaces;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace CRUDBooks.Services
{
    public class AuthService : IAuthService
    {
        DataContext dataContext;
        public AuthService(DataContext context)
        {
            dataContext = context;
        }
        public bool VerifyUser(User userFromClient)
        {
            User userFromDb = dataContext.Users.FirstOrDefault(u => u.Login == userFromClient.Login);
            if (userFromDb == null)
            {
                return false;
            }
            if (!(VerifyPassword(userFromClient.Password, userFromDb.Password)))
            {
                return false;
            }
            return true;
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
