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
    //[Authorize]
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

        /// <summary>
        /// получение всех книг
        /// </summary>
        [HttpGet("/books")]
        public async Task GetAllBooks()
        {
            //var books = await dataContext.Books.ToListAsync();
            //await httpContext.Response.WriteAsJsonAsync(books);
            var query = new GetAllBooksQuery();
            var books = _queryDispatcher.Execute<GetAllBooksQuery, List<Book>>(query);

            await httpContext.Response.WriteAsJsonAsync(books);
        }

        /// <summary>
        /// получение книги по id.
        /// </summary>
        /// <param name="id">id книги которую нужно получить</param>
        [HttpGet("/book/{id}")]
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

        /// <summary>
        /// получение книги по isbn.
        /// </summary>
        /// <param name="isbn">isbn книги которую нужно получить</param>
        [HttpGet("/book/ISBN/{isbn}")]
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

        /// <summary>
        /// Добавление новой книги.
        /// </summary>
        /// 
        /// <remarks>
        /// Пример запроса:
        /// 
        ///     POST /book
        ///     {
        ///        "id": 1,
        ///        "isbn": "1234567890",
        ///        "title": "Пример книги",
        ///        "author": "Автор",
        ///        "genre": "Жанр",
        ///        "description": "Описание",
        ///        "whenTake": "2023-01-04T12:00:00",
        ///        "whenReturn": "2023-01-14T12:00:00"
        ///     }
        /// 
        /// </remarks>
        /// <param name="book">Информация о новой книге.</param>
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("/book")]
        public async Task AddBook([FromBody]Book book)
        {
            //Book book = await httpContext.Request.ReadFromJsonAsync<Book>();
            if (book is null)
            {
                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                return;
            }
            var command = new AddBookCommand { Book = book };
            _commandDispatcher.Execute(command);
        }

        /// <summary>
        /// редактирование книги.
        /// </summary>
        /// 
        /// <remarks>
        /// Пример запроса:
        /// 
        ///     PUT /book
        ///     {
        ///        "id": 1,
        ///        "isbn": "1234567890",
        ///        "title": "Пример изменённой книги",
        ///        "author": "Автор",
        ///        "genre": "Жанр",
        ///        "description": "Описание",
        ///        "whenTake": "2023-01-04T12:00:00",
        ///        "whenReturn": "2023-01-14T12:00:00"
        ///     }
        /// 
        /// </remarks>
        /// <param name="updateBook">Информация о обновлённой книге.</param>
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPut("/book/{id}")]
        public async Task EditBook(int id, [FromBody] Book updateBook)
        {
            //Book updateBook = await httpContext.Request.ReadFromJsonAsync<Book>();
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

        /// <summary>
        /// удаление книги.
        /// </summary>
        /// <param name="id">id книги которую нужно удалить</param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete("/book/{id}")]
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
