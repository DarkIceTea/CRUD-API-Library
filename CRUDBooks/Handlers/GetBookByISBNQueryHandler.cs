using CRUDBooks.Models;
using CRUDBooks.Queries;
using CRUDBooks.Repositiries;
using MediatR;

namespace CRUDBooks.Handlers
{
    public class GetBookByISBNQueryHandler : IRequestHandler<GetBookByISBNQuery, Book>
    {
        private readonly IBookQueryRepository bookQueryRepository;

        public GetBookByISBNQueryHandler(IBookQueryRepository bookQueryRepository)
        {
            this.bookQueryRepository = bookQueryRepository;
        }

        async Task<Book> IRequestHandler<GetBookByISBNQuery, Book>.Handle(GetBookByISBNQuery request, CancellationToken cancellationToken)
        {
            return await bookQueryRepository.GetBookByISBNAsync(request.ISBN, cancellationToken);
        }
    }
}
