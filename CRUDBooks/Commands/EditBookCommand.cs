using CRUDBooks.Models;
using MediatR;

namespace CRUDBooks.Commands
{
    public class EditBookCommand : IRequest
    {
        public int Id { get; set; }
        public Book UpdateBook { get; set; }
    }
}
