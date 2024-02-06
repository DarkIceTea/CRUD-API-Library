using CRUDBooks.Models;
using CRUDBooks.Queries;
using CRUDBooks.Repositiries;
using MediatR;

namespace CRUDBooks.Handlers
{
    public class GetBookByISBNQueryHandler : IRequestHandler<GetBookByISBNQuery, Book>
    {
        private readonly IBookRepository bookRepository;

        public GetBookByISBNQueryHandler(IBookRepository bookQueryRepository)
        {
            this.bookRepository = bookRepository;
        }

        async Task<Book> IRequestHandler<GetBookByISBNQuery, Book>.Handle(GetBookByISBNQuery request, CancellationToken cancellationToken)
        {
            return await bookRepository.GetBookByISBNAsync(request.ISBN, cancellationToken);
        }
    }
}
