using MediatR;

namespace CRUDBooks.Commands
{
    public class DeleteBookCommand : IRequest
    {
        public int Id { get; set; }
    }
}
