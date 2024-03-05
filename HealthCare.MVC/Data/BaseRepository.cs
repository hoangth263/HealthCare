using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace HealthCare.MVC.Data
{
    public interface IBaseRepository<T, TKey> where T : class
    {
        Task<T> FindAsync(TKey id);
        IQueryable<T> GetAll();
        IQueryable<T> Get(Expression<Func<T, bool>> where);
        IQueryable<T> Get(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] includes);
        Task AddAsync(T entity);
        Task AddRangce(IEnumerable<T> entities);
        void Update(T entity);
        Task<bool> Remove(TKey id);
        Task<bool> Remove(T entity);
    }
    public class BaseRepository<T, Tkey> : IBaseRepository<T, Tkey> where T : class
    {
        private HealthCareContext _contex;
        private DbSet<T> dbSet;
        public BaseRepository(HealthCareContext applicationDbContext)
        {
            _contex = applicationDbContext;
            dbSet = _contex.Set<T>();
        }
        public virtual async Task AddAsync(T entity)
        {
            await dbSet.AddAsync(entity);
        }

        public virtual async Task AddRangce(IEnumerable<T> entities)
        {
            await dbSet.AddRangeAsync(entities);
        }

        public virtual async Task<T> FindAsync(Tkey id)
        {
            return await dbSet.FindAsync(id);
        }

        public virtual IQueryable<T> GetAll()
        {
            return dbSet;
        }

        public virtual IQueryable<T> Get(Expression<Func<T, bool>> where)
        {
            return dbSet.Where(where);
        }

        public virtual IQueryable<T> Get(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] includes)
        {
            var result = dbSet.Where(where);
            foreach (var include in includes)
            {
                result = result.Include(include);
            }
            return result;
        }

        public virtual async Task<bool> Remove(Tkey id)
        {
            var entity = await dbSet.FindAsync(id);
            if (entity == null)
            {
                return false;
            }
            dbSet.Remove(entity);
            return true;
        }

        public virtual async Task<bool> Remove(T entity)
        {
            if (entity == null)
            {
                return false;
            }
            dbSet.Remove(entity);
            return true;
        }

        public virtual void Update(T entity)
        {
            _contex.Entry(entity).State = EntityState.Modified;
        }
    }
}