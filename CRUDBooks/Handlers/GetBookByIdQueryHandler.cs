using CRUDBooks.Data;
using CRUDBooks.Models;
using CRUDBooks.Queries;
using MediatR;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace CRUDBooks.Handlers
{
    public class GetBookByIdQueryHandler : IRequestHandler<GetBookByIdQuery,  Book>
    {
        private readonly DataContext _dataContext;

        public GetBookByIdQueryHandler(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        async Task<Book> IRequestHandler<GetBookByIdQuery, Book>.Handle(GetBookByIdQuery request, CancellationToken cancellationToken)
        {
            return await _dataContext.Books.FindAsync(request.BookId,cancellationToken);
        }
    }

}
