using HealthCare.MVC.Data;
using HealthCare.MVC.Entities;
using HealthCare.MVC.Repositories.IRepositories;
using HealthCare.MVC.Services.IServices;
using System.Linq.Expressions;

namespace HealthCare.MVC.Services
{
    public class AgentService : IAgentService
    {
        private IUnitOfWork _unitOfWork;
        private IAgentRepository _agentRepository;
        public AgentService(IUnitOfWork unitOfWork, IAgentRepository agentRepository)
        {
            _unitOfWork = unitOfWork;
            _agentRepository = agentRepository;
        }

        public async Task<Agent?> Login(string Email, string Password)
        {
            var agent = _agentRepository.Get(x => x.Email == Email).FirstOrDefault();
            if (agent != null)
            {
                if (PasswordManager.VerifyPassword(Password, agent.Password))
                {
                    return agent;
                }
            }
            return null;
        }

        public async Task AddAsync(Agent agent)
        {
            agent.Password = EncryptPassword(agent.Password);
            agent.CreatedDate = DateTime.Now;
            agent.UpdatedDate = DateTime.Now;
            await _agentRepository.AddAsync(agent);
        }

        public async Task AddRangce(IEnumerable<Agent> agents)
        {
            await _agentRepository.AddRangce(agents);
        }

        public async Task<Agent> FindAsync(int id)
        {
            return await _agentRepository.FindAsync(id);
        }

        public IQueryable<Agent> GetAll()
        {
            return _agentRepository.GetAll();
        }

        public IQueryable<Agent> Get(Expression<Func<Agent, bool>> where)
        {
            return _agentRepository.Get(where);
        }

        public IQueryable<Agent> Get(Expression<Func<Agent, bool>> where, params Expression<Func<Agent, object>>[] includes)
        {
            return _agentRepository.Get(where, includes);
        }


        public async Task<bool> Remove(int id)
        {
            return await _agentRepository.Remove(id);
        }

        public void Update(Agent agent)
        {
            agent.Password = EncryptPassword(agent.Password);
            _agentRepository.Update(agent);
        }

        public async Task<bool> SaveChangeAsync()
        {
            return await _unitOfWork.SaveChangeAsync();
        }

        public string EncryptPassword(string password)
        {
            return PasswordManager.HashPassword(password);
        }
    }
}
