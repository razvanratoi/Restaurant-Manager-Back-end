using Microsoft.EntityFrameworkCore;
using RestaurantManager.Data;
using RestaurantManager.Repositories.Interfaces;

namespace RestaurantManager.Repositories
{
    public class GenericRepo<T> : IGenericRepo<T> where T : class
    {
        private readonly DataContext _context;
        public GenericRepo(DataContext context)
        {
            _context = context;
        }

        public async Task<bool> Add(T entity)
        {
            _context.Set<T>().Add(entity);
            return await SaveChanges();
        }

        public async Task<bool> Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
            return await SaveChanges();
        }

        public virtual async Task<IEnumerable<T>> GetAll()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public virtual async Task<T?> GetById(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<bool> SaveChanges()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> Update(T entity)
        {
            _context.Set<T>().Update(entity);
            return await SaveChanges();
        }
    }
}