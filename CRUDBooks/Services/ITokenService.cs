namespace CRUDBooks.Services
{
    public interface ITokenService
    {
        public string GenerateToken(string username);
    }
}
