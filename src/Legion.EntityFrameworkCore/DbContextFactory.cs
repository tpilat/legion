using Legion.EntityFrameworkCore.Exceptions.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel.Design;
using System.Data;
using System.Data.Common;

namespace Legion.EntityFrameworkCore;

public static partial class DbContextFactory
{
	public static TContext CreateNewDbContextWithDbContextTransaction<TContext>(
		IScopeContext scopeContext,
		IServiceProvider serviceProvider,
		IEFConnectionProvider connectionProvider,
		IDbContextSettintgs dbContextSettintgs,
		out IDbContextTransaction? newDbContextTransaction)
		where TContext : IDbContext
	{
		Throw.IfArgumentNull(serviceProvider);
		Throw.IfArgumentNull(connectionProvider);
		Throw.IfArgumentNull(connectionProvider.DbContextTransaction);

		var dbContext = serviceProvider.GetRequiredService<TContext>();
		if (dbContext is DbContextBase dbContextBase)
			dbContextBase.Initialize(dbContextSettintgs, connectionProvider);

		dbContext.SetDbTransaction(
			scopeContext,
			connectionProvider.DbContextTransaction,
			out newDbContextTransaction,
			TransactionUsage.Reuse,
			null);

		return dbContext;
	}

	public static TContext CreateNewDbContextWithExternalDbTransaction<TContext>(
		IScopeContext scopeContext,
		IServiceProvider serviceProvider,
		IEFConnectionProvider connectionProvider,
		IDbContextSettintgs dbContextSettintgs,
		out IDbContextTransaction? newDbContextTransaction)
		where TContext : IDbContext
	{
		Throw.IfArgumentNull(serviceProvider);
		Throw.IfArgumentNull(connectionProvider);
		Throw.IfArgumentNull(connectionProvider.ExternalDbTransaction);

		var dbContext = serviceProvider.GetRequiredService<TContext>();
		if (dbContext is DbContextBase dbContextBase)
			dbContextBase.Initialize(dbContextSettintgs, connectionProvider);

		dbContext.SetDbTransaction(
			scopeContext,
			connectionProvider.ExternalDbTransaction,
			out newDbContextTransaction,
			TransactionUsage.Reuse,
			null);

		return dbContext;
	}

	public static TContext CreateNewDbContextAndCreateNewTransaction<TContext>(
		IScopeContext scopeContext,
		IServiceProvider serviceProvider,
		IEFConnectionProvider connectionProvider,
		IDbContextSettintgs dbContextSettintgs,
		out IDbContextTransaction newDbContextTransaction,
		IsolationLevel? transactionIsolationLevel = null)
		where TContext : IDbContext
	{
		Throw.IfArgumentNull(serviceProvider);
		Throw.IfArgumentNull(connectionProvider);

		var dbContext = serviceProvider.GetRequiredService<TContext>();
		if (dbContext is DbContextBase dbContextBase)
			dbContextBase.Initialize(dbContextSettintgs, connectionProvider);

		dbContext.SetDbTransaction(
			scopeContext,
			(IDbContextTransaction)null!,
			out newDbContextTransaction!,
			TransactionUsage.CreateNew, //********NEW DB TRANSACTION
			transactionIsolationLevel);

		return dbContext;
	}

	public static TContext CreateNewDbContextWithoutTransaction<TContext>(
		IServiceProvider serviceProvider,
		IEFConnectionProvider connectionProvider,
		IDbContextSettintgs dbContextSettintgs)
		where TContext : IDbContext
	{
		Throw.IfArgumentNull(serviceProvider);
		Throw.IfArgumentNull(connectionProvider);

		var dbContext = serviceProvider.GetRequiredService<TContext>();
		if (dbContext is DbContextBase dbContextBase)
			dbContextBase.Initialize(dbContextSettintgs, connectionProvider);

		return dbContext;
	}

	//public static TContext SetIDbTransaction<TContext>(
	//	TContext dbContext,
	//	IDbContextTransaction? existingDbContextTransaction,
	//	out IDbContextTransaction? newDbContextTransaction,
	//	TransactionUsage transactionUsage,
	//	IsolationLevel? transactionIsolationLevel)
	//	where TContext : IDbContext
	//{
	//	newDbContextTransaction = null;

	//	Throw.ArgumentNull(dbContext);

	//	if (transactionUsage == TransactionUsage.NONE)
	//		return dbContext;

