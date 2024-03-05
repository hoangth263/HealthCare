using HealthCare.MVC.Data;
using HealthCare.MVC.Entities;
using HealthCare.MVC.Repositories.IRepositories;
using HealthCare.MVC.Services.IServices;
using System.Linq.Expressions;

namespace HealthCare.MVC.Services
{
    public class CustomerService : ICustomerService
    {
        private IUnitOfWork _unitOfWork;
        private ICustomerRepository _customerRepository;
        public CustomerService(IUnitOfWork unitOfWork, ICustomerRepository customerRepository)
        {
            _unitOfWork = unitOfWork;
            _customerRepository = customerRepository;
        }
        public async Task AddAsync(Customer customer)
        {
            await _customerRepository.AddAsync(customer);
        }

        public async Task AddRangce(IEnumerable<Customer> customers)
        {
            await _customerRepository.AddRangce(customers);
        }

        public async Task<Customer> FindAsync(int id)
        {
            return await _customerRepository.FindAsync(id);
        }

        public IQueryable<Customer> GetAll()
        {
            return _customerRepository.GetAll();
        }

        public IQueryable<Customer> Get(Expression<Func<Customer, bool>> where)
        {
            return _customerRepository.Get(where);
        }

        public IQueryable<Customer> Get(Expression<Func<Customer, bool>> where, params Expression<Func<Customer, object>>[] includes)
        {
            return _customerRepository.Get(where, includes);
        }


        public async Task<bool> Remove(int id)
        {
            return await _customerRepository.Remove(id);
        }

        public void Update(Customer customer)
        {
            _customerRepository.Update(customer);
        }

        public async Task<bool> SaveChangeAsync()
        {
            return await _unitOfWork.SaveChangeAsync();
        }
    }
}
