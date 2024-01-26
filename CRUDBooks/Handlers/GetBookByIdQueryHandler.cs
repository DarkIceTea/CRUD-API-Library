using CRUDBooks.Models;
using CRUDBooks.Queries;
using CRUDBooks.Repositiries;
using MediatR;

namespace CRUDBooks.Handlers
{
    public class GetBookByIdQueryHandler : IRequestHandler<GetBookByIdQuery,  Book>
    {
        private readonly IBookQueryRepository bookQueryRepository;

        public GetBookByIdQueryHandler(IBookQueryRepository bookQueryRepository)
        {
            this.bookQueryRepository = bookQueryRepository;
        }

        async Task<Book> IRequestHandler<GetBookByIdQuery, Book>.Handle(GetBookByIdQuery request, CancellationToken cancellationToken)
        {
            return await bookQueryRepository.GetBookByIdAsync(request.BookId, cancellationToken);
        }
    }

}
