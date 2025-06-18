using Legion.Transactions;

namespace Legion.EntityFrameworkCore.Database.Transactions;

public class DbContextManager : TransactionManagerBase
{
	private readonly IDbContext _dbContext;

	private bool _disposed;

	public override bool IsDisposed => _disposed;

	public DbContextManager(IDbContext dbContext)
	{
		Throw.IfArgumentNull(dbContext);

		_dbContext = dbContext;
	}

	public override void Commit(IScopeContext scopeContext)
	{
		IsCommitted = true;
		_dbContext.Save(scopeContext);
	}

	public override Task CommitAsync(IScopeContext scopeContext, CancellationToken cancellationToken)
#if NET5_0_OR_GREATER
	{
		IsCommitted = true;
		return _dbContext.SaveAsync(scopeContext, cancellationToken);
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
	}

	public override Task RollbackAsync(IScopeContext scopeContext, Exception? exception, CancellationToken cancellationToken)
	{
		IsRolledBack = true;
		return Task.CompletedTask;
	}


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
		return (_dbContext as T)!;
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
		=> _dbContext.DisposeAsync();
#endif

	protected virtual void Dispose(bool disposing)
	{
		if (_disposed)
			return;

		_disposed = true;

		if (disposing)
		{
			_dbContext.Dispose();
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

		if (obj is not DbContextManager otherDbContextManager)
		{
			return false;
		}

		return _dbContext.Equals(otherDbContextManager._dbContext);
	}

	public override int GetHashCode()
		=> _dbContext.GetHashCode();

	public static bool operator ==(DbContextManager? first, DbContextManager? second)
	{
		if (first is null && second is null)
			return true;

		if (first is null || second is null)
			return false;

		return first.Equals(second);
	}

	public static bool operator !=(DbContextManager? first, DbContextManager? second)
		=> !(first == second);
}
