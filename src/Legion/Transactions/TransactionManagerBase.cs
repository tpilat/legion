namespace Legion.Transactions;

public abstract class TransactionManagerBase : ITransactionManager, IDisposable, IAsyncDisposable
{
	public TransactionsController? TransactionsController { get; private set; }
	public bool IsCommitted { get; protected set; }
	public bool IsRolledBack { get; protected set; }
	public abstract bool IsDisposed { get; }

	public abstract void Commit(IScopeContext scopeContext);

	public abstract Task CommitAsync(IScopeContext scopeContext, CancellationToken cancellationToken);

	public abstract void Rollback(IScopeContext scopeContext, Exception? exception);

	public abstract Task RollbackAsync(IScopeContext scopeContext, Exception? exception, CancellationToken cancellationToken);


	public abstract void PreCommit(IScopeContext scopeContext);
	public abstract void PostCommit(IScopeContext scopeContext);
	public abstract void PreRollback(IScopeContext scopeContext, Exception? exception);
	public abstract void PostRollback(IScopeContext scopeContext, Exception? exception);

	public abstract Task PreCommitAsync(IScopeContext scopeContext, CancellationToken cancellationToken);
	public abstract Task PostCommitAsync(IScopeContext scopeContext, CancellationToken cancellationToken);
	public abstract Task PreRollbackAsync(IScopeContext scopeContext, Exception? exception, CancellationToken cancellationToken);
	public abstract Task PostRollbackAsync(IScopeContext scopeContext, Exception? exception, CancellationToken cancellationToken);

	public virtual void SetTransactionsController(TransactionsController? transactionsController)
	{
		TransactionsController = transactionsController;
	}

	public abstract T GetUnderlyingTransaction<T>()
		where T : class;

	public abstract void Dispose();

	public abstract ValueTask DisposeAsync();
}
