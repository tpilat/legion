using Legion.Database;
using Legion.Database.Transactions;
using Legion.EntityFrameworkCore.Database.Transactions;
using Legion.EntityFrameworkCore.Internals;
using Legion.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Collections.Concurrent;
using System.Data.Common;

namespace Legion.EntityFrameworkCore;

public abstract class EFConnectionProvider : ConnectionProvider, IEFConnectionProvider, IConnectionProvider, IDisposable, IAsyncDisposable
{
	private readonly ConcurrentDictionary<Type, IDbContext> _contextDict = [];

	public IDbContextTransaction? DbContextTransaction { get; protected set; }
	public Action<DbContextOptionsBuilder>? DbContextOptionsConfiguration { get; }

	protected EFConnectionProvider(
		IServiceProvider serviceProvider,
		ITransactionsController? transactionsController,
		bool createInternalTransaction,
		bool? allowLocking,
		bool createAuditEntryStore,
		Action<DbContextOptionsBuilder>?  dbContextOptionsConfiguration)
		: base(
			serviceProvider,
			transactionsController,
			createInternalTransaction,
			allowLocking,
			createAuditEntryStore)
	{
		DbContextOptionsConfiguration = dbContextOptionsConfiguration;
	}

	protected override void SetUnitOfWorkProvider()
	{
		UnitOfWorkProvider = new UnitOfWorkProvider(this);
	}

	public bool HasDbContext<TDbContext>()
		where TDbContext : IDbContext
	{
		if (_contextDict == null)
			Throw.ObjectDisposedException(nameof(_contextDict));

		return _contextDict?.ContainsKey(typeof(TDbContext)) ?? false;
	}

	public TDbContext GetOrCreateDbContext<TDbContext>(IScopeContext scopeContext, bool? allowLocking = null)
		where TDbContext : IDbContext
	{
		Throw.IfArgumentNull(scopeContext);

		if (_contextDict == null)
			Throw.ObjectDisposedException(nameof(_contextDict), scopeContext);

		var setAllowLocking = allowLocking.HasValue;
		allowLocking = allowLocking ?? AllowLocking;

		var ctx = _contextDict?.GetOrAdd(typeof(TDbContext), k =>
		{
			var dbContext = CreateNewDbContext<TDbContext>(scopeContext, allowLocking);
			RegisterDisposable(dbContext);
			return dbContext;
		});

		var result = (TDbContext)ctx!;

		if (result == null)
			Throw.ObjectDisposedException(nameof(_contextDict), scopeContext);

		if (setAllowLocking && result.WithAllowedLocking != allowLocking)
			Throw.InvalidOperationException(Exceptions.Internal.ErrorCodes.DbContext.MismatchLocking(typeof(TDbContext), allowLocking, result.WithAllowedLocking), scopeContext);

		return result;
	}

