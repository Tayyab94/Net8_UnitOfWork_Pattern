using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;
using System.Transactions;
using UnitOfWorkPractice.Models;

namespace UnitOfWorkPractice.Repos.Unit
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private readonly ILogger<UnitOfWork> _logger;
        private  IDbContextTransaction _transaction;
        private bool _disposed;


        public UnitOfWork(AppDbContext context, ILogger<UnitOfWork> logger)
        {
                _context= context ?? throw new ArgumentNullException(nameof(context));
                _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        private IUserRepository _userRepo;
        private IProductRepository _productRepo;




        public IUserRepository UserRepository => _userRepo ??= new UserRepository(_context);
       
        public IProductRepository ProductRepository => _productRepo ??= new ProductRepository(_context);

        public async Task BeginTransactionAsync(System.Transactions.IsolationLevel isolationLevel = System.Transactions.IsolationLevel.ReadCommitted)
        {
            if (_transaction != null)
            {
                throw new InvalidOperationException("A transaction is already in progress");
            }

           
            _transaction = await _context.Database.BeginTransactionAsync();
            _logger.LogInformation("Transaction started with isolation level {IsolationLevel}", isolationLevel);

        }

        public Task<bool> CanConnectAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task CommitTransactionAsync()
        {
            if (_transaction == null)
            {
                throw new InvalidOperationException("No transaction to commit");
            }

            try
            {
                await _transaction.CommitAsync();
                _logger.LogInformation("Transaction committed successfully");
            }
            finally
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _transaction?.Dispose();
                    _context.Dispose();
                }
                _disposed = true;
            }
        }

        public async ValueTask DisposeAsync()
        {
            await DisposeAsyncCore();
            Dispose(false);
            GC.SuppressFinalize(this);
        }

        protected virtual async ValueTask DisposeAsyncCore()
        {
            if (_transaction != null)
            {
                await _transaction.DisposeAsync();
            }
            await _context.DisposeAsync();
        }

        public async Task RollbackTransactionAsync()
        {
            if (_transaction == null)
            {
                _logger.LogWarning("Attempted to rollback a non-existent transaction");
                return;
            }

            try
            {
                await _transaction.RollbackAsync();
                _logger.LogInformation("Transaction rolled back successfully");

                // Detach all tracked entities after rollback
                foreach (var entry in _context.ChangeTracker.Entries()
                    .Where(e => e.State != EntityState.Unchanged))
                {
                    entry.State = EntityState.Detached;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to rollback transaction");
                throw new TransactionException("Transaction rollback failed", ex);
            }
            finally
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public Task<int> SaveChangesCountAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<int> SaveChangesWithAuditAsync(string userId, CancellationToken cancellationToken = default)
        {
         
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentNullException(nameof(userId));

            // Auto-set audit fields on entities
            //var auditables = _context.ChangeTracker.Entries()
            //    .Where(e => e.Entity is IAuditable &&
            //               (e.State == EntityState.Added || e.State == EntityState.Modified));

            var now = DateTime.UtcNow;
            //foreach (var entry in auditables)
            //{
            //    var auditable = (IAuditable)entry.Entity;
            //    if (entry.State == EntityState.Added)
            //    {
            //        auditable.CreatedDate = now;
            //        auditable.CreatedBy = userId;
            //    }
            //    auditable.ModifiedDate = now;
            //    auditable.ModifiedBy = userId;
            //}

            // Save within transaction if one exists
            if (_transaction != null)
            {
                return await SaveChangesAsync(cancellationToken);
            }

            // Otherwise create new transaction
            await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                int result = await SaveChangesCountAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);
                return result;
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }

        
        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
            //try
            //{
            //    // Validate entities before save
            //    var errors = _context.GetValidationErrors();
            //    if (errors.Any())
            //    {
            //        throw new DataValidationException("Entity validation failed", errors);
            //    }

            //    int result = await _context.SaveChangesAsync(cancellationToken);
            //    _logger.LogDebug("Saved {Count} changes to database", result);
            //    return result;
            //}
            //catch (DbUpdateConcurrencyException ex)
            //{
            //    _logger.LogWarning(ex, "Concurrency violation detected");
            //    throw new ConcurrencyException("The record was modified by another user", ex);
            //}
            //catch (DbUpdateException ex)
            //{
            //    _logger.LogError(ex, "Database update failed");
            //    throw new DataAccessException("Database update failed", ex);
            //}
        }

    
    }
}
