using CRUDBooks.Data;
using CRUDBooks.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using CRUDBooks.Queries;
using CRUDBooks.Commands;

namespace CRUDBooks.Controllers
{
    // Добавляем контроллер для работы с книгами
    [ApiController]
    //[Route("[Controller]")]
    public class BookController : Controller
    {
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly IQueryDispatcher _queryDispatcher;
        private readonly HttpContext httpContext;

        public BookController(IHttpContextAccessor accessor, IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher)
        {
            httpContext = accessor.HttpContext;
            _queryDispatcher = queryDispatcher;
            _commandDispatcher = commandDispatcher;
        }

        //Получение всех книг
        [HttpGet("/books")]
        //[Authorize]
        public async Task GetAllBooks()
        {
            //var books = await dataContext.Books.ToListAsync();
            //await httpContext.Response.WriteAsJsonAsync(books);
            var query = new GetAllBooksQuery();
            var books = _queryDispatcher.Execute<GetAllBooksQuery, List<Book>>(query);
            await httpContext.Response.WriteAsJsonAsync(books);
        }

        //Получение книги по id
        [HttpGet("/book/{id}")]
        //[Authorize]
        public async Task GetBookById(int id)
        {
            var query = new GetBookByIdQuery() {BookId = id};
            Book book = _queryDispatcher.Execute<GetBookByIdQuery, Book>(query);
            if (book is null)
            {
                httpContext.Response.StatusCode = StatusCodes.Status404NotFound;
                return;
            }
            await httpContext.Response.WriteAsJsonAsync(book);
        }

        //Получение книги по ISBN
        [HttpGet("/book/ISBN/{isbn}")]
        //[Authorize]
        public async Task GetBookByISBN(string isbn)
        {
            var query = new GetBookByISBNQuery() { ISBN = isbn};
            var book = _queryDispatcher.Execute<GetBookByISBNQuery, Book>(query);
            if (book is null)
            {
                httpContext.Response.StatusCode = StatusCodes.Status404NotFound;
                return;
            }
            await httpContext.Response.WriteAsJsonAsync(book);
        }

        //Добавление книги
        [HttpPost("/book")]
        //[Authorize]
        public async Task AddBook()
        {
            Book book = await httpContext.Request.ReadFromJsonAsync<Book>();
            if (book is null)
            {
                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                return;
            }
            var command = new AddBookCommand { Book = book };
            _commandDispatcher.Execute(command);
        }

        //Редактирование книги
        [HttpPut("/book/{id}")]
        //[Authorize]
        public async Task EditBook(int id)
        {
            Book updateBook = await httpContext.Request.ReadFromJsonAsync<Book>();
            if (updateBook is null)
            {
                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                return;
            }

            try
            {
                var command = new EditBookCommand { Id = id, UpdateBook = updateBook };
                _commandDispatcher.Execute(command);
            }
            catch (Exception ex)
            {
                httpContext.Response.StatusCode = StatusCodes.Status404NotFound;
            }
        }

        //Удалениие книги
        [HttpDelete("/book/{id}")]
        //[Authorize]
        public async Task DeleteBook(int id)
        {
            try
            {
                var command = new DeleteBookCommand { Id = id };
                _commandDispatcher.Execute(command);
            }
            catch (Exception ex)
            {
                httpContext.Response.StatusCode = StatusCodes.Status404NotFound;
            }
        }
    }
}
