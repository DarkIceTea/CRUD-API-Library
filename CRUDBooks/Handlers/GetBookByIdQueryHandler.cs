using CRUDBooks.Data;
using CRUDBooks.Models;
using CRUDBooks.Queries;

namespace CRUDBooks.Handlers
{
    public class GetBookByIdQueryHandler : IQueryHandler<GetBookByIdQuery, Book>
    {
        private readonly DataContext _dataContext;

        public GetBookByIdQueryHandler(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public Book Execute(GetBookByIdQuery query)
        {
            return _dataContext.Books.Find(query.BookId);
        }
    }

}
