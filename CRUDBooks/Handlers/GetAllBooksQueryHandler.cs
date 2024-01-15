using CRUDBooks.Data;
using CRUDBooks.Models;
using CRUDBooks.Queries;

namespace CRUDBooks.Handlers
{
    public class GetAllBooksQueryHandler : IQueryHandler<GetAllBooksQuery, List<Book>>
    {
        private readonly DataContext _dataContext;
        public GetAllBooksQueryHandler(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public List<Book> Execute(GetAllBooksQuery getAllBooksQuery)
        {
            return _dataContext.Books.ToList();
        }
    }
}
