using Legion.Database;
using Legion.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;
using System.Data.Common;

namespace Legion.EntityFrameworkCore.PostgreSQL;

internal class PostgreSQLConnectionProviderFactory(
	Action<DbContextOptionsBuilder>? defaultDbContextOptionsConfiguration = null) : IEFConnectionProviderFactory, IConnectionProviderFactory
{
	public Action<DbContextOptionsBuilder>? DefaultDbContextOptionsConfiguration { get; } = defaultDbContextOptionsConfiguration;

	public IEFConnectionProvider CreateWithoutTransactionByStoreId<TConnectionStringProvider>(
		IServiceProvider serviceProvider,
		string? storeId,
		bool? allowLocking,
		bool createAuditEntryStore,
		Action<DbContextOptionsBuilder>? dbContextOptionsConfiguration = null)
		where TConnectionStringProvider : class, IConnectionStringProvider
		=> PostgreSQLConnectionProvider.CreateWithoutTransactionByStoreId<TConnectionStringProvider>(
			serviceProvider,
			storeId,
			allowLocking,
			createAuditEntryStore,
			dbContextOptionsConfiguration ?? DefaultDbContextOptionsConfiguration);

	IConnectionProvider IConnectionProviderFactory.CreateWithoutTransactionByStoreId<TConnectionStringProvider>(
		IServiceProvider serviceProvider,
		string? storeId,
		bool? allowLocking,
		bool createAuditEntryStore)
		=> CreateWithoutTransactionByStoreId<TConnectionStringProvider>(
			serviceProvider,
			storeId,
			allowLocking,
			createAuditEntryStore);

	//Used in *UnitOfWork, *QueryUnitOfWork -> ctor with connectionString
	public IEFConnectionProvider CreateWithoutTransaction(
		IServiceProvider serviceProvider,
		string connectionString,
		bool? allowLocking,
		bool createAuditEntryStore,
		Action<DbContextOptionsBuilder>? dbContextOptionsConfiguration = null)
		=> PostgreSQLConnectionProvider.CreateWithoutTransaction(
			serviceProvider,
			connectionString,
			allowLocking,
			createAuditEntryStore,
			dbContextOptionsConfiguration ?? DefaultDbContextOptionsConfiguration);

	IConnectionProvider IConnectionProviderFactory.CreateWithoutTransaction(
		IServiceProvider serviceProvider,
		string connectionString,
		bool? allowLocking,
		bool createAuditEntryStore)
		=> CreateWithoutTransaction(
			serviceProvider,
			connectionString,
			allowLocking,
			createAuditEntryStore);

	public IEFConnectionProvider CreateWithoutTransaction(
		IServiceProvider serviceProvider,
		DbConnection dbConnection,
		bool? allowLocking,
		bool createAuditEntryStore,
		Action<DbContextOptionsBuilder>? dbContextOptionsConfiguration = null)
		=> PostgreSQLConnectionProvider.CreateWithoutTransaction(
			serviceProvider,
			dbConnection,
			allowLocking,
			createAuditEntryStore,
			dbContextOptionsConfiguration ?? DefaultDbContextOptionsConfiguration);

	IConnectionProvider IConnectionProviderFactory.CreateWithoutTransaction(
		IServiceProvider serviceProvider,
		DbConnection dbConnection,
		bool? allowLocking,
		bool createAuditEntryStore)
		=> CreateWithoutTransaction(
			serviceProvider,
			dbConnection,
			allowLocking,
			createAuditEntryStore);

	public IEFConnectionProvider CreateWithNewTransactionByStoreId<TConnectionStringProvider>(
		IServiceProvider serviceProvider,
		string? storeId,
		IsolationLevel? transactionIsolationLevel,
		bool? allowLocking,
		bool createAuditEntryStore,
		Action<DbContextOptionsBuilder>? dbContextOptionsConfiguration = null)
		where TConnectionStringProvider : class, IConnectionStringProvider
		=> PostgreSQLConnectionProvider.CreateWithNewTransactionByStoreId<TConnectionStringProvider>(
			serviceProvider,
			storeId,
			transactionIsolationLevel,
			allowLocking,
			createAuditEntryStore,
			dbContextOptionsConfiguration ?? DefaultDbContextOptionsConfiguration);

	public IEFConnectionProvider CreateWithNewTransactionByStoreId<TConnectionStringProvider>(
		IServiceProvider serviceProvider,
		string? storeId,
		ITransactionsController transactionsController,
		IsolationLevel? transactionIsolationLevel,
		bool? allowLocking,
		bool createAuditEntryStore,
		Action<DbContextOptionsBuilder>? dbContextOptionsConfiguration = null)
		where TConnectionStringProvider : class, IConnectionStringProvider
		=> PostgreSQLConnectionProvider.CreateWithNewTransactionByStoreId<TConnectionStringProvider>(
			serviceProvider,
			storeId,
			transactionsController,
			transactionIsolationLevel,
			allowLocking,
			createAuditEntryStore,
			dbContextOptionsConfiguration ?? DefaultDbContextOptionsConfiguration);

	//Used by Stores
	IConnectionProvider IConnectionProviderFactory.CreateWithNewTransactionByStoreId<TConnectionStringProvider>(
		IServiceProvider serviceProvider,
		string? storeId,
		IsolationLevel? transactionIsolationLevel,
		bool? allowLocking,
		bool createAuditEntryStore)
		=> CreateWithNewTransactionByStoreId<TConnectionStringProvider>(
			serviceProvider,
			storeId,
			transactionIsolationLevel,
			allowLocking,
			createAuditEntryStore);

	//Used by MessageBus
	IConnectionProvider IConnectionProviderFactory.CreateWithNewTransactionByStoreId<TConnectionStringProvider>(
		IServiceProvider serviceProvider,
		string? storeId,
		ITransactionsController transactionsController,
		IsolationLevel? transactionIsolationLevel,
		bool? allowLocking,
		bool createAuditEntryStore)
		=> CreateWithNewTransactionByStoreId<TConnectionStringProvider>(
			serviceProvider,
			storeId,
			transactionsController,
			transactionIsolationLevel,
			allowLocking,
			createAuditEntryStore);

	public IEFConnectionProvider CreateWithNewTransaction(
		IServiceProvider serviceProvider,
		string connectionString,
		IsolationLevel? transactionIsolationLevel,
		bool? allowLocking,
		bool createAuditEntryStore,
		Action<DbContextOptionsBuilder>? dbContextOptionsConfiguration = null)
		=> PostgreSQLConnectionProvider.CreateWithNewTransaction(
			serviceProvider,
			connectionString,
			transactionIsolationLevel,
			allowLocking,
			createAuditEntryStore,
			dbContextOptionsConfiguration ?? DefaultDbContextOptionsConfiguration);

	public IEFConnectionProvider CreateWithNewTransaction(
		IServiceProvider serviceProvider,
		string connectionString,
		ITransactionsController transactionsController,
		IsolationLevel? transactionIsolationLevel,
		bool? allowLocking,
		bool createAuditEntryStore,
		Action<DbContextOptionsBuilder>? dbContextOptionsConfiguration = null)
		=> PostgreSQLConnectionProvider.CreateWithNewTransaction(
			serviceProvider,
			connectionString,
			transactionsController,
			transactionIsolationLevel,
			allowLocking,
			createAuditEntryStore,
			dbContextOptionsConfiguration ?? DefaultDbContextOptionsConfiguration);

	IConnectionProvider IConnectionProviderFactory.CreateWithNewTransaction(
		IServiceProvider serviceProvider,
		string connectionString,
		IsolationLevel? transactionIsolationLevel,
		bool? allowLocking,
		bool createAuditEntryStore)
		=> CreateWithNewTransaction(
			serviceProvider,
			connectionString,
			transactionIsolationLevel,
			allowLocking,
			createAuditEntryStore);

	IConnectionProvider IConnectionProviderFactory.CreateWithNewTransaction(
		IServiceProvider serviceProvider,
		string connectionString,
		ITransactionsController transactionsController,
		IsolationLevel? transactionIsolationLevel,
		bool? allowLocking,
		bool createAuditEntryStore)
		=> CreateWithNewTransaction(
			serviceProvider,
			connectionString,
			transactionsController,
			transactionIsolationLevel,
			allowLocking,
			createAuditEntryStore);

	public IEFConnectionProvider CreateWithNewTransaction(
		IServiceProvider serviceProvider,
		DbConnection dbConnection,
		IsolationLevel? transactionIsolationLevel,
		bool? allowLocking,
		bool createAuditEntryStore,
		Action<DbContextOptionsBuilder>? dbContextOptionsConfiguration = null)
		=> PostgreSQLConnectionProvider.CreateWithNewTransaction(
			serviceProvider,
			dbConnection,
			transactionIsolationLevel,
			allowLocking,
			createAuditEntryStore,
			dbContextOptionsConfiguration ?? DefaultDbContextOptionsConfiguration);

	public IEFConnectionProvider CreateWithNewTransaction(
		IServiceProvider serviceProvider,
		DbConnection dbConnection,
		ITransactionsController transactionsController,
		IsolationLevel? transactionIsolationLevel,
		bool? allowLocking,
		bool createAuditEntryStore,
		Action<DbContextOptionsBuilder>? dbContextOptionsConfiguration = null)
		=> PostgreSQLConnectionProvider.CreateWithNewTransaction(
			serviceProvider,
			dbConnection,
			transactionsController,
			transactionIsolationLevel,
			allowLocking,
			createAuditEntryStore,
			dbContextOptionsConfiguration ?? DefaultDbContextOptionsConfiguration);

	IConnectionProvider IConnectionProviderFactory.CreateWithNewTransaction(
		IServiceProvider serviceProvider,
		DbConnection dbConnection,
		IsolationLevel? transactionIsolationLevel,
		bool? allowLocking,
		bool createAuditEntryStore)
		=> CreateWithNewTransaction(
			serviceProvider,
			dbConnection,
			transactionIsolationLevel,
			allowLocking,
			createAuditEntryStore);

	IConnectionProvider IConnectionProviderFactory.CreateWithNewTransaction(
		IServiceProvider serviceProvider,
		DbConnection dbConnection,
		ITransactionsController transactionsController,
		IsolationLevel? transactionIsolationLevel,
		bool? allowLocking,
		bool createAuditEntryStore)
		=> CreateWithNewTransaction(
			serviceProvider,
			dbConnection,
			transactionsController,
			transactionIsolationLevel,
			allowLocking,
			createAuditEntryStore);

	public IEFConnectionProvider CreateWithExistingTransaction(
		IScopeContext scopeContext,
		IServiceProvider serviceProvider,
		DbTransaction externalDbTransaction,
		bool? allowLocking,
		bool createAuditEntryStore,
		Action<DbContextOptionsBuilder>? dbContextOptionsConfiguration = null)
		=> PostgreSQLConnectionProvider.CreateWithExistingTransaction(
			scopeContext,
			serviceProvider,
			externalDbTransaction,
			allowLocking,
			createAuditEntryStore,
			dbContextOptionsConfiguration ?? DefaultDbContextOptionsConfiguration);

	public IEFConnectionProvider CreateWithExistingTransaction(
		IScopeContext scopeContext,
		IServiceProvider serviceProvider,
		DbTransaction externalDbTransaction,
		ITransactionsController transactionsController,
		bool? allowLocking,
		bool createAuditEntryStore,
		Action<DbContextOptionsBuilder>? dbContextOptionsConfiguration = null)
		=> PostgreSQLConnectionProvider.CreateWithExistingTransaction(
			scopeContext,
			serviceProvider,
			externalDbTransaction,
			transactionsController,
			allowLocking,
			createAuditEntryStore,
			dbContextOptionsConfiguration ?? DefaultDbContextOptionsConfiguration);

	IConnectionProvider IConnectionProviderFactory.CreateWithExistingTransaction(
		IScopeContext scopeContext,
		IServiceProvider serviceProvider,
		DbTransaction externalDbTransaction,
		bool? allowLocking,
		bool createAuditEntryStore)
		=> CreateWithExistingTransaction(
			scopeContext,
			serviceProvider,
			externalDbTransaction,
			allowLocking,
			createAuditEntryStore);

	IConnectionProvider IConnectionProviderFactory.CreateWithExistingTransaction(
		IScopeContext scopeContext,
		IServiceProvider serviceProvider,
		DbTransaction externalDbTransaction,
		ITransactionsController transactionsController,
		bool? allowLocking,
		bool createAuditEntryStore)
		=> CreateWithExistingTransaction(
			scopeContext,
			serviceProvider,
			externalDbTransaction,
			transactionsController,
			allowLocking,
			createAuditEntryStore);

	public IEFConnectionProvider CreateWithExistingTransaction(
		IScopeContext scopeContext,
		IServiceProvider serviceProvider,
		IDbContextTransaction dbContextTransaction,
		bool? allowLocking,
		bool createAuditEntryStore,
		Action<DbContextOptionsBuilder>? dbContextOptionsConfiguration = null)
		=> PostgreSQLConnectionProvider.CreateWithExistingTransaction(
			scopeContext,
			serviceProvider,
			dbContextTransaction,
			allowLocking,
			createAuditEntryStore,
			dbContextOptionsConfiguration ?? DefaultDbContextOptionsConfiguration);

	public IEFConnectionProvider CreateWithExistingTransaction(
		IScopeContext scopeContext,
		IServiceProvider serviceProvider,
		IDbContextTransaction dbContextTransaction,
		ITransactionsController transactionsController,
		bool? allowLocking,
		bool createAuditEntryStore,
		Action<DbContextOptionsBuilder>? dbContextOptionsConfiguration = null)
		=> PostgreSQLConnectionProvider.CreateWithExistingTransaction(
			scopeContext,
			serviceProvider,
			dbContextTransaction,
			transactionsController,
			allowLocking,
			createAuditEntryStore,
			dbContextOptionsConfiguration ?? DefaultDbContextOptionsConfiguration);
}
