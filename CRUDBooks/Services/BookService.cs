using CRUDBooks.Commands;
using CRUDBooks.Dto;
using CRUDBooks.Exceptions;
using CRUDBooks.Models;
using CRUDBooks.Queries;
using CRUDBooks.Services.ServiceInterfaces;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CRUDBooks.Services
{
    public class BookService : IBookService
    {
        readonly IMediator mediator;

        public BookService(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task AddBook(BookDto bookDto)
        {
            if (bookDto is null)
            {
                throw new BadRequestException("bookDto is null");
            }
            Book book = bookDto.Adapt<Book>();

            var command = new AddBookCommand { Book = book };
            await mediator.Send(command);
        }

        public async Task DeleteBook(int id)
        {
            var command = new DeleteBookCommand { Id = id };
            await mediator.Send(command);
        }

        public async Task EditBook(BookDto bookDto, int id)
        {
            if (bookDto is null)
            {
                throw new BadRequestException("bookDto is null");
            }
            Book updateBook = bookDto.Adapt<Book>();
            var command = new EditBookCommand { Id = id, UpdateBook = updateBook };
            await mediator.Send(command);
        }

        public async Task<List<BookDto>> GetAllBooks()
        {
            var query = new GetAllBooksQuery();
            List<Book> books = await mediator.Send(query);

            List<BookDto> booksDto = books.Adapt<List<BookDto>>();
            return booksDto;
        }

        public async Task<BookDto> GetBookById(int id)
        {
            var query = new GetBookByIdQuery() { BookId = id };
            Book book = await mediator.Send(query);

            BookDto bookDto = book.Adapt<BookDto>();
            return bookDto;
        }

        public async Task<BookDto> GetBookByISBN(string isbn)
        {
            var query = new GetBookByISBNQuery() { ISBN = isbn };
            var book = await mediator.Send(query);

            BookDto bookDto = book.Adapt<BookDto>();
            return bookDto;
        }
    }
}
