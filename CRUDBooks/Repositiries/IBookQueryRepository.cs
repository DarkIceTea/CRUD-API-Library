using CRUDBooks.Models;

namespace CRUDBooks.Repositiries
{
    public interface IBookQueryRepository
    {
        Task<List<Book>> GetAllBooksAsync(CancellationToken cancellationToken);
        Task<Book> GetBookByIdAsync(int id, CancellationToken cancellationToken);
        Task<Book> GetBookByISBNAsync(string isbn, CancellationToken cancellationToken);
    }
}
