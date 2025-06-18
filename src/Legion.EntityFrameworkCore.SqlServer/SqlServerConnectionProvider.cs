using Legion.Database;
using Legion.Database.Transactions;
using Legion.EntityFrameworkCore.Database.Transactions;
using Legion.EntityFrameworkCore.Exceptions.Internal;
using Legion.Extensions;
using Legion.Transactions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using System.Data;
using System.Data.Common;

namespace Legion.EntityFrameworkCore.SqlServer;

internal class SqlServerConnectionProvider : EFConnectionProvider, IEFConnectionProvider, IConnectionProvider, IDisposable, IAsyncDisposable
{
	public override SqlConnection? GetDbConnection()
	{
		if (DbConnection == null)
			return null;

		if (DbConnection is SqlConnection conn)
			return conn;

		Throw.NotSupportedException($"Invalid {nameof(DbConnection)} type = {DbConnection.GetType().ToFriendlyFullName()}");
		return null!;
	}

	public override SqlConnection GetOrCreateNewDbConnection(out bool isNewConnection)
	{
		isNewConnection = true;

		if (DbConnection == null)
		{
			if (string.IsNullOrWhiteSpace(ConnectionString))
				Throw.InvalidOperationException(Legion.EntityFrameworkCore.Exceptions.Internal.ErrorCodes.DbContext.NoConnectionString);

			string connectionString = string.Format(ConnectionString, System.Environment.GetEnvironmentVariable("PGPASSWORD"));
			var connection = new SqlConnection(connectionString);

			connection.Open();

			return connection;
		}
		else
		{
			if (DbConnection is SqlConnection conn)
			{
				isNewConnection = false;
				return conn;
			}
			else
			{
				Throw.NotSupportedException($"Invalid {nameof(DbConnection)} type = {DbConnection.GetType().ToFriendlyFullName()}");
				return null!;
			}
		}
	}

	public override SqlConnection CreateNewDbConnection()
	{
		if (string.IsNullOrWhiteSpace(ConnectionString))
			Throw.InvalidOperationException(Legion.EntityFrameworkCore.Exceptions.Internal.ErrorCodes.DbContext.NoConnectionString);

		string connectionString = string.Format(ConnectionString, System.Environment.GetEnvironmentVariable("PGPASSWORD"));
		var connection = new SqlConnection(connectionString);

		connection.Open();

		return connection;
	}

	private SqlServerConnectionProvider(
		IServiceProvider serviceProvider,
		ITransactionsController? transactionsController,
		bool createInternalTransaction,
		bool? allowLocking,
		bool createAuditEntryStore,
		Action<DbContextOptionsBuilder>? dbContextOptionsConfiguration)
		: base(
			serviceProvider,
			transactionsController,
			createInternalTransaction,
			allowLocking,
			createAuditEntryStore,
			dbContextOptionsConfiguration)
	{
	}

	public override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		if (DbConnection == null)
		{
			if (string.IsNullOrWhiteSpace(ConnectionString))
				Throw.InvalidOperationException(ErrorCodes.DbContext.NoConnectionString);

			optionsBuilder.UseSqlServer(string.Format(ConnectionString, System.Environment.GetEnvironmentVariable("MSSQLPASSWORD")));
		}
		else
		{
			optionsBuilder.UseSqlServer(DbConnection);
		}

