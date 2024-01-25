using CRUDBooks.Models;

namespace CRUDBooks.Services
{
    public interface IAuthService
    {
        public bool VerifyUser(User userFromClient);
    }
}
