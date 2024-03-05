using HealthCare.MVC.Entities;
using System.Linq.Expressions;

namespace HealthCare.MVC.Services.IServices
{
    public interface ICustomerService
    {
        Task<Customer> FindAsync(int id);
        IQueryable<Customer> GetAll();
        IQueryable<Customer> Get(Expression<Func<Customer, bool>> where);
        IQueryable<Customer> Get(Expression<Func<Customer, bool>> where, params Expression<Func<Customer, object>>[] includes);
        Task AddAsync(Customer customer);
        Task AddRangce(IEnumerable<Customer> customers);
        void Update(Customer customer);
        Task<bool> Remove(int id);
        Task<bool> SaveChangeAsync();
    }
}