		DbContextOptionsConfiguration?.Invoke(optionsBuilder);
	}

	public static EFConnectionProvider CreateWithoutTransactionByStoreId<TConnectionStringProvider>(
		IServiceProvider serviceProvider,
		string? storeId,
		bool? allowLocking,
		bool createAuditEntryStore,
		Action<DbContextOptionsBuilder>? dbContextOptionsConfiguration = null)
		where TConnectionStringProvider : class, IConnectionStringProvider
	{
		Throw.IfArgumentNull(serviceProvider);

		var connectionStringProvider = serviceProvider.GetRequiredService<TConnectionStringProvider>();
		var connectionString = string.IsNullOrWhiteSpace(storeId)
			? connectionStringProvider.GetDefaultConncetionString()
			: connectionStringProvider.GetConncetionString(storeId);

		Throw.IfNullOrWhiteSpace(connectionString);

		var connectionProvider = new SqlServerConnectionProvider(
			serviceProvider,
			null,
			createInternalTransaction: false,
			allowLocking,
			createAuditEntryStore,
			dbContextOptionsConfiguration)
		{
			ConnectionString = connectionString,
			WithTransaction = false,
			TransactionIsolationLevel = null,
			ExternalDbTransaction = null,
			DbContextTransaction = null
		};

		return connectionProvider;
	}

	public static EFConnectionProvider CreateWithoutTransaction(
		IServiceProvider serviceProvider,
		string connectionString,
		bool? allowLocking,
		bool createAuditEntryStore,
		Action<DbContextOptionsBuilder>? dbContextOptionsConfiguration = null)
	{
		Throw.IfArgumentNull(serviceProvider);
		Throw.IfArgumentNullOrWhiteSpace(connectionString);

		var connectionProvider = new SqlServerConnectionProvider(
			serviceProvider,
			null,
			createInternalTransaction: false,
			allowLocking,
			createAuditEntryStore,
			dbContextOptionsConfiguration)
		{
			ConnectionString = connectionString,
			WithTransaction = false,
			TransactionIsolationLevel = null,
			ExternalDbTransaction = null,
			DbContextTransaction = null
		};

		return connectionProvider;
	}

	public static EFConnectionProvider CreateWithoutTransaction(
		IServiceProvider serviceProvider,
		DbConnection dbConnection,
		bool? allowLocking,
		bool createAuditEntryStore,
		Action<DbContextOptionsBuilder>? dbContextOptionsConfiguration = null)
	{
		Throw.IfArgumentNull(serviceProvider);
		Throw.IfArgumentNull(dbConnection);

		var connectionProvider = new SqlServerConnectionProvider(
			serviceProvider,
			null,
			createInternalTransaction: false,
			allowLocking,
			createAuditEntryStore,
			dbContextOptionsConfiguration)
		{
			DbConnection = dbConnection,
			WithTransaction = false,
			TransactionIsolationLevel = null,
			ExternalDbTransaction = null,
			DbContextTransaction = null
		};

		return connectionProvider;
	}

	public static EFConnectionProvider CreateWithNewTransactionByStoreId<TConnectionStringProvider>(
		IServiceProvider serviceProvider,
		string? storeId,
		IsolationLevel? transactionIsolationLevel,
		bool? allowLocking,
		bool createAuditEntryStore,
		Action<DbContextOptionsBuilder>? dbContextOptionsConfiguration = null)
		where TConnectionStringProvider : class, IConnectionStringProvider
	{
		Throw.IfArgumentNull(serviceProvider);

		var connectionStringProvider = serviceProvider.GetRequiredService<TConnectionStringProvider>();
		var connectionString = string.IsNullOrWhiteSpace(storeId)
			? connectionStringProvider.GetDefaultConncetionString()
			: connectionStringProvider.GetConncetionString(storeId);

		Throw.IfNullOrWhiteSpace(connectionString);

		var connectionProvider = new SqlServerConnectionProvider(
			serviceProvider,
			null,
			createInternalTransaction: true,
			allowLocking,
			createAuditEntryStore,
			dbContextOptionsConfiguration)
		{
			ConnectionString = connectionString,
			WithTransaction = true,
			TransactionIsolationLevel = transactionIsolationLevel,
			ExternalDbTransaction = null,
			DbContextTransaction = null
		};

		return connectionProvider;
	}

	public static EFConnectionProvider CreateWithNewTransactionByStoreId<TConnectionStringProvider>(
		IServiceProvider serviceProvider,
		string? storeId,
		ITransactionsController transactionsController,
		IsolationLevel? transactionIsolationLevel,
		bool? allowLocking,
		bool createAuditEntryStore,
		Action<DbContextOptionsBuilder>? dbContextOptionsConfiguration = null)
		where TConnectionStringProvider : class, IConnectionStringProvider
	{
		Throw.IfArgumentNull(serviceProvider);

		var connectionStringProvider = serviceProvider.GetRequiredService<TConnectionStringProvider>();
		var connectionString = string.IsNullOrWhiteSpace(storeId)
			? connectionStringProvider.GetDefaultConncetionString()
			: connectionStringProvider.GetConncetionString(storeId);

		Throw.IfNullOrWhiteSpace(connectionString);
		Throw.IfArgumentNull(transactionsController);

		var connectionProvider = new SqlServerConnectionProvider(
			serviceProvider,
			transactionsController,
			createInternalTransaction: false,
			allowLocking,
			createAuditEntryStore,
			dbContextOptionsConfiguration)
		{
			ConnectionString = connectionString,
			WithTransaction = true,
			TransactionIsolationLevel = transactionIsolationLevel,
			ExternalDbTransaction = null,
			DbContextTransaction = null
		};

		return connectionProvider;
	}

	public static EFConnectionProvider CreateWithNewTransaction(
		IServiceProvider serviceProvider,
		string connectionString,
		IsolationLevel? transactionIsolationLevel,
		bool? allowLocking,
		bool createAuditEntryStore,
		Action<DbContextOptionsBuilder>? dbContextOptionsConfiguration = null)
	{
		Throw.IfArgumentNull(serviceProvider);
		Throw.IfArgumentNullOrWhiteSpace(connectionString);

		var connectionProvider = new SqlServerConnectionProvider(
			serviceProvider,
			null,
			createInternalTransaction: true,
			allowLocking,
			createAuditEntryStore,
			dbContextOptionsConfiguration)
		{
			ConnectionString = connectionString,
			WithTransaction = true,
			TransactionIsolationLevel = transactionIsolationLevel,
			ExternalDbTransaction = null,
			DbContextTransaction = null
		};

		return connectionProvider;
	}

	public static EFConnectionProvider CreateWithNewTransaction(
		IServiceProvider serviceProvider,
		string connectionString,
		ITransactionsController transactionsController,
		IsolationLevel? transactionIsolationLevel,
		bool? allowLocking,
		bool createAuditEntryStore,
		Action<DbContextOptionsBuilder>? dbContextOptionsConfiguration = null)
	{
		Throw.IfArgumentNull(serviceProvider);
		Throw.IfArgumentNullOrWhiteSpace(connectionString);
		Throw.IfArgumentNull(transactionsController);

		var connectionProvider = new SqlServerConnectionProvider(
			serviceProvider,
			transactionsController,
			createInternalTransaction: false,
			allowLocking,
			createAuditEntryStore,
			dbContextOptionsConfiguration)
		{
			ConnectionString = connectionString,
			WithTransaction = true,
			TransactionIsolationLevel = transactionIsolationLevel,
			ExternalDbTransaction = null,
			DbContextTransaction = null
		};

		return connectionProvider;
	}

	public static EFConnectionProvider CreateWithNewTransaction(
		IServiceProvider serviceProvider,
		DbConnection dbConnection,
		IsolationLevel? transactionIsolationLevel,
		bool? allowLocking,
		bool createAuditEntryStore,
		Action<DbContextOptionsBuilder>? dbContextOptionsConfiguration = null)
	{
		Throw.IfArgumentNull(serviceProvider);
		Throw.IfArgumentNull(dbConnection);

		var connectionProvider = new SqlServerConnectionProvider(
			serviceProvider,
			null,
			createInternalTransaction: true,
			allowLocking,
			createAuditEntryStore,
			dbContextOptionsConfiguration)
		{
			DbConnection = dbConnection,
			WithTransaction = true,
			TransactionIsolationLevel = transactionIsolationLevel,
			ExternalDbTransaction = null,
			DbContextTransaction = null
		};

		return connectionProvider;
	}

	public static EFConnectionProvider CreateWithNewTransaction(
		IServiceProvider serviceProvider,
		DbConnection dbConnection,
		ITransactionsController transactionsController,
		IsolationLevel? transactionIsolationLevel,
		bool? allowLocking,
		bool createAuditEntryStore,
		Action<DbContextOptionsBuilder>? dbContextOptionsConfiguration = null)
	{
		Throw.IfArgumentNull(serviceProvider);
		Throw.IfArgumentNull(dbConnection);
		Throw.IfArgumentNull(transactionsController);

		var connectionProvider = new SqlServerConnectionProvider(
			serviceProvider,
			transactionsController,
			createInternalTransaction: false,
			allowLocking,
			createAuditEntryStore,
			dbContextOptionsConfiguration)
		{
			DbConnection = dbConnection,
			WithTransaction = true,
			TransactionIsolationLevel = transactionIsolationLevel,
			ExternalDbTransaction = null,
			DbContextTransaction = null
		};

		return connectionProvider;
	}

	public static EFConnectionProvider CreateWithExistingTransaction(
		IScopeContext scopeContext,
		IServiceProvider serviceProvider,
		DbTransaction externalDbTransaction,
		bool? allowLocking,
		bool createAuditEntryStore,
		Action<DbContextOptionsBuilder>? dbContextOptionsConfiguration = null)
	{
		Throw.IfArgumentNull(serviceProvider);
		Throw.IfArgumentNull(externalDbTransaction);

		var connectionProvider = new SqlServerConnectionProvider(
			serviceProvider,
			null,
			createInternalTransaction: true,
			allowLocking,
			createAuditEntryStore,
			dbContextOptionsConfiguration)
		{
			DbConnection = externalDbTransaction.Connection,
			WithTransaction = true,
			TransactionIsolationLevel = externalDbTransaction.IsolationLevel,
			ExternalDbTransaction = externalDbTransaction,
			DbContextTransaction = null
		};

		connectionProvider.TransactionsController!.RegisterTransactionManager(scopeContext, new DbTransactionManager(externalDbTransaction));

		return connectionProvider;
	}

	public static EFConnectionProvider CreateWithExistingTransaction(
		IScopeContext scopeContext,
		IServiceProvider serviceProvider,
		DbTransaction externalDbTransaction,
		ITransactionsController transactionsController,
		bool? allowLocking,
		bool createAuditEntryStore,
		Action<DbContextOptionsBuilder>? dbContextOptionsConfiguration = null)
	{
		Throw.IfArgumentNull(serviceProvider);
		Throw.IfArgumentNull(externalDbTransaction);
		Throw.IfArgumentNull(transactionsController);

		var connectionProvider = new SqlServerConnectionProvider(
			serviceProvider,
			transactionsController,
			createInternalTransaction: false,
			allowLocking,
			createAuditEntryStore,
			dbContextOptionsConfiguration)
		{
			DbConnection = externalDbTransaction.Connection,
			WithTransaction = true,
			TransactionIsolationLevel = externalDbTransaction.IsolationLevel,
			ExternalDbTransaction = externalDbTransaction,
			DbContextTransaction = null
		};

		connectionProvider.TransactionsController!.RegisterTransactionManager(scopeContext, new DbTransactionManager(externalDbTransaction));

		return connectionProvider;
	}

	public static EFConnectionProvider CreateWithExistingTransaction(
		IScopeContext scopeContext,
		IServiceProvider serviceProvider,
		IDbContextTransaction dbContextTransaction,
		bool? allowLocking,
		bool createAuditEntryStore,
		Action<DbContextOptionsBuilder>? dbContextOptionsConfiguration = null)
	{
		Throw.IfArgumentNull(serviceProvider);
		Throw.IfArgumentNull(dbContextTransaction);

		var externalDbTransaction = dbContextTransaction.GetDbTransaction();

		var connectionProvider = new SqlServerConnectionProvider(
			serviceProvider,
			null,
			createInternalTransaction: true,
			allowLocking,
			createAuditEntryStore,
			dbContextOptionsConfiguration)
		{
			DbConnection = externalDbTransaction.Connection,
			WithTransaction = true,
			TransactionIsolationLevel = externalDbTransaction.IsolationLevel,
			ExternalDbTransaction = externalDbTransaction,
			DbContextTransaction = dbContextTransaction
		};

		connectionProvider.TransactionsController!.RegisterTransactionManager(scopeContext, new DbContextTransactionManager(dbContextTransaction));

		return connectionProvider;
	}

	public static EFConnectionProvider CreateWithExistingTransaction(
		IScopeContext scopeContext,
		IServiceProvider serviceProvider,
		IDbContextTransaction dbContextTransaction,
		ITransactionsController transactionsController,
		bool? allowLocking,
		bool createAuditEntryStore,
		Action<DbContextOptionsBuilder>? dbContextOptionsConfiguration = null)
	{
		Throw.IfArgumentNull(serviceProvider);
		Throw.IfArgumentNull(dbContextTransaction);
		Throw.IfArgumentNull(transactionsController);

		var externalDbTransaction = dbContextTransaction.GetDbTransaction();

		var connectionProvider = new SqlServerConnectionProvider(
			serviceProvider,
			transactionsController,
			createInternalTransaction: false,
			allowLocking,
			createAuditEntryStore,
			dbContextOptionsConfiguration)
		{
			DbConnection = externalDbTransaction.Connection,
			WithTransaction = true,
			TransactionIsolationLevel = externalDbTransaction.IsolationLevel,
			ExternalDbTransaction = externalDbTransaction,
			DbContextTransaction = dbContextTransaction
		};

		connectionProvider.TransactionsController!.RegisterTransactionManager(scopeContext, new DbContextTransactionManager(dbContextTransaction));

		return connectionProvider;
	}
}
