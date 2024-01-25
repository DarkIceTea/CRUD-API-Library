using CRUDBooks.Models;
using MediatR;

namespace CRUDBooks.Queries
{
    public class GetBookByIdQuery : IRequest<Book>
    {
        public int BookId { get; set; }
    }
}