	private readonly object _lockDbContexts = new();
	private TDbContext CreateNewDbContext<TDbContext>(IScopeContext scopeContext, bool? allowLocking)
		where TDbContext : IDbContext
	{
		lock (_lockDbContexts)
		{
			if (WithTransaction)
			{
				if (TransactionsController == null)
					Throw.InvalidOperationException(Exceptions.Internal.ErrorCodes.DbContext.RegisterToNullTransactionsController, scopeContext);

				if (DbContextTransaction != null)
				{
					var dbContext = DbContextFactory.CreateNewDbContextWithDbContextTransaction<TDbContext>(
						scopeContext,
						ServiceProvider,
						this,
						new DbContextSettintgs
						{
							AllowLocking = allowLocking,
							AuditEntryStore = AuditEntryStore,
							DomainEventStore = DomainEventStore
						},
						out _); //discard is ok, because DbContextTransaction will be reused

					SetConnection(scopeContext);

					//TODO test if this all would be deleted
					////if (RegisterToTransactionsController)
					////{
					//	//if (TransactionsController == null)
					//	//	Throw.InvalidOperationException(Exceptions.Internal.ErrorCodes.DbContext.RegisterToNullTransactionsController, scopeContext);

					//	var dbTransactionManager = new DbContextTransactionManager(DbContextTransaction);
					//	TransactionsController.UnregisterTransactionManager(scopeContext, dbTransactionManager); //TODO: why is there the same object dbTransactionManager as in two lines lower? Is it a bug?

					////we don't need this TransactionsController.RegisterTransactionManager(scopeContext, new DbContextManager(dbContext));
					//TransactionsController.RegisterTransactionManager(scopeContext, dbTransactionManager); //TODO: why is there the same object dbTransactionManager as in two lines upper? Is it a bug?
					////}

					return dbContext;
				}
				else if (ExternalDbTransaction != null)
				{
					var dbContext = DbContextFactory.CreateNewDbContextWithExternalDbTransaction<TDbContext>(
						scopeContext,
						ServiceProvider,
						this,
						new DbContextSettintgs
						{
							AllowLocking = AllowLocking,
							AuditEntryStore = AuditEntryStore,
							DomainEventStore = DomainEventStore
						},
						out _); //discard is ok, because ExternalDbTransaction will be reused

					DbContextTransaction = dbContext.Database.CurrentTransaction;
					SetConnection(scopeContext);

					//if (registerToTransactionsController ?? RegisterToTransactionsController)
					//{
					//if (TransactionsController == null)
					//	Throw.InvalidOperationException(Exceptions.Internal.ErrorCodes.DbContext.RegisterToNullTransactionsController, scopeContext);

					var dbTransactionManager = new DbTransactionManager(ExternalDbTransaction);
					TransactionsController.UnregisterTransactionManager(scopeContext, dbTransactionManager);

					//we don't need this TransactionsController.RegisterTransactionManager(scopeContext, new DbContextManager(dbContext));
					TransactionsController.RegisterTransactionManager(scopeContext, new DbContextTransactionManager(DbContextTransaction!));

					//zaregistrujem na konci - az za DbContextManager - AK VOBEC TREBA ????
					//TransactionsController.RegisterTransactionManager(scopeContext, dbTransactionManager);
					//}

					return dbContext;
				}
				else
				{
					var dbContext = DbContextFactory.CreateNewDbContextAndCreateNewTransaction<TDbContext>(
						scopeContext,
						ServiceProvider,
						this,
						new DbContextSettintgs
						{
							AllowLocking = AllowLocking,
							AuditEntryStore = AuditEntryStore,
							DomainEventStore = DomainEventStore
						},
						out var dbContextTransaction,
						TransactionIsolationLevel);

					DbContextTransaction = dbContextTransaction;
					SetConnection(scopeContext);

					//if (registerToTransactionsController ?? RegisterToTransactionsController)
					//{
					//if (TransactionsController == null)
					//	Throw.InvalidOperationException(Exceptions.Internal.ErrorCodes.DbContext.RegisterToNullTransactionsController, scopeContext);

					//we don't need this TransactionsController.RegisterTransactionManager(scopeContext, new DbContextManager(dbContext));
					TransactionsController.RegisterTransactionManager(scopeContext, new DbContextTransactionManager(DbContextTransaction));
					//}

					return dbContext;
				}
			}
			else
			{
				var dbContext = DbContextFactory.CreateNewDbContextWithoutTransaction<TDbContext>(
					ServiceProvider,
					this,
					new DbContextSettintgs
					{
						AllowLocking = AllowLocking,
						AuditEntryStore = AuditEntryStore,
						DomainEventStore = DomainEventStore
					});

				//if (registerToTransactionsController ?? RegisterToTransactionsController)
				//{
				//	Throw.InvalidOperationException($"{nameof(WithTransaction)} = false && {nameof(RegisterToTransactionsController)} = true", scopeContext);

				//	//TransactionsController.RegisterTransactionManager(scopeContext, new DbContextManager(dbContext));
				//}

				return dbContext;
			}
		}
	}

