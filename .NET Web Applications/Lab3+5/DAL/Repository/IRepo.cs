using System.Threading.Tasks;

namespace DAL.Repository
{
    // a generic representation of a repository
    // it is used to make access to the data 
    // regardless of the data type and storage (database, file, etc.)
    public interface IRepo<T>
    {
        IEnumerable<T> GetAll();
        T GetById(int id);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        Task SaveAsync();
    }
}