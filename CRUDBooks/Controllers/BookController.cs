using CRUDBooks.Data;
using CRUDBooks.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace CRUDBooks.Controllers
{
    // Добавляем контроллер для работы с книгами
    [ApiController]
    //[Route("[Controller]")]
    public class BookController : Controller
    {
        private readonly DataContext dataContext;
        private readonly HttpContext httpContext;
        public BookController(DataContext dataContext, IHttpContextAccessor accessor)
        {
            this.dataContext = dataContext;
            httpContext = accessor.HttpContext;
        }

        //Получение всех книг
        [HttpGet("/books")]
        [Authorize]
        public async Task GetAllBooks()
        {
            var books = await dataContext.Books.ToListAsync();
            await httpContext.Response.WriteAsJsonAsync(books);
        }

        //Получение книги по id
        [HttpGet("/book/{id}")]
        [Authorize]
        public async Task GetBookById(int id)
        {
            Book book = await dataContext.Books.FindAsync(id);
            if (book == null)
            {
                httpContext.Response.StatusCode = StatusCodes.Status404NotFound;
            }
            else
            {
                await httpContext.Response.WriteAsJsonAsync(book);
            }
        }

        //Получение книги по ISBN
        [HttpGet("/book/ISBN/{isbn}")]
        [Authorize]
        public async Task GetBookByISBN(string isbn)
        {
            Book book = await dataContext.Books.FirstAsync(b => b.ISBN == isbn);
            if (book == null)
            {
                HttpContext.Response.StatusCode = StatusCodes.Status404NotFound;
            }
            else
            {
                await HttpContext.Response.WriteAsJsonAsync(book);
            }
        }

        //Добавление книги
        [HttpPost("/book")]
        [Authorize]
        public async Task AddBook()
        {
            Book book = await HttpContext.Request.ReadFromJsonAsync<Book>();
            if (book == null)
            {
                HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            }
            else
            {
                dataContext.Books.Add(book);
                await dataContext.SaveChangesAsync(true);
            }
        }

        //Редактирование книги
        [HttpPut("/book/{id}")]
        [Authorize]
        public async Task EditBook(int id)
        {
            Book updateBook = await httpContext.Request.ReadFromJsonAsync<Book>();
            if (updateBook == null)
            {
                HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                return;
            }
            Book existingBook = await dataContext.Books.FindAsync(id);
            if (existingBook == null)
            {
                httpContext.Response.StatusCode = StatusCodes.Status404NotFound;
                return;
            }
            existingBook.Title = updateBook.Title ?? existingBook.Title;
            existingBook.Author = updateBook.Author ?? existingBook.Author;
            existingBook.ISBN = updateBook.ISBN ?? existingBook.ISBN;
            existingBook.Description = updateBook.Description ?? existingBook.Description;
            existingBook.Genre = updateBook.Genre ?? existingBook.Genre;
            existingBook.WhenTake = updateBook.WhenTake != default ? updateBook.WhenTake : existingBook.WhenTake;
            existingBook.WhenReturn = updateBook.WhenReturn != default ? updateBook.WhenReturn : existingBook.WhenReturn;
            dataContext.Update(existingBook);
            await dataContext.SaveChangesAsync();
        }

        //Удалениие книги
        [HttpDelete("/book/{id}")]
        [Authorize]
        public async Task DeleteBook(int id)
        {
            Book book = await dataContext.Books.FindAsync(id);
            if (book == null)
            {
                httpContext.Response.StatusCode = StatusCodes.Status404NotFound;
            }
            else
            {
                dataContext.Books.Remove(book);
                dataContext.SaveChanges();
                httpContext.Response.StatusCode = StatusCodes.Status200OK;
            }
        }
    }
}
