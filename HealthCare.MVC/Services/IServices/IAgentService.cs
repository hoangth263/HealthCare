using HealthCare.MVC.Entities;
using System.Linq.Expressions;

namespace HealthCare.MVC.Services.IServices
{
    public interface IAgentService
    {
        Task<Agent> FindAsync(int id);
        Task<Agent> Login(string Email, string Password);
        IQueryable<Agent> GetAll();
        IQueryable<Agent> Get(Expression<Func<Agent, bool>> where);
        IQueryable<Agent> Get(Expression<Func<Agent, bool>> where, params Expression<Func<Agent, object>>[] includes);
        Task AddAsync(Agent agent);
        Task AddRangce(IEnumerable<Agent> agents);
        void Update(Agent agent);
        Task<bool> Remove(int id);
        Task<bool> SaveChangeAsync();
    }
}
