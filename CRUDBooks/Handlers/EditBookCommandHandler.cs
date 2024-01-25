using CRUDBooks.Commands;
using CRUDBooks.Data;
using CRUDBooks.Models;
using MediatR;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace CRUDBooks.Handlers
{
    public class EditBookCommandHandler : IRequestHandler<EditBookCommand>
    {
        private readonly DataContext _dataContext;

        public EditBookCommandHandler(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public void Handle(EditBookCommand command)
        {
            
        }

        async Task IRequestHandler<EditBookCommand>.Handle(EditBookCommand request, CancellationToken cancellationToken)
        {
            Book existingBook = _dataContext.Books.Find(request.Id);

            if (existingBook is null)
            {
                throw new Exception("Book not Found");
            }

            existingBook.Title = request.UpdateBook.Title ?? existingBook.Title;
            existingBook.Author = request.UpdateBook.Author ?? existingBook.Author;
            existingBook.ISBN = request.UpdateBook.ISBN ?? existingBook.ISBN;
            existingBook.Description = request.UpdateBook.Description ?? existingBook.Description;
            existingBook.Genre = request.UpdateBook.Genre ?? existingBook.Genre;
            existingBook.WhenTake = request.UpdateBook.WhenTake != default ? request.UpdateBook.WhenTake : existingBook.WhenTake;
            existingBook.WhenReturn = request.UpdateBook.WhenReturn != default ? request.UpdateBook.WhenReturn : existingBook.WhenReturn;

            _dataContext.Books.Update(existingBook);
            await _dataContext.SaveChangesAsync(true, cancellationToken);
        }
    }
}
