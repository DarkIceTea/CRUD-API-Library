using CRUDBooks.Commands;
using CRUDBooks.Data;
using CRUDBooks.Models;

namespace CRUDBooks.Handlers
{
    public class EditBookCommandHandler : ICommandHandler<EditBookCommand>
    {
        private readonly DataContext _dataContext;

        public EditBookCommandHandler(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public void Execute(EditBookCommand command)
        {
            Book existingBook = _dataContext.Books.Find(command.Id);

            if (existingBook is null)
            {
                throw new Exception("Book not Found");
            }

            existingBook.Title = command.UpdateBook.Title ?? existingBook.Title;
            existingBook.Author = command.UpdateBook.Author ?? existingBook.Author;
            existingBook.ISBN = command.UpdateBook.ISBN ?? existingBook.ISBN;
            existingBook.Description = command.UpdateBook.Description ?? existingBook.Description;
            existingBook.Genre = command.UpdateBook.Genre ?? existingBook.Genre;
            existingBook.WhenTake = command.UpdateBook.WhenTake != default ? command.UpdateBook.WhenTake : existingBook.WhenTake;
            existingBook.WhenReturn = command.UpdateBook.WhenReturn != default ? command.UpdateBook.WhenReturn : existingBook.WhenReturn;
            _dataContext.Books.Update(existingBook);
            _dataContext.SaveChanges(true);
        }
    }
}
