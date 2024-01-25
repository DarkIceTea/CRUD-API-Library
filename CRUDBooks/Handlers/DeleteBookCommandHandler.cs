using CRUDBooks.Commands;
using CRUDBooks.Data;
using CRUDBooks.Models;
using MediatR;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace CRUDBooks.Handlers
{
    public class DeleteBookCommandHandler : IRequestHandler<DeleteBookCommand>
    {
        private readonly DataContext _dataContext;

        public DeleteBookCommandHandler(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        async Task IRequestHandler<DeleteBookCommand>.Handle(DeleteBookCommand request, CancellationToken cancellationToken)
        {
            Book book = _dataContext.Books.Find(request.Id);

            if (book is null)
            {
                throw new Exception("Book Not Found");
            }
            _dataContext.Books.Remove(book);
            await _dataContext.SaveChangesAsync(true, cancellationToken);
        }
    }
}
