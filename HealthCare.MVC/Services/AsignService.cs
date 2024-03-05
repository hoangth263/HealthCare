using HealthCare.MVC.Data;
using HealthCare.MVC.Entities;
using HealthCare.MVC.Repositories.IRepositories;
using HealthCare.MVC.Services.IServices;
using System.Linq.Expressions;

namespace HealthCare.MVC.Services
{
    public class AsignService : IAsignService
    {
        private IUnitOfWork _unitOfWork;
        private IAsignRepository _asignRepository;
        public AsignService(IUnitOfWork unitOfWork, IAsignRepository asignRepository)
        {
            _unitOfWork = unitOfWork;
            _asignRepository = asignRepository;
        }
        public async Task AddAsync(Asign asign)
        {
            await _asignRepository.AddAsync(asign);
        }

        public async Task AddRangce(IEnumerable<Asign> asigns)
        {
            await _asignRepository.AddRangce(asigns);
        }

        public async Task<Asign> FindAsync(int id)
        {
            return await _asignRepository.FindAsync(id);
        }

        public IQueryable<Asign> GetAll()
        {
            return _asignRepository.GetAll();
        }

        public IQueryable<Asign> Get(Expression<Func<Asign, bool>> where)
        {
            return _asignRepository.Get(where);
        }

        public IQueryable<Asign> Get(Expression<Func<Asign, bool>> where, params Expression<Func<Asign, object>>[] includes)
        {
            return _asignRepository.Get(where, includes);
        }


        public async Task<bool> Remove(int id)
        {
            return await _asignRepository.Remove(id);
        }

        public void Update(Asign asign)
        {
            _asignRepository.Update(asign);
        }

        public async Task<bool> SaveChangeAsync()
        {
            return await _unitOfWork.SaveChangeAsync();
        }
    }
}
