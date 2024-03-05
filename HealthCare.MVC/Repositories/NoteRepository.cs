using HealthCare.MVC.Data;
using HealthCare.MVC.Entities;
using HealthCare.MVC.Repositories.IRepositories;

namespace HealthCare.MVC.Repositories
{
    public class NoteRepository : BaseRepository<Note, int>, INoteRepository
    {
        public NoteRepository(HealthCareContext context) : base(context)
        {

        }
    }
}
