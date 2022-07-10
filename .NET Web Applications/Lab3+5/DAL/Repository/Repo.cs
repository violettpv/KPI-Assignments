using DAL.Model;
using DAL;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repository
{
    public class Repo<T> : IRepo<T> where T : class, IEntity
    {
        private LibraryDbContext _context;

        public Repo(IDbContextFactory<LibraryDbContext> contexts)
        {
            // each repo is going to hvae its own context in order to avoid
            // concurrency issues (pomelo nuget package flaw)
            _context = contexts.CreateDbContext();
        }

        public IEnumerable<T> GetAll()
        {
            return _context.Set<T>();
        }
        public T GetById(int id)
        {
            return _context.Set<T>().Where(b => b.Id == id).FirstOrDefault()!;
        }
        public void Add(T entity)
        {
            _context.Set<T>().Add(entity);
        }
        public void Update(T entity)
        {
            _context.Set<T>().Update(entity);
        }
        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}



/*

using DAL.Model;
using DAL;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repository
{
    public class Repo<T> : IRepo<T> where T : class, IEntity
    {
        private IDbContextFactory<LibraryDbContext> _contextFactory;

        public Repo(IDbContextFactory<LibraryDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public IEnumerable<T> GetAll()
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                return context.Set<T>();
            }
            
        }
        public T GetById(int id)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                return context.Set<T>().Where(b => b.Id == id).FirstOrDefault()!;
            }
        }
        public void Add(T entity)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                context.Set<T>().Add(entity);
                context.SaveChanges();
            }
        }
        public void Update(T entity)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                context.Set<T>().Update(entity);
                context.SaveChanges();
            }
        }
        public void Delete(T entity)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                context.Set<T>().Remove(entity);
                context.SaveChanges();
            }
        }

        public async Task SaveAsync()
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                await context.SaveChangesAsync();
            }
        }
    }
}

*/