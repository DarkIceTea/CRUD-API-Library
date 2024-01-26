using CRUDBooks.Commands;
using CRUDBooks.Repositiries;
using MediatR;

namespace CRUDBooks.Handlers
{
    public class DeleteBookCommandHandler : IRequestHandler<DeleteBookCommand>
    {
        private readonly IBookCommandRepository bookCommandRepository;

        public DeleteBookCommandHandler(IBookCommandRepository bookCommandRepository)
        {
            this.bookCommandRepository = bookCommandRepository;
        }

        async Task IRequestHandler<DeleteBookCommand>.Handle(DeleteBookCommand request, CancellationToken cancellationToken)
        {
            await bookCommandRepository.DeleteBookAsync(request.Id, cancellationToken);
        }
    }
}
