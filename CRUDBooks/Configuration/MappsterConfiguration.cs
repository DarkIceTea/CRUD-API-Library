using CRUDBooks.Dto;
using CRUDBooks.Models;
using Mapster;

namespace CRUDBooks.Configuration
{
    public class MappsterConfiguration
    {
        public void Configure()
        {
            TypeAdapterConfig<Book, BookDto>.NewConfig()
                .Map(dest => dest.AuthorName, src => src.Author.FirstName)
                .Map(dest => dest.AutorLastName, src => src.Author.LastName)
                .Map(dest => dest.Genre, src => src.Genre.Name);

            TypeAdapterConfig<BookDto, Book>.NewConfig()
                .Map(dest => dest.Author, src => new Author
                {
                    FirstName = src.AuthorName,
                    LastName = src.AutorLastName
                })
                .Map(dest => dest.Genre.Name, src => src.Genre);
        }
    }
}
