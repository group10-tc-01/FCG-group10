using FCG.Domain.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace FCG.Infrastructure.Persistance.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly FcgDbContext _dbContext;
        private IDbContextTransaction _transaction;
        public UnitOfWork(FcgDbContext dbContext, IDbContextTransaction transaction)
        {
            _dbContext = dbContext;
            _transaction = transaction;
        }

        public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            _transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
        }

        public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
        {
            var result = await _dbContext.SaveChangesAsync(cancellationToken);
            if (_transaction != null)
            {
                await _transaction.CommitAsync(cancellationToken);
            }

            return result;
        }

        public async Task RollbackAsync(CancellationToken cancellationToken = default)
        {
            await _transaction.RollbackAsync(cancellationToken);
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _dbContext?.Dispose();
        }
    }
}
