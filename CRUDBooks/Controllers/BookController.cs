using CRUDBooks.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CRUDBooks.Queries;
using CRUDBooks.Commands;
using MediatR;
using System.Text.Json.Serialization;
using System.Text.Json;
using CRUDBooks.Dto;
using Mapster;

namespace CRUDBooks.Controllers
{
    // Добавляем контроллер для работы с книгами
    [ApiController]
    [Authorize]
    public class BookController : Controller
    {
        private readonly IMediator mediator;
        

        public BookController(IMediator mediator)
        {
            this.mediator = mediator;

            TypeAdapterConfig<BookDto, Book>.NewConfig()
                .Map(dest => dest.Author, src => new Author { FirstName = src.AuthorName, LastName = src.AutorLastName})
                .Map(dest => dest.Genre, src => new Genre { Name = src.Genre });

            TypeAdapterConfig<Book, BookDto>.NewConfig()
                .Map(dest => dest.AuthorName, src => src.Author.FirstName)
                .Map(dest => dest.AutorLastName, src => src.Author.LastName)
                .Map(dest => dest.Genre, src => src.Genre.Name);
        }

        /// <summary>
        /// получение всех книг
        /// </summary>
        [HttpGet("/books")]
        public async Task<IActionResult> GetAllBooks()
        {
            var query = new GetAllBooksQuery();
            List<Book> books = await mediator.Send(query);

            List<BookDto> booksDto = books.Adapt<List<BookDto>>();
            return Json(booksDto);
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

            BookDto bookDto = book.Adapt<BookDto>();
            return Json(bookDto);
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

            BookDto bookDto = book.Adapt<BookDto>();
            return Json(bookDto);
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
            if (bookDto is null)
            {
                return BadRequest();
            }

            Book book = bookDto.Adapt<Book>();

            var command = new AddBookCommand { Book = book };
            await mediator.Send(command);
            
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
            if (updateBookDto is null)
            {
                return BadRequest();
            }

            Book updateBook = updateBookDto.Adapt<Book>();

            try
            {
                var command = new EditBookCommand { Id = id, UpdateBook = updateBook };
                await mediator.Send(command);
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
        [HttpDelete("/book/{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            try
            {
                var command = new DeleteBookCommand { Id = id };
                await mediator.Send(command);
                return Ok();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