	//	if (transactionUsage == TransactionUsage.Reuse)
	//	{
	//		if (existingDbContextTransaction == null)
	//		{
	//			throw new ArgumentNullException(nameof(existingDbContextTransaction));

	//			//if (transactionIsolationLevel.HasValue)
	//			//{
	//			//	newDbContextTransaction = dbContext.Database.BeginTransaction(transactionIsolationLevel.Value);
	//			//}
	//			//else
	//			//{
	//			//	newDbContextTransaction = dbContext.Database.BeginTransaction();
	//			//}

	//			//return dbContext;
	//		}
	//		else
	//		{
	//			if (dbContext.Database.CurrentTransaction == null)
	//			{
	//				newDbContextTransaction = existingDbContextTransaction;
	//				dbContext.Database.UseTransaction(newDbContextTransaction.GetDbTransaction());
	//				return dbContext;
	//			}
	//			else
	//			{
	//				if (dbContext.Database.CurrentTransaction.TransactionId != existingDbContextTransaction.TransactionId)
	//					throw new InvalidOperationException($"DbContext already has set another transaction with id {dbContext.Database.CurrentTransaction.TransactionId}");

	//				return dbContext;
	//			}
	//		}
	//	}

	//	if (transactionUsage == TransactionUsage.CreateNew)
	//	{
	//		if (dbContext.Database.CurrentTransaction == null)
	//		{
	//			if (transactionIsolationLevel.HasValue)
	//			{
	//				newDbContextTransaction = dbContext.Database.BeginTransaction(transactionIsolationLevel.Value);
	//			}
	//			else
	//			{
	//				newDbContextTransaction = dbContext.Database.BeginTransaction();
	//			}

	//			return dbContext;
	//		}
	//		else
	//		{
	//			throw new InvalidOperationException($"DbContext already has set another transaction with id {dbContext.Database.CurrentTransaction.TransactionId}");
	//		}
	//	}

	//	return dbContext;
	//}

	//public static TContext SetIDbTransaction<TContext>(
	//	TContext dbContext,
	//	DbTransaction? existingTransaction,
	//	out IDbContextTransaction? newDbContextTransaction,
	//	TransactionUsage transactionUsage,
	//	IsolationLevel? transactionIsolationLevel)
	//	where TContext : IDbContext
	//{
	//	newDbContextTransaction = null;

	//	Throw.ArgumentNull(dbContext);

	//	if (transactionUsage == TransactionUsage.NONE)
	//		return dbContext;

	//	if (transactionUsage == TransactionUsage.Reuse)
	//	{
	//		if (existingTransaction == null)
	//		{
	//			throw new ArgumentNullException(nameof(existingTransaction));

	//			//if (transactionIsolationLevel.HasValue)
	//			//{
	//			//	newDbContextTransaction = dbContext.Database.BeginTransaction(transactionIsolationLevel.Value);
	//			//}
	//			//else
	//			//{
	//			//	newDbContextTransaction = dbContext.Database.BeginTransaction();
	//			//}

	//			//return dbContext;
	//		}
	//		else
	//		{
	//			if (dbContext.Database.CurrentTransaction == null)
	//			{
	//				dbContext.Database.UseTransaction(existingTransaction);
	//				newDbContextTransaction = dbContext.Database.CurrentTransaction;
	//				return dbContext;
	//			}
	//			else
	//			{
	//				if (dbContext.Database.CurrentTransaction.GetDbTransaction() != existingTransaction)
	//					throw new InvalidOperationException($"DbContext already has set another transaction with id {dbContext.Database.CurrentTransaction.TransactionId}");

	//				return dbContext;
	//			}
	//		}
	//	}

	//	if (transactionUsage == TransactionUsage.CreateNew)
	//	{
	//		if (dbContext.Database.CurrentTransaction == null)
	//		{
	//			if (transactionIsolationLevel.HasValue)
	//			{
	//				newDbContextTransaction = dbContext.Database.BeginTransaction(transactionIsolationLevel.Value);
	//			}
	//			else
	//			{
	//				newDbContextTransaction = dbContext.Database.BeginTransaction();
	//			}

	//			return dbContext;
	//		}
	//		else
	//		{
	//			throw new InvalidOperationException($"DbContext already has set another transaction with id {dbContext.Database.CurrentTransaction.TransactionId}");
	//		}
	//	}

	//	return dbContext;
	//}





