using CRUDBooks.Data;
using CRUDBooks.Models;
using CRUDBooks.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace CRUDBooks.Handlers
{
    public class GetAllBooksQueryHandler : IRequestHandler<GetAllBooksQuery, List<Book>>
    {
        private readonly DataContext _dataContext;
        public GetAllBooksQueryHandler(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        async Task<List<Book>> IRequestHandler<GetAllBooksQuery, List<Book>>.Handle(GetAllBooksQuery request, CancellationToken cancellationToken)
        {
            return await _dataContext.Books.ToListAsync(cancellationToken);
        }
    }
}
