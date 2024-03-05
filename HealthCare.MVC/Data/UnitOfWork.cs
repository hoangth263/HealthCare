namespace HealthCare.MVC.Data
{
    public interface IUnitOfWork
    {
        Task<bool> SaveChangeAsync();
    }
    public class UnitOfWork : IUnitOfWork
    {
        private HealthCareContext _context;
        public UnitOfWork(HealthCareContext context)
        {
            _context = context;
        }

        public async Task<bool> SaveChangeAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
