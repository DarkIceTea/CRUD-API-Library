using CRUDBooks.Data;
using CRUDBooks.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace CRUDBooks.Services
{
    public class RegistrationService : IRegistrationService
    {
        private readonly DataContext _dataContext;

        public RegistrationService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public bool RegisterUser(User user)
        {
            if (_dataContext.Users.FirstOrDefault(u => u.Login == user.Login) != null)
            {
                return false;
            }

            user.Id = 0;
            user.Password = HashPassword(user.Password);
            _dataContext.Add(user);
            _dataContext.SaveChanges();

            return true;
        }

        private string HashPassword(string password)
        {
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: new byte[0],
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            return hashed;
        }
    }
}
