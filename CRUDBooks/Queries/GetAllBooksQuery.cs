using CRUDBooks.Models;
using MediatR;

namespace CRUDBooks.Queries
{
    public class GetAllBooksQuery : IRequest<List<Book>>
    {
    }
}
