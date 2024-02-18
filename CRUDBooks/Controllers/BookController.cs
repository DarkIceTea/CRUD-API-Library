using CRUDBooks.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CRUDBooks.Queries;
using CRUDBooks.Commands;
using MediatR;
using CRUDBooks.Dto;
using Mapster;
using CRUDBooks.Services.ServiceInterfaces;

namespace CRUDBooks.Controllers
{
    // Добавляем контроллер для работы с книгами
    [ApiController]
    [Authorize]
    public class BookController : Controller
    {
        private readonly IBookService bookService;

        public BookController(IMediator mediator, IBookService bookService)
        {
            this.bookService = bookService;
        }

        /// <summary>
        /// получение всех книг
        /// </summary>
        [HttpGet("/books")]
        public async Task<IActionResult> GetAllBooks()
        {
            List<BookDto> bookList = await bookService.GetAllBooks();
            return Ok(bookList);
        }

        /// <summary>
        /// получение книги по id.
        /// </summary>
        /// <param name="id">id книги которую нужно получить</param>
        [HttpGet("/book/{id}")]
        public async Task<IActionResult> GetBookById(int id)
        {
            BookDto book = await bookService.GetBookById(id);
            return Ok(book);
        }

        /// <summary>
        /// получение книги по isbn.
        /// </summary>
        /// <param name="isbn">isbn книги которую нужно получить</param>
        [HttpGet("/book/ISBN/{isbn}")]
        public async Task<IActionResult> GetBookByISBN(string isbn)
        {
            BookDto book = await bookService.GetBookByISBN(isbn);
            return Ok(book);
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
        ///         {
        ///         "title": "Dead Souls",
        ///         "isbn": "978-5-93673-435-2",
        ///         "description": "about Chichikov",
        ///         "authorName": "Nikolay",
        ///         "autorLastName": "Gogol",
        ///         "genre": "Satire"
        ///         }
        ///     }
        /// 
        /// </remarks>
        /// <param name="book">Информация о новой книге.</param>
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost("/book")]
        public async Task<IActionResult> AddBook([FromBody]BookDto bookDto)
        {
            bookService.AddBook(bookDto);
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
        ///        {
        ///         "title": "Dead Souls",
        ///         "isbn": "978-5-93673-435-2",
        ///         "description": "about Chichikov",
        ///         "authorName": "Nikolay",
        ///         "autorLastName": "Gogol",
        ///         "genre": "Satire"
        ///         }
        ///     }
        /// 
        /// </remarks>
        /// <param name="updateBook">Информация о обновлённой книге.</param>
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPut("/book/{id}")]
        public async Task<IActionResult> EditBook(int id, [FromBody] BookDto updateBookDto)
        {
            bookService.EditBook(updateBookDto, id);
            return Ok();
        }

        /// <summary>
        /// удаление книги.
        /// </summary>
        /// <param name="id">id книги которую нужно удалить</param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpDelete("/book/{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            bookService.DeleteBook(id);
            return Ok();
        }
    }
}