	//public static TContext CreateNewDbContext<TContext>(
	//	IScopeContext scopeContext,
	//	IServiceProvider serviceProvider,
	//	IDbContextTransaction existingDbContextTransaction,
	//	out IDbContextTransaction? newDbContextTransaction,
	//	string? traceFrame = null,
	//	Guid? idCommandQuery = null)
	//	where TContext : DbContext
	//{
	//	Throw.ArgumentNull(serviceProvider);
	//	Throw.ArgumentNull(existingDbContextTransaction);

	//	var dbContext = serviceProvider.GetRequiredService<TContext>();
	//	if (dbContext is DbContextBase dbContextBase)
	//	{
	//		dbContextBase.TraceFrame = traceFrame;
	//		dbContextBase.IdCommandQuery = idCommandQuery;
	//		dbContextBase.Initialize(
	//			existingDbContextTransaction.GetDbTransaction().Connection,
	//			null,
	//			null);
	//	}

	//	return SetDbTransaction(scopeContext, dbContext, existingDbContextTransaction, out newDbContextTransaction, TransactionUsage.Reuse, null);
	//}

	//public static TContext CreateNewDbContext<TContext>(
	//	IScopeContext scopeContext,
	//	IServiceProvider serviceProvider,
	//	DbTransaction existingDbTransaction,
	//	out IDbContextTransaction? newDbContextTransaction,
	//	string? traceFrame = null,
	//	Guid? idCommandQuery = null)
	//	where TContext : DbContext
	//{
	//	Throw.ArgumentNull(serviceProvider);
	//	Throw.ArgumentNull(existingDbTransaction);

	//	var dbContext = serviceProvider.GetRequiredService<TContext>();
	//	if (dbContext is DbContextBase dbContextBase)
	//	{
	//		dbContextBase.TraceFrame = traceFrame;
	//		dbContextBase.IdCommandQuery = idCommandQuery;
	//		dbContextBase.Initialize(
	//			existingDbTransaction.Connection,
	//			null,
	//			null);
	//	}

	//	return SetDbTransaction(scopeContext, dbContext, existingDbTransaction, out newDbContextTransaction, TransactionUsage.Reuse, null);
	//}

	//public static TContext CreateNewDbContext<TContext>(
	//	IScopeContext scopeContext,
	//	IServiceProvider serviceProvider,
	//	out IDbContextTransaction newDbContextTransaction,
	//	IsolationLevel? transactionIsolationLevel = null,
	//	DbConnection? externalDbConnection = null,
	//	string? connectionString = null,
	//	string? traceFrame = null,
	//	Guid? idCommandQuery = null)
	//	where TContext : DbContext
	//{
	//	Throw.ArgumentNull(serviceProvider);

	//	var dbContext = serviceProvider.GetRequiredService<TContext>();
	//	if (dbContext is DbContextBase dbContextBase)
	//	{
	//		dbContextBase.TraceFrame = traceFrame;
	//		dbContextBase.IdCommandQuery = idCommandQuery;
	//		dbContextBase.Initialize(
	//			externalDbConnection,
	//			connectionString,
	//			null);
	//	}

	//	return SetDbTransaction(scopeContext, dbContext, (IDbContextTransaction)null!, out newDbContextTransaction!, TransactionUsage.CreateNew, transactionIsolationLevel);
	//}

	//public static TContext CreateNewDbContextWithoutTransaction<TContext>(
	//	IServiceProvider serviceProvider,
	//	DbConnection? externalDbConnection = null,
	//	string? connectionString = null,
	//	string? traceFrame = null,
	//	Guid? idCommandQuery = null)
	//	where TContext : DbContext
	//{
	//	Throw.ArgumentNull(serviceProvider);

	//	var dbContext = serviceProvider.GetRequiredService<TContext>();
	//	if (dbContext is DbContextBase dbContextBase)
	//	{
	//		dbContextBase.TraceFrame = traceFrame;
	//		dbContextBase.IdCommandQuery = idCommandQuery;
	//		dbContextBase.Initialize(
	//			externalDbConnection,
	//			connectionString,
	//			null);
	//	}

	//	return dbContext;
	//}

