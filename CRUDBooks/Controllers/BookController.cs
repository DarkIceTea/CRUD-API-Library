﻿using CRUDBooks.Data;
using CRUDBooks.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;

namespace CRUDBooks.Controllers
{
    // Добавляем контроллер для работы с книгами
    public class BookController
    {
        private readonly DataContext db;
        public BookController(DataContext context)
        {
            db = context;
        }
        [Authorize]
        public static async Task GetAllBooks(HttpContext context, DataContext dbContext)
        {
            var books = await dbContext.Books.ToListAsync();
            await context.Response.WriteAsJsonAsync(books);
        }
        [Authorize]
        public static async Task GetBookById(int id, HttpContext context, DataContext dbContext)
        {
            Book book = await dbContext.Books.FindAsync(id);
            if (book == null)
            {
                context.Response.StatusCode = StatusCodes.Status404NotFound;
            }
            else
            {
                await context.Response.WriteAsJsonAsync(book);
            }
        }
        [Authorize]
        public static async Task GetBookByISBN(string isbn, HttpContext context, DataContext dbContext)
        {
            Book book = await dbContext.Books.FirstAsync(b => b.ISBN == isbn);
            if (book == null)
            {
                context.Response.StatusCode = StatusCodes.Status404NotFound;
            }
            else
            {
                await context.Response.WriteAsJsonAsync(book);
            }
        }
        [Authorize]
        public static async Task AddBook(HttpContext context, DataContext dbContext)
        {
            Book book = await context.Request.ReadFromJsonAsync<Book>();
            if (book == null)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
            }
            else
            {
                dbContext.Books.Add(book);
                await dbContext.SaveChangesAsync(true);
            }
        }
        [Authorize]
        public static async Task EditBook(int id, HttpContext context, DataContext dbContext)
        {
            Book updateBook = await context.Request.ReadFromJsonAsync<Book>();
            if (updateBook == null)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                return;
            }
            Book existingBook = await dbContext.Books.FindAsync(id);
            if (existingBook == null)
            {
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                return;
            }
            existingBook.Title = updateBook.Title ?? existingBook.Title;
            existingBook.Author = updateBook.Author ?? existingBook.Author;
            existingBook.ISBN = updateBook.ISBN ?? existingBook.ISBN;
            existingBook.Description = updateBook.Description ?? existingBook.Description;
            existingBook.Genre = updateBook.Genre ?? existingBook.Genre;
            existingBook.WhenTake = updateBook.WhenTake != default ? updateBook.WhenTake : existingBook.WhenTake;
            existingBook.WhenReturn = updateBook.WhenReturn != default ? updateBook.WhenReturn : existingBook.WhenReturn;
            dbContext.Update(existingBook);
            await dbContext.SaveChangesAsync();
        }
        [Authorize]
        public static async Task DeleteBook(int id, HttpContext context, DataContext dbContext)
        {
            Book book = await dbContext.Books.FindAsync(id);
            if (book == null)
            {
                context.Response.StatusCode = StatusCodes.Status404NotFound;
            }
            else
            {
                dbContext.Books.Remove(book);
                dbContext.SaveChanges();
                context.Response.StatusCode = StatusCodes.Status200OK;
            }
        }
    }
}
