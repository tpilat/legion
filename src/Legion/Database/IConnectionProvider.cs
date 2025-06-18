using Legion.Caching;
using Legion.Model.Audit;
using Legion.Model.Messaging;
using Legion.Model.Repositories;
using Legion.Transactions;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Data.Common;
using System.Runtime.CompilerServices;

namespace Legion.Database;

public interface IConnectionProvider : IDisposable, IAsyncDisposable
{
	IServiceProvider ServiceProvider { get; }
	ITransactionsController? TransactionsController { get; }
	IUnitOfWorkProvider UnitOfWorkProvider { get; }
	string? ConnectionString { get; }
	DbConnection? DbConnection { get; }
	bool WithTransaction { get; }
	IsolationLevel? TransactionIsolationLevel { get; }
	DbTransaction? ExternalDbTransaction { get; }
	ILogger Logger { get; }

	bool? AllowLocking { get; }
	bool CreateAuditEntryStore { get; }

	IAuditEntryStore? AuditEntryStore { get; }
	IDomainEventStore? DomainEventStore { get; }
	IReloadableCacheKeyStore? ReloadableCacheKeyStore { get; }

	IReloadableCacheKeyStore? GetOrCreateReloadableCacheKeyStore();

	bool RegisterDisposable(IDisposable disposable);

	bool ReCreateTransaction(IScopeContext scopeContext);

	DbConnection? GetDbConnection();

	DbConnection GetOrCreateNewDbConnection(out bool isNewConnection);

	DbConnection CreateNewDbConnection();

	IResult<bool?> CommitAll(
		IScopeContext scopeContext,
		TransactionsControllerStatus throwInvalidStatuses = TransactionsControllerStatus.NotIdle,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0);

	Task<IResult<bool?>> CommitAllAsync(
		IScopeContext scopeContext,
		TransactionsControllerStatus throwInvalidStatuses = TransactionsControllerStatus.NotIdle,
		CancellationToken cancellationToken = default,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0);

	IResult<bool?> RollbackAll(
		IScopeContext scopeContext,
		Exception? exception,
		TransactionsControllerStatus throwInvalidStatuses = TransactionsControllerStatus.CommitInProgress,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0);

	Task<IResult<bool?>> RollbackAllAsync(
		IScopeContext scopeContext,
		Exception? exception,
		TransactionsControllerStatus throwInvalidStatuses = TransactionsControllerStatus.CommitInProgress,
		CancellationToken cancellationToken = default,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0);
}
