using CRUDBooks.Data;
using CRUDBooks.Models;
using CRUDBooks.Queries;

namespace CRUDBooks.Handlers
{
    public class GetBookByISBNQueryHandler : IQueryHandler<GetBookByISBNQuery, Book>
    {
        private readonly DataContext _dataContext;

        public GetBookByISBNQueryHandler(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public Book Execute(GetBookByISBNQuery query)
        {
            return _dataContext.Books.FirstOrDefault(b => b.ISBN == query.ISBN);
        }
    }
}
