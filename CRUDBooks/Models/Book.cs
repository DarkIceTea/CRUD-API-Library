namespace CRUDBooks.Models
{
    public class Book
    {
        public string ISBN { get; set; }
        public string Title { get; set; }
        public string Genre { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public DateTime WhenTake { get; set; }
        public DateTime WhenReturn { get; set; }
    }
}
