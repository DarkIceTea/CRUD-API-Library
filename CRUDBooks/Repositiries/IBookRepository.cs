using CRUDBooks.Models;

namespace CRUDBooks.Repositiries
{
    public interface IBookRepository
    {
        Task<List<Book>> GetAllBooksAsync(CancellationToken cancellationToken);
        Task<Book> GetBookByIdAsync(int id, CancellationToken cancellationToken);
        Task<Book> GetBookByISBNAsync(string isbn, CancellationToken cancellationToken);

        Task AddBookAsync(Book book, CancellationToken cancellationToken);
        Task EditBookAsync(Book book, int id, CancellationToken cancellationToken);
        Task DeleteBookAsync(int id, CancellationToken cancellationToken);
    }
}
