
using System.Transactions;

namespace UnitOfWorkPractice.Repos.Unit
{
    public interface IUnitOfWork: IDisposable, IAsyncDisposable
    {
        IUserRepository UserRepository { get; }
        IProductRepository ProductRepository { get; }

        Task BeginTransactionAsync(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();


        // Save changes with optional audit
        Task<int> SaveChangesWithAuditAsync(string userId,CancellationToken cancellationToken= default);
        Task<int> SaveChangesCountAsync(CancellationToken cancellationToken= default);


        // Health check
        Task<bool> CanConnectAsync(CancellationToken cancellationToken = default);
    }
}
