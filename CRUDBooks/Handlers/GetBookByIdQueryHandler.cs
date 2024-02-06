using CRUDBooks.Models;
using CRUDBooks.Queries;
using CRUDBooks.Repositiries;
using MediatR;

namespace CRUDBooks.Handlers
{
    public class GetBookByIdQueryHandler : IRequestHandler<GetBookByIdQuery,  Book>
    {
        private readonly IBookRepository bookRepository;

        public GetBookByIdQueryHandler(IBookRepository bookRepository)
        {
            this.bookRepository = bookRepository;
        }

        async Task<Book> IRequestHandler<GetBookByIdQuery, Book>.Handle(GetBookByIdQuery request, CancellationToken cancellationToken)
        {
            return await bookRepository.GetBookByIdAsync(request.BookId, cancellationToken);
        }
    }

}
