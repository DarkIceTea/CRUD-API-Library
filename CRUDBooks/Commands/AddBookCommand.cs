using CRUDBooks.Models;
using MediatR;

namespace CRUDBooks.Commands
{
    public class AddBookCommand : IRequest
    {
        public Book Book { get; set; }
    }
}
