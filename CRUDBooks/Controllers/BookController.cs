using CRUDBooks.Data;
using CRUDBooks.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using CRUDBooks.Queries;
using CRUDBooks.Commands;
using MediatR;

namespace CRUDBooks.Controllers
{
    // Добавляем контроллер для работы с книгами
    [ApiController]
    //[Authorize]
    public class BookController : Controller
    {
        private readonly IMediator mediator;
        private readonly HttpContext httpContext;

        public BookController(IHttpContextAccessor accessor, IMediator mediator)
        {
            httpContext = accessor.HttpContext;
            this.mediator = mediator;
        }

        /// <summary>
        /// получение всех книг
        /// </summary>
        [HttpGet("/books")]
        public async Task<IActionResult> GetAllBooks()
        {
            var query = new GetAllBooksQuery();
            List<Book> books = await mediator.Send(query);

            return Json(books);
        }

        /// <summary>
        /// получение книги по id.
        /// </summary>
        /// <param name="id">id книги которую нужно получить</param>
        [HttpGet("/book/{id}")]
        public async Task<IActionResult> GetBookById(int id)
        {
            var query = new GetBookByIdQuery() {BookId = id};
            Book book = await mediator.Send(query);
            if (book is null)
            {
                return NotFound();
            }
            return Json(book);
        }

        /// <summary>
        /// получение книги по isbn.
        /// </summary>
        /// <param name="isbn">isbn книги которую нужно получить</param>
        [HttpGet("/book/ISBN/{isbn}")]
        public async Task<IActionResult> GetBookByISBN(string isbn)
        {
            var query = new GetBookByISBNQuery() { ISBN = isbn};
            var book = await mediator.Send(query);
            if (book is null)
            {
                return NotFound();
            }
            return Json(book);
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
        public async Task<IActionResult> AddBook([FromBody]Book book)
        {
            //Book book = await httpContext.Request.ReadFromJsonAsync<Book>();
            if (book is null)
            {
                return NotFound();
            }
            var command = new AddBookCommand { Book = book };
            mediator.Send(command);
            
            return Ok();
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
        [HttpPut("/book/{id}")]
        public async Task<IActionResult> EditBook(int id, [FromBody] Book updateBook)
        {
            //Book updateBook = await httpContext.Request.ReadFromJsonAsync<Book>();
            if (updateBook is null)
            {
                return BadRequest();
            }

            try
            {
                var command = new EditBookCommand { Id = id, UpdateBook = updateBook };
                mediator.Send(command);
                return Ok();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
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
        public async Task<IActionResult> DeleteBook(int id)
        {
            try
            {
                var command = new DeleteBookCommand { Id = id };
                mediator.Send(command);
                return Ok();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
