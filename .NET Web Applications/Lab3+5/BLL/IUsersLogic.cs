using DAL;
using DAL.Model;
using DAL.Repository;

namespace BLL
{
   public interface IUsersLogic
   {
      Task<User> AddUser(string name);
      Task<User> UpdateUser(int id, string name);
      Task DeleteUser(int id);
      User GetUser(int id);
      IEnumerable<User> GetAllUsers();
      Task AddBook(int userId, int bookId);
      IEnumerable<UserBook> GetUserBooks(int userId);
      Task RemoveBook(int userId, int bookId);
   }
}