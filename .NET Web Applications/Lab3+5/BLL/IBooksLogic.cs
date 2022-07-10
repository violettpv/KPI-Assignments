using DAL;
using DAL.Model;

namespace BLL
{
    public interface IBooksLogic
    {
        Task<Book> AddBook(string name, string author, string genre, int amount);
        IEnumerable<Book> GetAllBooks();
        Book GetBook(string name);
        Book GetBook(int id);
        Task<Book> UpdateBook(int id, string name, string author, string genre, int amount);
        Task DeleteBook(string name);
        Task DeleteBook(int id);
        Task<Genre> AddGenre(string name);
        Task<Author> AddAuthor(string name);
        Genre GetGenre(int id);
        Author GetAuthor(int id);
        Task AddBookCopies(string name, int amount);
        Task AddBookCopies(int id, int amount);
        Task RemoveBookCopies(string name, int amount);
        Task RemoveBookCopies(int id, int amount);
    }
}