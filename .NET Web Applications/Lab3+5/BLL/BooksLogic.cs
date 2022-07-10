using DAL;
using DAL.Model;
using DAL.Repository;

namespace BLL
{
    public class BooksLogic : IBooksLogic
    {
        private IUnitOfWork _unitOfWork;

        public BooksLogic(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Book> AddBook(string name, string author, string genre, int amount)
        {
            try
            {
                // this will throw if book doesnt exist
                this.GetBook(name);
                throw new InvalidOperationException("Book already exists");
            }
            catch (ArgumentException)
            {
                var aObj = await this.AddAuthor(author);
                var gObj = await this.AddGenre(genre);

                var book = new Book
                {
                    Name = name,
                    AuthorId = aObj.Id,
                    GenreId = gObj.Id
                };

                _unitOfWork.Books.Add(book);
                await _unitOfWork.CommitAsync();

                // dont forget to increase copies amount
                await this.AddBookCopies(book.Id, amount);

                return book;
            }
        }

        public Book GetBook(string name)
        {
            var book = _unitOfWork.Books.GetAll().FirstOrDefault(x => x.Name == name);
            if (book == null)
            {
                throw new System.ArgumentException("Book does not exist");
            }
            // include author and genre to book object
            book.Author = this.GetAuthor(book.AuthorId);
            book.Genre = this.GetGenre(book.GenreId);
            return book;
        }
        public Book GetBook(int id)
        {
            var book = _unitOfWork.Books.GetById(id);
            if (book == null)
            {
                throw new System.ArgumentException("Book does not exist");
            }
            // include author and genre to book object
            book.Author = this.GetAuthor(book.AuthorId);
            book.Genre = this.GetGenre(book.GenreId);
            return book;
        }

        public IEnumerable<Book> GetAllBooks()
        {
            var books = _unitOfWork.Books.GetAll();

            // include author and genre to book objects
            foreach (var book in books)
            {
                book.Author = this.GetAuthor(book.AuthorId);
                book.Genre = this.GetGenre(book.GenreId);
            }

            return books;
        }

        public async Task<Book> UpdateBook(string name, string author, string genre, int amount = 0)
        {
            var book = this.GetBook(name);

            if (book == null)
            {
                new ArgumentException("Book does not exist");
            }

            // call overload with id
            return await this.UpdateBook(book!.Id, name, author, genre, amount);
        }
        public async Task<Book> UpdateBook(int id, string name, string author, string genre, int amount = 0)
        {  
            var book = this.GetBook(id);

            if (book == null)
            {
                new ArgumentException("Book does not exist");
            }

            // include author and genre to book object
            var aObj = await this.AddAuthor(author);
            var gObj = await this.AddGenre(genre);

            book!.Name = name;
            book!.Author = aObj;
            book!.Genre = gObj;

            await _unitOfWork.CommitAsync();

            // dont forget to increase copies amount
            await this.AddBookCopies(book!.Id, amount);

            return book;
        }
        
        public async Task DeleteBook(string name)
        {
            var book = this.GetBook(name);

            if (book == null)
            {
                new ArgumentException("Book does not exist");
            }

            _unitOfWork.Books.Delete(book);
            await _unitOfWork.CommitAsync();
        }
        public async Task DeleteBook(int id)
        {
            var book = this.GetBook(id);

            if (book == null)
            {
                new ArgumentException("Book does not exist");
            }

            _unitOfWork.Books.Delete(book);
            await _unitOfWork.CommitAsync();
        }

        public async Task<Genre> AddGenre(string name)
        {
            // if genre already exists, return it
            // if not, create new genre and return it

            var genre = _unitOfWork.Genres.GetAll().FirstOrDefault(x => x.Name == name);
            if (genre == null)
            {
                genre = new Genre { Name = name };
                _unitOfWork.Genres.Add(genre);
                await _unitOfWork.CommitAsync();
            }
            return genre;
        }
        
        public async Task<Author> AddAuthor(string name)
        {
            // if author already exists, return it
            // if not, create new author and return it

            var author = _unitOfWork.Authors.GetAll().FirstOrDefault(x => x.Name == name);
            if (author == null)
            {
                author = new Author { Name = name };
                _unitOfWork.Authors.Add(author);
                await _unitOfWork.CommitAsync();
            }
            return author;
        }
    
        public Genre GetGenre(int id)
        {
            var genre = _unitOfWork.Genres.GetById(id);
            if (genre == null)
            {
                throw new System.ArgumentException("Genre does not exist");
            }
            return genre;
        }

        public Author GetAuthor(int id)
        {
            var author = _unitOfWork.Authors.GetById(id);
            if (author == null)
            {
                throw new System.ArgumentException("Author does not exist");
            }
            return author;
        }

        public async Task AddBookCopies(string name, int amount)
        {
            if (name == null)
            {
                throw new System.ArgumentException("Book name or id is required");
            }

            var book = _unitOfWork.Books.GetAll().FirstOrDefault(x => x.Name == name);
            if (book == null)
            {
                throw new System.ArgumentException("Book not found");
            }

            // simple incrementing
            book.Amount += amount;
            await _unitOfWork.Books.SaveAsync();
        }
        public async Task AddBookCopies(int id, int amount)
        {
            if (id == 0)
            {
                throw new System.ArgumentException("Book name or id is required");
            }

            var book = _unitOfWork.Books.GetById(id);
            if (book == null)
            {
                throw new System.ArgumentException("Book not found");
            }

            // simple incrementing
            book.Amount += amount;
            await _unitOfWork.Books.SaveAsync();
        }
    
        public async Task RemoveBookCopies(string name, int amount)
        {
            if (name == null)
            {
                throw new System.ArgumentException("Book name or id is required");
            }

            var book = _unitOfWork.Books.GetAll().FirstOrDefault(x => x.Name == name);
            if (book == null)
            {
                throw new System.ArgumentException("Book not found");
            }
            // make sure book's count can't go below 0
            else if (book.Amount < amount)
            {
                throw new System.ArgumentException("Not enough copies");
            }

            // simple decrementing
            book.Amount -= amount;
            await _unitOfWork.Books.SaveAsync();
        }
        public async Task RemoveBookCopies(int id, int amount)
        {
            if (id == 0)
            {
                throw new System.ArgumentException("Book name or id is required");
            }

            var book = _unitOfWork.Books.GetById(id);
            if (book == null)
            {
                throw new System.ArgumentException("Book not found");
            }
            // make sure book's count can't go below 0
            else if (book.Amount < amount)
            {
                throw new System.ArgumentException("Not enough copies");
            }

            // simple decrementing
            book.Amount -= amount;
            await _unitOfWork.Books.SaveAsync();
        }
    }
}