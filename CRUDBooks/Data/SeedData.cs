using CRUDBooks.Models;

namespace CRUDBooks.Data
{
    public class SeedData : IDataContextInitializer
    {
        readonly DataContext dataContext;

        public SeedData(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public void Initialize()
        {
            if (dataContext.Books.Any()) return;

            Author gogol, dostoevskiy;
            Genre satire, drama;

            dataContext.Authors.AddRange(   
                gogol = new Author() { FirstName = "Nikolay", LastName = "Gogol" },
                dostoevskiy = new Author() { FirstName = "Fyodor", LastName = "Dostoevskiy"});

            dataContext.Genres.AddRange(
                satire = new Genre() { Name = "Satire" },
                drama = new Genre() { Name = "Drama" });

            dataContext.Books.AddRange(
                new Book { Title = "Crime and panishment", Author = dostoevskiy, Description = "about roskolnikov", Genre = drama, ISBN = "978-5-93673-265-2", WhenTake = DateTime.Now, WhenReturn = DateTime.Now.AddMonths(1) },
                new Book { Title = "Dead Souls", Author = gogol, Description = "about Chichikov", Genre = satire, ISBN = "978-5-93673-435-2", WhenTake = DateTime.Now, WhenReturn = DateTime.Now.AddMonths(1) });

            dataContext.Users.Add(new User { Login = "Konstantin", Password = "Ldzk4Uai7Y8LKtylgV7nlqBrBZ2R/KPh+/W/8QKnYlU=" });       //Пароль 12345
            dataContext.SaveChanges();
        }
    }
}
