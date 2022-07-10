using DAL.Model;
using DAL.Repository;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    public class UnitOfWork : IUnitOfWork
    {
        public UnitOfWork(IRepo<Author> authors,
            IRepo<Genre> genres,
            IRepo<Book> books,
            IRepo<User> users,
            IRepo<UserBook> userBooks)
        {
            Authors = authors;
            Genres = genres;
            Books = books;
            Users = users;
            UserBooks = userBooks;
        }

        public IRepo<Author> Authors { get; }
        public IRepo<Genre> Genres { get; }
        public IRepo<Book> Books { get; }
        public IRepo<User> Users { get; }
        public IRepo<UserBook> UserBooks { get; }

        // practical crutch, used to save all possible changes
        public async Task CommitAsync()
        {
            await Authors.SaveAsync();
            await Genres.SaveAsync();
            await Books.SaveAsync();
            await Users.SaveAsync();
            await UserBooks.SaveAsync();
        }
    }
}