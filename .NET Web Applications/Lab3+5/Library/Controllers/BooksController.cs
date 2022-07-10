using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Library.Model;

namespace Library.Controllers
{
    [ApiController]
    [Route("api")]
    public class BooksController : ControllerBase
    {
        private readonly IBooksLogic _booksLogic;

        public BooksController(IBooksLogic booksLogic)
        {
            _booksLogic = booksLogic;
        }

        [HttpGet]
        [Route("books")]
        public IActionResult GetBooks()
        {
            try
            {
                var books = _booksLogic.GetAllBooks();
                var res = new List<BookDto>();

                foreach (var book in books)
                {
                    res.Add(new BookDto(book));
                }

                return Ok(res);
            }
            catch
            {
                return StatusCode(500);
            }
        }

        [HttpGet]
        [Route("books/name")]
        public IActionResult GetBook([FromQuery(Name = "Name")] string name)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var book = _booksLogic.GetBook(name);
                var res = new BookDto(book);

                return Ok(res);
            }
            catch (System.ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch
            {
                return StatusCode(500);
            }
        }

        [HttpGet]
        [Route("books/author")]
        public IActionResult GetBookByAuthor([FromQuery(Name = "Author")] string author)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var books = _booksLogic.GetAllBooks().Where(b => b.Author!.Name == author);
                var res = new List<BookDto>();

                foreach (var book in books)
                {
                    res.Add(new BookDto(book));
                }

                return Ok(res);
            }
            catch
            {
                return StatusCode(500);
            }
        }

        [HttpGet]
        [Route("books/genre")]
        public IActionResult GetBookByGenre([FromQuery(Name = "Genre")] string genre)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var books = _booksLogic.GetAllBooks().Where(b => b.Genre!.Name == genre);
                var res = new List<BookDto>();

                foreach (var book in books)
                {
                    res.Add(new BookDto(book));
                }

                return Ok(res);
            }
            catch
            {
                return StatusCode(500);
            }
        }

        [HttpGet]
        [Route("books/{id}")]
        public IActionResult GetBook(int id)
        {
            try
            {
                var book = _booksLogic.GetBook(id);
                var res = new BookDto(book);

                return Ok(res);
            }
            catch (System.ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch
            {
                return StatusCode(500);
            }
        }
    
        [HttpPost]
        [Route("books")]
        public async Task<IActionResult> AddBook([FromBody] BookDto bookDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                await _booksLogic.AddBook(bookDto.Name!, bookDto.Author!, bookDto.Genre!, bookDto.AmountInStock);

                return Ok();
            }
            catch (System.InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (System.ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch
            {
                return StatusCode(500);
            }
        }
    
        [HttpPut]
        [Route("books/{id}")]
        public async Task<IActionResult> UpdateBook(int id, [FromBody] BookDto bookDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                await _booksLogic.UpdateBook(id, bookDto.Name!, bookDto.Author!, bookDto.Genre!, bookDto.AmountInStock);

                return Ok();
            }
            catch (System.ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch
            {
                return StatusCode(500);
            }
        }

        [HttpDelete]
        [Route("books")]
        public async Task<IActionResult> DeleteBook([FromQuery(Name = "Name")] string name)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                await _booksLogic.DeleteBook(name);

                return Ok();
            }
            catch (System.ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch
            {
                return StatusCode(500);
            }
        }

        [HttpDelete]
        [Route("books/{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                await _booksLogic.DeleteBook(id);

                return Ok();
            }
            catch (System.ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch
            {
                return StatusCode(500);
            }
        }
    
        [HttpPut]
        [Route("books/{id}/stock")]
        public async Task<IActionResult> UpdateBookStock(int id, [FromQuery(Name = "Amount")] int amount)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                await _booksLogic.AddBookCopies(id, amount);

                return Ok();
            }
            catch (System.ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch
            {
                return StatusCode(500);
            }
        }
    
        [HttpDelete]
        [Route("books/{id}/stock")]
        public async Task<IActionResult> RemoveBookStock(int id, [FromQuery(Name = "Amount")] int amount)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                await _booksLogic.RemoveBookCopies(id, amount);

                return Ok();
            }
            catch (System.ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch
            {
                return StatusCode(500);
            }
        }
    }
}