using Legion.Database;
using Legion.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;
using System.Data.Common;

namespace Legion.EntityFrameworkCore;

public interface IEFConnectionProviderFactory : IConnectionProviderFactory
{
	Action<DbContextOptionsBuilder>? DefaultDbContextOptionsConfiguration { get; }

	IEFConnectionProvider CreateWithoutTransactionByStoreId<TConnectionStringProvider>(
		IServiceProvider serviceProvider,
		string? storeId,
		bool? allowLocking,
		bool createAuditEntryStore,
		Action<DbContextOptionsBuilder>? dbContextOptionsConfiguration = null)
		where TConnectionStringProvider : class, IConnectionStringProvider;

	IEFConnectionProvider CreateWithoutTransaction(
		IServiceProvider serviceProvider,
		string connectionString,
		bool? allowLocking,
		bool createAuditEntryStore,
		Action<DbContextOptionsBuilder>? dbContextOptionsConfiguration = null);

	IEFConnectionProvider CreateWithoutTransaction(
		IServiceProvider serviceProvider,
		DbConnection dbConnection,
		bool? allowLocking,
		bool createAuditEntryStore,
		Action<DbContextOptionsBuilder>? dbContextOptionsConfiguration = null);

	IEFConnectionProvider CreateWithNewTransactionByStoreId<TConnectionStringProvider>(
		IServiceProvider serviceProvider,
		string? storeId,
		ITransactionsController transactionsController,
		IsolationLevel? transactionIsolationLevel,
		bool? allowLocking,
		bool createAuditEntryStore,
		Action<DbContextOptionsBuilder>? dbContextOptionsConfiguration = null)
		where TConnectionStringProvider : class, IConnectionStringProvider;

	IEFConnectionProvider CreateWithNewTransactionByStoreId<TConnectionStringProvider>(
		IServiceProvider serviceProvider,
		string? storeId,
		IsolationLevel? transactionIsolationLevel,
		bool? allowLocking,
		bool createAuditEntryStore,
		Action<DbContextOptionsBuilder>? dbContextOptionsConfiguration = null)
		where TConnectionStringProvider : class, IConnectionStringProvider;

	IEFConnectionProvider CreateWithNewTransaction(
		IServiceProvider serviceProvider,
		string connectionString,
		IsolationLevel? transactionIsolationLevel,
		bool? allowLocking,
		bool createAuditEntryStore,
		Action<DbContextOptionsBuilder>? dbContextOptionsConfiguration = null);

	IEFConnectionProvider CreateWithNewTransaction(
		IServiceProvider serviceProvider,
		string connectionString,
		ITransactionsController transactionsController,
		IsolationLevel? transactionIsolationLevel,
		bool? allowLocking,
		bool createAuditEntryStore,
		Action<DbContextOptionsBuilder>? dbContextOptionsConfiguration = null);

	IEFConnectionProvider CreateWithNewTransaction(
		IServiceProvider serviceProvider,
		DbConnection dbConnection,
		IsolationLevel? transactionIsolationLevel,
		bool? allowLocking,
		bool createAuditEntryStore,
		Action<DbContextOptionsBuilder>? dbContextOptionsConfiguration = null);

	IEFConnectionProvider CreateWithNewTransaction(
		IServiceProvider serviceProvider,
		DbConnection dbConnection,
		ITransactionsController transactionsController,
		IsolationLevel? transactionIsolationLevel,
		bool? allowLocking,
		bool createAuditEntryStore,
		Action<DbContextOptionsBuilder>? dbContextOptionsConfiguration = null);

	IEFConnectionProvider CreateWithExistingTransaction(
		IScopeContext scopeContext,
		IServiceProvider serviceProvider,
		DbTransaction externalDbTransaction,
		bool? allowLocking,
		bool createAuditEntryStore,
		Action<DbContextOptionsBuilder>? dbContextOptionsConfiguration = null);

	IEFConnectionProvider CreateWithExistingTransaction(
		IScopeContext scopeContext,
		IServiceProvider serviceProvider,
		DbTransaction externalDbTransaction,
		ITransactionsController transactionsController,
		bool? allowLocking,
		bool createAuditEntryStore,
		Action<DbContextOptionsBuilder>? dbContextOptionsConfiguration = null);

	IEFConnectionProvider CreateWithExistingTransaction(
		IScopeContext scopeContext,
		IServiceProvider serviceProvider,
		IDbContextTransaction dbContextTransaction,
		bool? allowLocking,
		bool createAuditEntryStore,
		Action<DbContextOptionsBuilder>? dbContextOptionsConfiguration = null);

	IEFConnectionProvider CreateWithExistingTransaction(
		IScopeContext scopeContext,
		IServiceProvider serviceProvider,
		IDbContextTransaction dbContextTransaction,
		ITransactionsController transactionsController,
		bool? allowLocking,
		bool createAuditEntryStore,
		Action<DbContextOptionsBuilder>? dbContextOptionsConfiguration = null);
}
