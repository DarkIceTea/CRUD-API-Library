using CRUDBooks.Models;
using MediatR;

namespace CRUDBooks.Queries
{
    public class GetBookByISBNQuery : IRequest<Book>
    {
        public string ISBN { get; set; }
    }
}
