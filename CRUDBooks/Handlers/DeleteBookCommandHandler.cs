using CRUDBooks.Commands;
using CRUDBooks.Data;
using CRUDBooks.Models;

namespace CRUDBooks.Handlers
{
    public class DeleteBookCommandHandler : ICommandHandler<DeleteBookCommand>
    {
        private readonly DataContext _dataContext;

        public DeleteBookCommandHandler(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public void Execute(DeleteBookCommand command)
        {
            Book book = _dataContext.Books.Find(command.Id);

            if (book is null)
            {
                throw new Exception("Book Not Found");
            }
            _dataContext.Books.Remove(book);
            _dataContext.SaveChangesAsync(true);
        }

    }
}
