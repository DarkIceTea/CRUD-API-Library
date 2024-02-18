using CRUDBooks.Models;
using CRUDBooks.Queries;
using CRUDBooks.Repositiries;
using MediatR;

namespace CRUDBooks.Handlers
{
    public class GetAllBooksQueryHandler : IRequestHandler<GetAllBooksQuery, List<Book>>
    {
        private readonly IBookRepository bookRepository;
        public GetAllBooksQueryHandler(IBookRepository bookRepository)
        {
            this.bookRepository = bookRepository;
        }

        async Task<List<Book>> IRequestHandler<GetAllBooksQuery, List<Book>>.Handle(GetAllBooksQuery request, CancellationToken cancellationToken)
        {
            return await bookRepository.GetAllBooksAsync(cancellationToken);
        }
    }
}
