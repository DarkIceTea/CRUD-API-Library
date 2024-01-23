namespace CRUDBooks.Models
{
    /// <summary>
    /// Represents a book entity.
    /// </summary>
    public class Book
    {
        /// <summary>
        /// Gets or sets the unique identifier of the book.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets the ISBN (International Standard Book Number) of the book.
        /// </summary>
        public string ISBN { get; set; }
        /// <summary>
        /// Gets or sets the title of the book.
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Gets or sets the description of the book.
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Gets or sets the date when the book was taken.
        /// </summary>
        public DateTime WhenTake { get; set; }
        /// <summary>
        /// Gets or sets the date when the book should be returned.
        /// </summary>
        public DateTime WhenReturn { get; set; }

        public int AuthorId { get; set; }
        public Author Author { get; set; }

        public int GenreId { get; set; }
        public Genre Genre { get; set; }
    }
}
