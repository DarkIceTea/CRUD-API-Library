using CRUDBooks.Commands;
using CRUDBooks.Data;

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
            _dataContext.Books.Add(command.Book);
            _dataContext.SaveChanges(true);
        }
    }
}