	public override bool ReCreateTransaction(IScopeContext scopeContext)
	{
		if (!WithTransaction || !_isInternalTransaction || ExternalDbTransaction != null)
			return false;

		var allDbContexts = _contextDict.Values.ToList();
		if (allDbContexts.Count == 0)
			return false;

		Throw.IfNull(TransactionsController, scopeContext);
		Throw.IfNull(DbContextTransaction, scopeContext);

		lock (_lockDbContexts)
		{
			TransactionsController.SetRecreated();
			TransactionsController.UnregisterTransactionManager(scopeContext, new DbContextTransactionManager(DbContextTransaction));
			DbContextTransaction.Dispose();

			DbContextFactory.ReSetDbTransaction(
				scopeContext,
				allDbContexts[0],
				out var dbContextTransaction,
				TransactionIsolationLevel);

			DbContextTransaction = dbContextTransaction;
			SetConnection(scopeContext);

			foreach (var dbContext in allDbContexts.Skip(1))
			{
				dbContext.Database.UseTransaction(DbContextTransaction.GetDbTransaction());
			}

			TransactionsController.RegisterTransactionManager(scopeContext, new DbContextTransactionManager(DbContextTransaction));

			return true;
		}
	}

	private bool SetConnection(IScopeContext scopeContext, DbConnection dbConnection)
	{
		Throw.IfArgumentNull(dbConnection);

		SetConnection(scopeContext);
		if (DbConnection != null)
			return false;

		DbConnection = dbConnection;
		return true;
	}

	private void SetConnection(IScopeContext scopeContext)
	{
		var dbContextTransactionConnection = DbContextTransaction?.GetDbTransaction()?.Connection;
		var externalDbTransactionConnection = ExternalDbTransaction?.Connection;

		if (DbConnection == null)
		{
			if (dbContextTransactionConnection == null)
			{
				DbConnection = externalDbTransactionConnection;
			}
			else
			{
				if (externalDbTransactionConnection == null)
				{
					DbConnection = dbContextTransactionConnection;
				}
				else
				{
					if (dbContextTransactionConnection != externalDbTransactionConnection)
					{
						Throw.InvalidOperationException(Exceptions.Internal.ErrorCodes.DbContext.ConnectionMismatch(nameof(dbContextTransactionConnection), nameof(externalDbTransactionConnection)), scopeContext);
					}
					else
					{
						DbConnection = dbContextTransactionConnection;
					}
				}
			}
		}
		else
		{
			if (dbContextTransactionConnection == null)
			{
				if (DbConnection != externalDbTransactionConnection)
				{
					Throw.InvalidOperationException(Exceptions.Internal.ErrorCodes.DbContext.ConnectionMismatch(nameof(DbConnection), nameof(externalDbTransactionConnection)), scopeContext);
				}
			}
			else
			{
				if (externalDbTransactionConnection == null)
				{
					if (DbConnection != dbContextTransactionConnection)
					{
						Throw.InvalidOperationException(Exceptions.Internal.ErrorCodes.DbContext.ConnectionMismatch(nameof(DbConnection), nameof(dbContextTransactionConnection)), scopeContext);
					}
				}
				else
				{
					if (DbConnection != externalDbTransactionConnection)
					{
						Throw.InvalidOperationException(Exceptions.Internal.ErrorCodes.DbContext.ConnectionMismatch(nameof(DbConnection), nameof(externalDbTransactionConnection)), scopeContext);
					}

					if (DbConnection != dbContextTransactionConnection)
					{
						Throw.InvalidOperationException(Exceptions.Internal.ErrorCodes.DbContext.ConnectionMismatch(nameof(DbConnection), nameof(dbContextTransactionConnection)), scopeContext);
					}
				}
			}
		}
	}

	public abstract void OnConfiguring(DbContextOptionsBuilder optionsBuilder);

	protected override void ClearBeforeDispose()
	{
		base.ClearBeforeDispose();
		_contextDict.Clear();
	}
}
