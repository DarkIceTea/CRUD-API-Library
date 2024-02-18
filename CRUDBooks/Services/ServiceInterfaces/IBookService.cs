using CRUDBooks.Dto;
using Microsoft.AspNetCore.Mvc;

namespace CRUDBooks.Services.ServiceInterfaces
{
    public interface IBookService
    {
        public Task<List<BookDto>> GetAllBooks();
        public Task<BookDto> GetBookById(int id);
        public Task<BookDto> GetBookByISBN(string isbn);
        public Task AddBook(BookDto bookDto);
        public Task EditBook(BookDto bookDto, int id);
        public Task DeleteBook(int id);
    }
}
