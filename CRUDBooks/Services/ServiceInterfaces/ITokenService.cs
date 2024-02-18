namespace CRUDBooks.Services.ServiceInterfaces
{
    public interface ITokenService
    {
        public string GenerateToken(string username);
    }
}
