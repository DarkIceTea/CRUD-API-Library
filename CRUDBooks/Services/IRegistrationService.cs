using CRUDBooks.Models;

namespace CRUDBooks.Services
{
    public interface IRegistrationService
    {
        public bool RegisterUser(User user);
    }
}
