using CRUDBooks.Commands;
using CRUDBooks.Data;
using Mapster;
using CRUDBooks.Models;
using MediatR;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace CRUDBooks.Handlers
{
    public class AddBookCommandHandler : IRequestHandler<AddBookCommand>
    {
        private readonly DataContext _dataContext;

        public AddBookCommandHandler(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        async Task IRequestHandler<AddBookCommand>.Handle(AddBookCommand request, CancellationToken cancellationToken)
        {
            Book book = request.Book.Adapt<Book>();
            book.Id = 0; //Id назначается сам
            _dataContext.Books.Add(request.Book);
            await _dataContext.SaveChangesAsync(true, cancellationToken);
        }
    }
}
