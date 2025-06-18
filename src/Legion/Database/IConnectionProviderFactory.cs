using Legion.Transactions;
using System.Data;
using System.Data.Common;

namespace Legion.Database;

public interface IConnectionProviderFactory
{
	IConnectionProvider CreateWithoutTransactionByStoreId<TConnectionStringProvider>(
		IServiceProvider serviceProvider,
		string? storeId,
		bool? allowLocking,
		bool createAuditEntryStore)
		where TConnectionStringProvider : class, IConnectionStringProvider;

	IConnectionProvider CreateWithoutTransaction(
		IServiceProvider serviceProvider,
		string connectionString,
		bool? allowLocking,
		bool createAuditEntryStore);

	IConnectionProvider CreateWithoutTransaction(
		IServiceProvider serviceProvider,
		DbConnection dbConnection,
		bool? allowLocking,
		bool createAuditEntryStore);

	IConnectionProvider CreateWithNewTransactionByStoreId<TConnectionStringProvider>(
		IServiceProvider serviceProvider,
		string? storeId,
		IsolationLevel? transactionIsolationLevel,
		bool? allowLocking,
		bool createAuditEntryStore)
		where TConnectionStringProvider : class, IConnectionStringProvider;

	IConnectionProvider CreateWithNewTransactionByStoreId<TConnectionStringProvider>(
		IServiceProvider serviceProvider,
		string? storeId,
		ITransactionsController transactionsController,
		IsolationLevel? transactionIsolationLevel,
		bool? allowLocking,
		bool createAuditEntryStore)
		where TConnectionStringProvider : class, IConnectionStringProvider;

	IConnectionProvider CreateWithNewTransaction(
		IServiceProvider serviceProvider,
		string connectionString,
		IsolationLevel? transactionIsolationLevel,
		bool? allowLocking,
		bool createAuditEntryStore);

	IConnectionProvider CreateWithNewTransaction(
		IServiceProvider serviceProvider,
		string connectionString,
		ITransactionsController transactionsController,
		IsolationLevel? transactionIsolationLevel,
		bool? allowLocking,
		bool createAuditEntryStore);

	IConnectionProvider CreateWithNewTransaction(
		IServiceProvider serviceProvider,
		DbConnection dbConnection,
		IsolationLevel? transactionIsolationLevel,
		bool? allowLocking,
		bool createAuditEntryStore);

	IConnectionProvider CreateWithNewTransaction(
		IServiceProvider serviceProvider,
		DbConnection dbConnection,
		ITransactionsController transactionsController,
		IsolationLevel? transactionIsolationLevel,
		bool? allowLocking,
		bool createAuditEntryStore);

	IConnectionProvider CreateWithExistingTransaction(
		IScopeContext scopeContext,
		IServiceProvider serviceProvider,
		DbTransaction externalDbTransaction,
		bool? allowLocking,
		bool createAuditEntryStore);

	IConnectionProvider CreateWithExistingTransaction(
		IScopeContext scopeContext,
		IServiceProvider serviceProvider,
		DbTransaction externalDbTransaction,
		ITransactionsController transactionsController,
		bool? allowLocking,
		bool createAuditEntryStore);
}
