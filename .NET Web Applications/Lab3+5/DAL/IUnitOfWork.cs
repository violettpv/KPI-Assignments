using DAL.Model;
using DAL.Repository;

namespace DAL
{
    public interface IUnitOfWork
    {
        IRepo<Author> Authors { get; }
        IRepo<Genre> Genres { get; }
        IRepo<Book> Books { get; }
        IRepo<User> Users { get; }
        IRepo<UserBook> UserBooks { get; }

        Task CommitAsync();
    }
}