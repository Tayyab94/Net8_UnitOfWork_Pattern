using Microsoft.EntityFrameworkCore;
using UnitOfWorkPractice.Models;

namespace UnitOfWorkPractice.Repos
{
    public class BaseRepository<T>: IRepository<T> where T : class
    {
        protected readonly AppDbContext _context;
        protected readonly DbSet<T> _dbSet;


        protected BaseRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        // Common CRUD operations
        public async virtual  Task<T> GetByIdAsync(object id) => await _dbSet.FindAsync(id);

        public virtual async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }
    }
}
