using Legion.EntityFrameworkCore.QueryCache;
using Legion.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Data.Common;

namespace Legion.EntityFrameworkCore;

public interface IDbContext : IDisposable, IAsyncDisposable
{
	ILogger DbContextLogger { get; }
	DbConnection DbConnection { get; }
	IDbContextTransaction? DbContextTransaction { get; }
	DbTransaction? DbTransaction { get; }
	DbContextId ContextId { get; }
	ChangeTracker ChangeTracker { get; }
	DatabaseFacade Database { get; }
	string DBConnectionString { get; }

	bool? WithAllowedLocking { get; }
	bool IsAuditDbContext { get; }
	bool IsDomainEventContext { get; }

	int Save(
		IScopeContext scopeContext);

	int Save(
		IScopeContext scopeContext,
		SaveOptions? options);

	int Save(
		IScopeContext scopeContext,
		bool acceptAllChangesOnSuccess);

	int Save(
		IScopeContext scopeContext,
		bool acceptAllChangesOnSuccess,
		SaveOptions? options);

	Task<int> SaveAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default);

	Task<int> SaveAsync(
		IScopeContext scopeContext,
		SaveOptions? options,
		CancellationToken cancellationToken = default);

	Task<int> SaveAsync(
		IScopeContext scopeContext,
		bool acceptAllChangesOnSuccess,
		CancellationToken cancellationToken = default);

	Task<int> SaveAsync(
		IScopeContext scopeContext,
		bool acceptAllChangesOnSuccess,
		SaveOptions? options,
		CancellationToken cancellationToken = default);

	void ConfigureQueryCacheManager(Action<QueryCacheManager> configure, bool force);

	void EnableQueryCacheManager();

	void Initialize(
		IDbContextSettintgs dbContextSettintgs,
		IEFConnectionProvider connectionProvider);

	void SetDbTransaction(
		IScopeContext scopeContext,
		IDbContextTransaction? existingDbContextTransaction,
		out IDbContextTransaction? newDbContextTransaction,
		TransactionUsage transactionUsage,
		IsolationLevel? transactionIsolationLevel);

	void SetDbTransaction(
		IScopeContext scopeContext,
		DbTransaction? existingTransaction,
		out IDbContextTransaction? newDbContextTransaction,
		TransactionUsage transactionUsage,
		IsolationLevel? transactionIsolationLevel);
}
