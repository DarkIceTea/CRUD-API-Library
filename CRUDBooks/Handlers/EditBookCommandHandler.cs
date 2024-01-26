using CRUDBooks.Commands;
using CRUDBooks.Repositiries;
using MediatR;

namespace CRUDBooks.Handlers
{
    public class EditBookCommandHandler : IRequestHandler<EditBookCommand>
    {
        private readonly IBookCommandRepository bookCommandRepository;

        public EditBookCommandHandler(IBookCommandRepository bookCommandRepository)
        {
            this.bookCommandRepository = bookCommandRepository;
        }

        async Task IRequestHandler<EditBookCommand>.Handle(EditBookCommand request, CancellationToken cancellationToken)
        {

            //existingBook.Title = request.UpdateBook.Title ?? existingBook.Title;
            //existingBook.Author = request.UpdateBook.Author ?? existingBook.Author;
            //existingBook.ISBN = request.UpdateBook.ISBN ?? existingBook.ISBN;
            //existingBook.Description = request.UpdateBook.Description ?? existingBook.Description;
            //existingBook.Genre = request.UpdateBook.Genre ?? existingBook.Genre;
            //existingBook.WhenTake = request.UpdateBook.WhenTake != default ? request.UpdateBook.WhenTake : existingBook.WhenTake;
            //existingBook.WhenReturn = request.UpdateBook.WhenReturn != default ? request.UpdateBook.WhenReturn : existingBook.WhenReturn;

            await bookCommandRepository.EditBookAsync(request.UpdateBook, cancellationToken);
        }
    }
}
