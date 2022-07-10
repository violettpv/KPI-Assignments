using DAL;
using DAL.Model;
using DAL.Repository;

namespace BLL
{
    public class UsersLogic : IUsersLogic
    {
        private IUnitOfWork _unitOfWork;
        private IBooksLogic _booksLogic;

        public UsersLogic(IUnitOfWork unitOfWork, IBooksLogic booksLogic)
        {
            _unitOfWork = unitOfWork;
            _booksLogic = booksLogic;
        }

        // NOTE: users' names are not unique,
        // so we will interact with particular user ONLY by id

        public async Task<User> AddUser(string name)
        {
            var user = new User { Name = name };
            _unitOfWork.Users.Add(user);
            await _unitOfWork.CommitAsync();

            return user;
        }
    
        public async Task<User> UpdateUser(int id, string name)
        {
            var user = this.GetUser(id);

            if (user == null)
            {
                throw new ArgumentException("User not found");
            }

            user.Name = name;
            await _unitOfWork.CommitAsync();

            return user;
        }

        public async Task DeleteUser(int id)
        {
            var user = this.GetUser(id);

            if (user == null)
            {
                throw new ArgumentException("User not found");
            }

            // dont forget to delete all user's books
            var userBooks = _unitOfWork.UserBooks.GetAll().Where(x => x.UserId == id);
            foreach(var userBook in userBooks)
            {
                // and don't forget to increase book's quantity
                await _booksLogic.AddBookCopies(userBook.BookId, 1);
                _unitOfWork.UserBooks.Delete(userBook);
            }

            _unitOfWork.Users.Delete(user);

            await _unitOfWork.UserBooks.SaveAsync();
            await _unitOfWork.Users.SaveAsync();
        }

        public User GetUser(int id)
        {
            var user = _unitOfWork.Users.GetById(id);
            if (user == null)
            {
                throw new ArgumentException("User does not exist");
            }
            
            return user;
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _unitOfWork.Users.GetAll();
        }

        public async Task AddBook(int userId, int bookId)
        {
            var userBooks = this.GetUserBooks(userId);
            // check if user has this book
            if (userBooks.Where(b => b.BookId == bookId).Any())
            {
                throw new System.ArgumentException("User already has this book");
            }
            // check if user has less then 10 books
            else if (userBooks.Count() >= 10)
            {
                throw new System.ArgumentException("User has too many books");
            }

            // get user and book objects for many-to-many relationship
            var user = this.GetUser(userId);
            var book = _booksLogic.GetBook(bookId);
            // check if book is in stock
            if (book.Amount == 0)
            {
                throw new System.ArgumentException("Book is out of stock");
            }

            // create and add a many-to-many relationship pair
            var pair = new UserBook { UserId = user.Id, BookId = book.Id };
            _unitOfWork.UserBooks.Add(pair);

            // update book amount
            await _booksLogic.RemoveBookCopies(book.Id, 1);

            await _unitOfWork.UserBooks.SaveAsync();
        }

        public IEnumerable<UserBook> GetUserBooks(int userId)
        {
            var userBooks = _unitOfWork.UserBooks.GetAll().Where(ub => ub.UserId == userId);

            foreach (var userBook in userBooks)
            {
                userBook.Book = _booksLogic.GetBook(userBook.BookId);
            }

            return userBooks;
        }

        public async Task RemoveBook(int userId, int bookId)
        {
            var userBooks = this.GetUserBooks(userId);
            var userBook = userBooks.Where(ub => ub.BookId == bookId).FirstOrDefault();
            if (userBook == null)
            {
                throw new System.ArgumentException("User does not have this book");
            }

            _unitOfWork.UserBooks.Delete(userBook);

            await _booksLogic.AddBookCopies(bookId, 1);

            await _unitOfWork.UserBooks.SaveAsync();
        }
    }
}