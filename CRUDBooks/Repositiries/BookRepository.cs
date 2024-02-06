using CRUDBooks.Data;
using CRUDBooks.Models;
using Microsoft.EntityFrameworkCore;

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

            return book;
        }

        public async Task<Book> GetBookByISBNAsync(string isbn, CancellationToken cancellationToken)
        {
            var book = await dataContext.Books
                .Include(b => b.Author)
                .Include(b => b.Genre)
                .FirstOrDefaultAsync(b => b.ISBN == isbn, cancellationToken);

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
            if (existingBook is null) throw new Exception();

            existingBook.Author = book.Author;
            existingBook.Genre = book.Genre;
            existingBook.ISBN = book.ISBN;
            existingBook.Title = book.Title;
            existingBook.Description = book.Description;

            await dataContext.SaveChangesAsync(true, cancellationToken);
        }

        public async Task DeleteBookAsync(int id, CancellationToken cancellationToken)
        {
            Book book = dataContext.Books.Find(id);

            if (book is null)
            {
                throw new Exception("Book Not Found");
            }
            dataContext.Books.Remove(book);
            await dataContext.SaveChangesAsync(true, cancellationToken);
        }
    }
}
