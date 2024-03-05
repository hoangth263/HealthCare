using HealthCare.MVC.Entities;
using System.Linq.Expressions;

namespace HealthCare.MVC.Services.IServices
{
    public interface INoteService
    {
        Task<Note> FindAsync(int id);
        IQueryable<Note> GetAll();
        IQueryable<Note> Get(Expression<Func<Note, bool>> where);
        IQueryable<Note> Get(Expression<Func<Note, bool>> where, params Expression<Func<Note, object>>[] includes);
        Task AddAsync(Note note);
        Task AddRangce(IEnumerable<Note> notes);
        void Update(Note note);
        Task<bool> Remove(int id);
        Task<bool> SaveChangeAsync();
    }
}
