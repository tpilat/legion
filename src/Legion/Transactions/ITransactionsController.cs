namespace Legion.Transactions;

public interface ITransactionsController : IDisposable, IAsyncDisposable
{
	Guid TransactionsControllerIdentifier { get; }
	TransactionsControllerStatus TransactionsControllerStatus { get; }

	void RegisterTransactionManager(IScopeContext scopeContext, ITransactionManager transactionManager);

	void UnregisterTransactionManager(IScopeContext scopeContext, ITransactionManager transactionManager);

	void SetRecreated();

	IResult CommitAll(IScopeContext scopeContext, TransactionsControllerStatus throwInvalidStatuses = TransactionsControllerStatus.NotIdle);

	Task<IResult> CommitAllAsync(IScopeContext scopeContext, TransactionsControllerStatus throwInvalidStatuses = TransactionsControllerStatus.NotIdle, CancellationToken cancellationToken = default);

	IResult RollbackAll(IScopeContext scopeContext, Exception? exception, TransactionsControllerStatus throwInvalidStatuses = TransactionsControllerStatus.CommitInProgress);

	Task<IResult> RollbackAllAsync(IScopeContext scopeContext, Exception? exception, TransactionsControllerStatus throwInvalidStatuses = TransactionsControllerStatus.CommitInProgress, CancellationToken cancellationToken = default);

	//IResult CommitAll(IScopeContext scopeContext, bool throwInvalidTransactionStatus);

	//Task<IResult> CommitAllAsync(IScopeContext scopeContext, bool throwInvalidTransactionStatus, CancellationToken cancellationToken = default);

	//IResult RollbackAll(IScopeContext scopeContext, Exception? exception, bool throwInvalidTransactionStatus);

	//Task<IResult> RollbackAllAsync(IScopeContext scopeContext, Exception? exception, bool throwInvalidTransactionStatus, CancellationToken cancellationToken = default);
}