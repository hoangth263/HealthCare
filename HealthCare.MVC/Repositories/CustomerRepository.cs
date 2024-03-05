using HealthCare.MVC.Data;
using HealthCare.MVC.Entities;
using HealthCare.MVC.Repositories.IRepositories;

namespace HealthCare.MVC.Repositories
{
    public class CustomerRepository : BaseRepository<Customer, int>, ICustomerRepository
    {
        public CustomerRepository(HealthCareContext context) : base(context)
        {

        }
    }
}
