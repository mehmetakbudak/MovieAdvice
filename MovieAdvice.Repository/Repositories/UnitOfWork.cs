using System;
using System.Threading.Tasks;

namespace MovieAdvice.Repository.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        void BeginTransaction();
        void CommitTransaction();
        void RollbackTransaction();
        int Save();
        Task<int> SaveAsync();
    }

    public class UnitOfWork : IUnitOfWork
    {
        private readonly MovieAdviceContext _context;

        public UnitOfWork(MovieAdviceContext context)
        {
            _context = context;
        }

        public void BeginTransaction()
        {
            if (_context.Database.CurrentTransaction != null)
                _context.Database.BeginTransaction();
        }

        public void CommitTransaction()
        {
            if (_context.Database.CurrentTransaction != null)
            {
                _context.Database.CommitTransaction();
            }
        }

        public void RollbackTransaction()
        {
            if (_context.Database.CurrentTransaction != null)
            {
                _context.Database.RollbackTransaction();
            }
        }

        public int Save()
        {
            return _context.SaveChanges();
        }

        public Task<int> SaveAsync()
        {
            return _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}