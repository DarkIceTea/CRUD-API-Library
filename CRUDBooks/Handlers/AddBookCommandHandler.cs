using CRUDBooks.Commands;
using CRUDBooks.Data;
using Mapster;
using CRUDBooks.Models;

namespace CRUDBooks.Handlers
{
    public class AddBookCommandHandler : ICommandHandler<AddBookCommand>
    {
        private readonly DataContext _dataContext;

        public AddBookCommandHandler(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public void Execute(AddBookCommand command)
        {
            Book book = command.Book.Adapt<Book>();
            book.Id = 0; //Id назначается сам
            _dataContext.Books.Add(command.Book);
            _dataContext.SaveChanges(true);
        }
    }
}
