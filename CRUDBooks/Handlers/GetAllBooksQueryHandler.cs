using CRUDBooks.Models;
using CRUDBooks.Queries;
using CRUDBooks.Repositiries;
using MediatR;

namespace CRUDBooks.Handlers
{
    public class GetAllBooksQueryHandler : IRequestHandler<GetAllBooksQuery, List<Book>>
    {
        private readonly IBookQueryRepository bookQueryRepository;
        public GetAllBooksQueryHandler(IBookQueryRepository bookQueryRepository)
        {
            this.bookQueryRepository = bookQueryRepository;
        }

        async Task<List<Book>> IRequestHandler<GetAllBooksQuery, List<Book>>.Handle(GetAllBooksQuery request, CancellationToken cancellationToken)
        {
            return await bookQueryRepository.GetAllBooksAsync(cancellationToken);
        }
    }
}
