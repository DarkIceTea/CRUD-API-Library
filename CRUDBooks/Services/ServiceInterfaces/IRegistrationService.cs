using CRUDBooks.Models;

namespace CRUDBooks.Services.ServiceInterfaces
{
    public interface IRegistrationService
    {
        public bool RegisterUser(User user);
    }
}
