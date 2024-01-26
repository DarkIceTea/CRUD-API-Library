using CRUDBooks.Data;
using CRUDBooks.Models;
using Microsoft.EntityFrameworkCore;
using Mapster;
using Azure.Core;
using System.Threading;

namespace CRUDBooks.Repositiries
{
    public class BookRepository : IBookQueryRepository, IBookCommandRepository
    {
        private readonly DataContext dataContext;

        public BookRepository(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public async Task<List<Book>> GetAllBooksAsync(CancellationToken cancellationToken)
        {
            return await dataContext.Books.ToListAsync(cancellationToken);
        }

        public async Task<Book> GetBookByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await dataContext.Books.FindAsync(id, cancellationToken);
        }

        public async Task<Book> GetBookByISBNAsync(string isbn, CancellationToken cancellationToken)
        {
            return await dataContext.Books.FirstOrDefaultAsync(b => b.ISBN == isbn, cancellationToken);
        }

        public async Task AddBookAsync(Book book, CancellationToken cancellationToken)
        {
            book = book.Adapt<Book>();

            book.Id = 0; //Id назначается сам
            dataContext.Books.Add(book);
            await dataContext.SaveChangesAsync(true, cancellationToken);
        }

        public async Task EditBookAsync(Book book, CancellationToken cancellationToken)
        {
            dataContext.Books.Update(book);
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