	public static TContext SetDbTransaction<TContext>(
		IScopeContext scopeContext,
		TContext dbContext,
		IDbContextTransaction? existingDbContextTransaction,
		out IDbContextTransaction? newDbContextTransaction,
		TransactionUsage transactionUsage,
		IsolationLevel? transactionIsolationLevel)
		where TContext : DbContext
	{
		newDbContextTransaction = null;

		Throw.IfArgumentNull(dbContext);

		if (transactionUsage == TransactionUsage.NONE)
			return dbContext;

		if (transactionUsage == TransactionUsage.Reuse)
		{
			if (existingDbContextTransaction == null)
			{
				Throw.IfArgumentNull(existingDbContextTransaction, scopeContext);

				//if (transactionIsolationLevel.HasValue)
				//{
				//	newDbContextTransaction = dbContext.Database.BeginTransaction(transactionIsolationLevel.Value);
				//}
				//else
				//{
				//	newDbContextTransaction = dbContext.Database.BeginTransaction();
				//}

				//return dbContext;
			}
			else
			{
				if (dbContext.Database.CurrentTransaction == null)
				{
					newDbContextTransaction = existingDbContextTransaction;
					dbContext.Database.UseTransaction(newDbContextTransaction.GetDbTransaction());
					return dbContext;
				}
				else
				{
					if (dbContext.Database.CurrentTransaction.TransactionId != existingDbContextTransaction.TransactionId)
						Throw.InvalidOperationException(
							ErrorCodes.DbContext.InvalidTransaction(dbContext.Database.CurrentTransaction.TransactionId),
							scopeContext);

					return dbContext;
				}
			}
		}

		if (transactionUsage == TransactionUsage.CreateNew)
		{
			if (dbContext.Database.CurrentTransaction == null)
			{
				if (transactionIsolationLevel.HasValue)
				{
					newDbContextTransaction = dbContext.Database.BeginTransaction(transactionIsolationLevel.Value);
				}
				else
				{
					newDbContextTransaction = dbContext.Database.BeginTransaction();
				}

				return dbContext;
			}
			else
			{
				Throw.InvalidOperationException(
					ErrorCodes.DbContext.InvalidTransaction(dbContext.Database.CurrentTransaction.TransactionId),
					scopeContext);
			}
		}

		return dbContext;
	}

	public static TContext SetDbTransaction<TContext>(
		IScopeContext scopeContext,
		TContext dbContext,
		DbTransaction? existingTransaction,
		out IDbContextTransaction? newDbContextTransaction,
		TransactionUsage transactionUsage,
		IsolationLevel? transactionIsolationLevel)
		where TContext : DbContext
	{
		newDbContextTransaction = null;

		Throw.IfArgumentNull(dbContext);

		if (transactionUsage == TransactionUsage.NONE)
			return dbContext;

		if (transactionUsage == TransactionUsage.Reuse)
		{
			if (existingTransaction == null)
			{
				throw new ArgumentNullException(nameof(existingTransaction));

				//if (transactionIsolationLevel.HasValue)
				//{
				//	newDbContextTransaction = dbContext.Database.BeginTransaction(transactionIsolationLevel.Value);
				//}
				//else
				//{
				//	newDbContextTransaction = dbContext.Database.BeginTransaction();
				//}

				//return dbContext;
			}
			else
			{
				if (dbContext.Database.CurrentTransaction == null)
				{
					dbContext.Database.UseTransaction(existingTransaction);
					newDbContextTransaction = dbContext.Database.CurrentTransaction;
					return dbContext;
				}
				else
				{
					if (dbContext.Database.CurrentTransaction.GetDbTransaction() != existingTransaction)
						throw new InvalidOperationException($"DbContext already has set another transaction with id {dbContext.Database.CurrentTransaction.TransactionId}");

					return dbContext;
				}
			}
		}

		if (transactionUsage == TransactionUsage.CreateNew)
		{
			if (dbContext.Database.CurrentTransaction == null)
			{
				if (transactionIsolationLevel.HasValue)
				{
					newDbContextTransaction = dbContext.Database.BeginTransaction(transactionIsolationLevel.Value);
				}
				else
				{
					newDbContextTransaction = dbContext.Database.BeginTransaction();
				}

				return dbContext;
			}
			else
			{
				throw new InvalidOperationException($"DbContext already has set another transaction with id {dbContext.Database.CurrentTransaction.TransactionId}");
			}
		}

		return dbContext;
	}

	public static void ReSetDbTransaction(
		IScopeContext scopeContext,
		IDbContext dbContext,
		out IDbContextTransaction newDbContextTransaction,
		IsolationLevel? transactionIsolationLevel)
	{
		scopeContext = scopeContext.CreateNew();

		Throw.IfArgumentNull(dbContext, scopeContext);

		if (transactionIsolationLevel.HasValue)
		{
			newDbContextTransaction = dbContext.Database.BeginTransaction(transactionIsolationLevel.Value);
		}
		else
		{
			newDbContextTransaction = dbContext.Database.BeginTransaction();
		}
	}
}
