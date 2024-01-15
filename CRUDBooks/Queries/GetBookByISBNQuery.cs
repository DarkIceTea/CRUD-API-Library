using CRUDBooks.Models;

namespace CRUDBooks.Queries
{
    public class GetBookByISBNQuery : IQuery<Book>
    {
        public string ISBN { get; set; }
    }
}
