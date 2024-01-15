using CRUDBooks.Models;

namespace CRUDBooks.Commands
{
    public class EditBookCommand : ICommand
    {
        public int Id { get; set; }
        public Book UpdateBook { get; set; }
    }
}
