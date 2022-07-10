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
    public class UsersController : ControllerBase
    {
        private readonly IUsersLogic _usersLogic;

        public UsersController(IUsersLogic usersLogic)
        {
            _usersLogic = usersLogic;
        }

        [HttpGet]
        [Route("users")]
        public IActionResult GetUsers()
        {
            try
            {
                var users = _usersLogic.GetAllUsers();
                var res = new List<UserDto>();

                foreach (var user in users)
                {
                    res.Add(new UserDto(user));
                }

                return Ok(res);
            }
            catch
            {
                return StatusCode(500);
            }
        }

        [HttpGet]
        [Route("users/{id}")]
        public IActionResult GetUser(int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var user = _usersLogic.GetUser(id);
                var userBooks = _usersLogic.GetUserBooks(id);
                var res = new UserDto(user, userBooks);

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
        [Route("users")]
        public async Task<IActionResult> AddUser([FromBody] UserDto user)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                await _usersLogic.AddUser(user.Name!);

                return Ok();
            }
            catch
            {
                return StatusCode(500);
            }
        }

        [HttpPut]
        [Route("users/{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserDto user)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                await _usersLogic.UpdateUser(id, user.Name!);

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
        [Route("users/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                await _usersLogic.DeleteUser(id);

                return Ok();
            }
            catch (System.ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }
    
        [HttpPut]
        [Route("users/{id}/books/{bookId}")]
        public async Task<IActionResult> AddBook(int id, int bookId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                await _usersLogic.AddBook(id, bookId);

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
        [Route("users/{id}/books/{bookId}")]
        public async Task<IActionResult> DeleteBook(int id, int bookId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                await _usersLogic.RemoveBook(id, bookId);

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