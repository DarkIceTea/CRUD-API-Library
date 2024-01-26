using CRUDBooks.Commands;
using MediatR;
using CRUDBooks.Repositiries;

namespace CRUDBooks.Handlers
{
    public class AddBookCommandHandler : IRequestHandler<AddBookCommand>
    {
        private readonly IBookCommandRepository bookCommandRepository;

        public AddBookCommandHandler(IBookCommandRepository bookCommandRepository)
        {
            this.bookCommandRepository = bookCommandRepository;
        }

        async Task IRequestHandler<AddBookCommand>.Handle(AddBookCommand request, CancellationToken cancellationToken)
        {
            await bookCommandRepository.AddBookAsync(request.Book, cancellationToken);
        }
    }
}
