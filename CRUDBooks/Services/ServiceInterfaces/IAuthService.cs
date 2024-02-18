using CRUDBooks.Models;

namespace CRUDBooks.Services.ServiceInterfaces
{
    public interface IAuthService
    {
        public bool VerifyUser(User userFromClient);
    }
}
