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
            await bookCommandRepository.EditBookAsync(request.UpdateBook, request.Id, cancellationToken);
        }
    }
}
