using HealthCare.MVC.Data;
using HealthCare.MVC.Entities;
using HealthCare.MVC.Repositories.IRepositories;

namespace HealthCare.MVC.Repositories
{
    public class AgentRepository : BaseRepository<Agent, int>, IAgentRepository
    {
        public AgentRepository(HealthCareContext context) : base(context)
        {

        }
    }
}
