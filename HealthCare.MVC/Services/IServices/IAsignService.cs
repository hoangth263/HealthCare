using HealthCare.MVC.Entities;
using System.Linq.Expressions;

namespace HealthCare.MVC.Services.IServices
{
    public interface IAsignService
    {
        Task<Asign> FindAsync(int id);
        IQueryable<Asign> GetAll();
        IQueryable<Asign> Get(Expression<Func<Asign, bool>> where);
        IQueryable<Asign> Get(Expression<Func<Asign, bool>> where, params Expression<Func<Asign, object>>[] includes);
        Task AddAsync(Asign asign);
        Task AddRangce(IEnumerable<Asign> asigns);
        void Update(Asign asign);
        Task<bool> Remove(int id);
        Task<bool> SaveChangeAsync();
    }
}
