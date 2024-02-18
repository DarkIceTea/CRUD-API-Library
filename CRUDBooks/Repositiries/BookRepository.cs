using CRUDBooks.Data;
using CRUDBooks.Models;
using Microsoft.EntityFrameworkCore;
using CRUDBooks.Exceptions;

namespace CRUDBooks.Repositiries
{
    public class BookRepository : IBookRepository
    {
        private readonly DataContext dataContext;

        public BookRepository(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public async Task<List<Book>> GetAllBooksAsync(CancellationToken cancellationToken)
        {
            var books = await dataContext.Books
                .Include(b => b.Author)
                .Include(b => b.Genre)
                .ToListAsync(cancellationToken);

            return books;
        }

        public async Task<Book> GetBookByIdAsync(int id, CancellationToken cancellationToken)
        {
            var book = await dataContext.Books
                .Include(b => b.Author)
                .Include(b => b.Genre)
                .FirstOrDefaultAsync(b => b.Id == id, cancellationToken);
            if(book is null)
            {
                throw new NotFoundException("Book not found");
            }

            return book;
        }

        public async Task<Book> GetBookByISBNAsync(string isbn, CancellationToken cancellationToken)
        {
            var book = await dataContext.Books
                .Include(b => b.Author)
                .Include(b => b.Genre)
                .FirstOrDefaultAsync(b => b.ISBN == isbn, cancellationToken);

            if (book is null)
            {
                throw new NotFoundException("Book not found");
            }

            return book;
        }

        public async Task AddBookAsync(Book book, CancellationToken cancellationToken)
        {
            dataContext.Books.Add(book);
            await dataContext.SaveChangesAsync(true, cancellationToken);
        }

        public async Task EditBookAsync(Book book, int id, CancellationToken cancellationToken)
        {
            Book existingBook = dataContext.Books.Find(id);
            if (book is null)
            {
                throw new NotFoundException("Book not found");
            }

            dataContext.Entry(existingBook).CurrentValues.SetValues(book);

            await dataContext.SaveChangesAsync(true, cancellationToken);
        }

        public async Task DeleteBookAsync(int id, CancellationToken cancellationToken)
        {
            Book book = dataContext.Books.Find(id);
            if (book is null)
            {
                throw new NotFoundException("Book not found");
            }

            dataContext.Books.Remove(book);
            await dataContext.SaveChangesAsync(true, cancellationToken);
        }
    }
}
