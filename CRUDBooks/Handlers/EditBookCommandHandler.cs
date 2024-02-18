using CRUDBooks.Commands;
using CRUDBooks.Repositiries;
using MediatR;

namespace CRUDBooks.Handlers
{
    public class EditBookCommandHandler : IRequestHandler<EditBookCommand>
    {
        private readonly IBookRepository bookRepository;

        public EditBookCommandHandler(IBookRepository bookCommandRepository)
        {
            this.bookRepository = bookCommandRepository;
        }

        async Task IRequestHandler<EditBookCommand>.Handle(EditBookCommand request, CancellationToken cancellationToken)
        {
            await bookRepository.EditBookAsync(request.UpdateBook, request.Id, cancellationToken);
        }
    }
}
