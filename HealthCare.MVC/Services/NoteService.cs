using HealthCare.MVC.Data;
using HealthCare.MVC.Entities;
using HealthCare.MVC.Repositories.IRepositories;
using HealthCare.MVC.Services.IServices;
using System.Linq.Expressions;

namespace HealthCare.MVC.Services
{
    public class NoteService : INoteService
    {
        private IUnitOfWork _unitOfWork;
        private INoteRepository _noteRepository;
        public NoteService(IUnitOfWork unitOfWork, INoteRepository noteRepository)
        {
            _unitOfWork = unitOfWork;
            _noteRepository = noteRepository;
        }
        public async Task AddAsync(Note note)
        {
            await _noteRepository.AddAsync(note);
        }

        public async Task AddRangce(IEnumerable<Note> notes)
        {
            await _noteRepository.AddRangce(notes);
        }

        public async Task<Note> FindAsync(int id)
        {
            return await _noteRepository.FindAsync(id);
        }

        public IQueryable<Note> GetAll()
        {
            return _noteRepository.GetAll();
        }

        public IQueryable<Note> Get(Expression<Func<Note, bool>> where)
        {
            return _noteRepository.Get(where);
        }

        public IQueryable<Note> Get(Expression<Func<Note, bool>> where, params Expression<Func<Note, object>>[] includes)
        {
            return _noteRepository.Get(where, includes);
        }


        public async Task<bool> Remove(int id)
        {
            return await _noteRepository.Remove(id);
        }

        public void Update(Note note)
        {
            note.UpdatedDate = DateTime.Now;
            _noteRepository.Update(note);
        }

        public async Task<bool> SaveChangeAsync()
        {
            return await _unitOfWork.SaveChangeAsync();
        }
    }
}
