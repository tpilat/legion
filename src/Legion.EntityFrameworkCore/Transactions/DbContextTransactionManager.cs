using Legion.Transactions;
using Microsoft.EntityFrameworkCore.Storage;

namespace Legion.EntityFrameworkCore.Database.Transactions;

public class DbContextTransactionManager : TransactionManagerBase
{
	private readonly IDbContextTransaction _dbContextTransaction;

	private bool _disposed;

	public override bool IsDisposed => _disposed;

	public DbContextTransactionManager(IDbContextTransaction dbContextTransaction)
	{
		Throw.IfArgumentNull(dbContextTransaction);

		_dbContextTransaction = dbContextTransaction;
	}

	public override void Commit(IScopeContext scopeContext)
	{
		IsCommitted = true;
		_dbContextTransaction.Commit();
	}

	public override Task CommitAsync(IScopeContext scopeContext, CancellationToken cancellationToken)
#if NET5_0_OR_GREATER
	{
		IsCommitted = true;
		return _dbContextTransaction.CommitAsync(cancellationToken);
	}
#else
	{
		IsCommitted = true;
		_transaction.Commit();
		return Task.CompletedTask;
	}
#endif

	public override void Rollback(IScopeContext scopeContext, Exception? exception)
	{
		IsRolledBack = true;
		_dbContextTransaction.Rollback();
	}

	public override Task RollbackAsync(IScopeContext scopeContext, Exception? exception, CancellationToken cancellationToken)
#if NET5_0_OR_GREATER
	{
		IsRolledBack = true;
		return _dbContextTransaction.RollbackAsync(cancellationToken);
	}
#else
	{
		IsRolledBack = true;
		_transaction.Rollback();
		return Task.CompletedTask;
	}
#endif


	public override void PreCommit(IScopeContext scopeContext)
	{
	}

	public override void PostCommit(IScopeContext scopeContext)
	{
	}

	public override void PreRollback(IScopeContext scopeContext, Exception? exception)
	{
	}

	public override void PostRollback(IScopeContext scopeContext, Exception? exception)
	{
	}

	public override Task PreCommitAsync(IScopeContext scopeContext, CancellationToken cancellationToken)
		=> Task.CompletedTask;

	public override Task PostCommitAsync(IScopeContext scopeContext, CancellationToken cancellationToken)
		=> Task.CompletedTask;

	public override Task PreRollbackAsync(IScopeContext scopeContext, Exception? exception, CancellationToken cancellationToken)
		=> Task.CompletedTask;

	public override Task PostRollbackAsync(IScopeContext scopeContext, Exception? exception, CancellationToken cancellationToken)
		=> Task.CompletedTask;

	public override T GetUnderlyingTransaction<T>()
		where T : class
	{
		return (_dbContextTransaction as T)!;
	}

	public override async ValueTask DisposeAsync()
	{
		if (_disposed)
			return;

		_disposed = true;

#if NET5_0_OR_GREATER
		await DisposeAsyncCoreAsync().ConfigureAwait(false);
#else
		_transaction.Dispose();
#endif

		Dispose(disposing: false);
		GC.SuppressFinalize(this);
	}

#if NET5_0_OR_GREATER
	protected virtual ValueTask DisposeAsyncCoreAsync()
		=> _dbContextTransaction.DisposeAsync();
#endif

	protected virtual void Dispose(bool disposing)
	{
		if (_disposed)
			return;

		_disposed = true;

		if (disposing)
		{
			_dbContextTransaction.Dispose();
		}
	}

	public override void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	public override bool Equals(object? obj)
	{
		if (obj is null)
			return false;

		if (GetType() != obj.GetType())
			return false;

		if (obj is not DbContextTransactionManager otherDbContextTransactionManager)
		{
			return false;
		}

		return _dbContextTransaction.Equals(otherDbContextTransactionManager._dbContextTransaction);
	}

	public override int GetHashCode()
		=> _dbContextTransaction.GetHashCode();

	public static bool operator ==(DbContextTransactionManager? first, DbContextTransactionManager? second)
	{
		if (first is null && second is null)
			return true;

		if (first is null || second is null)
			return false;

		return first.Equals(second);
	}

	public static bool operator !=(DbContextTransactionManager? first, DbContextTransactionManager? second)
		=> !(first == second);
}
