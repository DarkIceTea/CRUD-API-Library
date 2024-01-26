using CRUDBooks.Models;

namespace CRUDBooks.Repositiries
{
    public interface IBookCommandRepository
    {
        Task AddBookAsync(Book book, CancellationToken cancellationToken);
        Task EditBookAsync(Book book, CancellationToken cancellationToken);
        Task DeleteBookAsync(int id, CancellationToken cancellationToken);
    }
}
