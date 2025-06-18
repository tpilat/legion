namespace Legion.Transactions;

public interface ITransactionManager : IDisposable, IAsyncDisposable
{
	TransactionsController? TransactionsController { get; }
	bool IsCommitted { get; }
	bool IsRolledBack { get; }
	bool IsDisposed { get; }

	void Commit(IScopeContext scopeContext);

	Task CommitAsync(IScopeContext scopeContext, CancellationToken cancellationToken);

	void Rollback(IScopeContext scopeContext, Exception? exception);

	Task RollbackAsync(IScopeContext scopeContext, Exception? exception, CancellationToken cancellationToken);


	void PreCommit(IScopeContext scopeContext);
	void PostCommit(IScopeContext scopeContext);
	void PreRollback(IScopeContext scopeContext, Exception? exception);
	void PostRollback(IScopeContext scopeContext, Exception? exception);

	Task PreCommitAsync(IScopeContext scopeContext, CancellationToken cancellationToken);
	Task PostCommitAsync(IScopeContext scopeContext, CancellationToken cancellationToken);
	Task PreRollbackAsync(IScopeContext scopeContext, Exception? exception, CancellationToken cancellationToken);
	Task PostRollbackAsync(IScopeContext scopeContext, Exception? exception, CancellationToken cancellationToken);

	void SetTransactionsController(TransactionsController? transactionsController);

	T GetUnderlyingTransaction<T>()
		where T : class;
}
