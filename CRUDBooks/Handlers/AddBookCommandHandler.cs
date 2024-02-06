using CRUDBooks.Commands;
using MediatR;
using CRUDBooks.Repositiries;

namespace CRUDBooks.Handlers
{
    public class AddBookCommandHandler : IRequestHandler<AddBookCommand>
    {
        private readonly IBookRepository bookRepository;

        public AddBookCommandHandler(IBookRepository bookRepository)
        {
            this.bookRepository = bookRepository;
        }

        async Task IRequestHandler<AddBookCommand>.Handle(AddBookCommand request, CancellationToken cancellationToken)
        {
            await bookRepository.AddBookAsync(request.Book, cancellationToken);
        }
    }
}
