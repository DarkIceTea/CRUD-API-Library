using CRUDBooks.Models;

namespace CRUDBooks.Queries
{
    public class GetBookByIdQuery : IQuery<Book>
    {
        public int BookId { get; set; }
    }
}
