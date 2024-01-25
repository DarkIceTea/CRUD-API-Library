using CRUDBooks.Data;
using CRUDBooks.Models;
using CRUDBooks.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace CRUDBooks.Handlers
{
    public class GetBookByISBNQueryHandler : IRequestHandler<GetBookByISBNQuery, Book>
    {
        private readonly DataContext _dataContext;

        public GetBookByISBNQueryHandler(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        async Task<Book> IRequestHandler<GetBookByISBNQuery, Book>.Handle(GetBookByISBNQuery request, CancellationToken cancellationToken)
        {
            return await _dataContext.Books.FirstOrDefaultAsync(b => b.ISBN == request.ISBN);
        }
    }
}
