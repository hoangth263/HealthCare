using HealthCare.MVC.Data;
using HealthCare.MVC.Entities;
using HealthCare.MVC.Repositories.IRepositories;

namespace HealthCare.MVC.Repositories
{
    public class AsignRepository : BaseRepository<Asign, int>, IAsignRepository
    {
        public AsignRepository(HealthCareContext context) : base(context)
        {

        }
    }
}
