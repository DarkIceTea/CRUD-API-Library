using CRUDBooks.Models;

namespace CRUDBooks.Commands
{
    public class AddBookCommand : ICommand
    {
        public Book Book { get; set; }
    }
}
