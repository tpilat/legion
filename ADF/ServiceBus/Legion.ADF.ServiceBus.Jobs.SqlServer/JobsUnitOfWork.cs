using Legion;
using Legion.Database;
using Legion.EntityFrameworkCore;
using Legion.Model.Audit;
using Legion.EntityFrameworkCore.Queries;
using Legion.Exceptions;
using Legion.Model;
using Legion.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;

namespace Legion.ADF.ServiceBus.Jobs.SqlServer;

internal partial class JobsUnitOfWork : Legion.ADF.ServiceBus.Jobs.IJobsUnitOfWork, Legion.EntityFrameworkCore.Model.Repositories.IDbUnitOfWork, Legion.Model.Repositories.IUnitOfWork, IDisposable, IAsyncDisposable
{
	private bool _isInternalConnectionProvider;
	private bool _disposed;

#if TRACK_OBJECTS
	public Guid IdJobsUnitOfWork { get; }
#endif

	public IEFConnectionProvider ConnectionProvider { get; }
	Legion.Database.IConnectionProvider Legion.Model.Repositories.IUnitOfWork.ConnectionProvider => ConnectionProvider;
	public IServiceProvider ServiceProvider => ConnectionProvider.ServiceProvider;

	public ILogger Logger => ConnectionProvider.Logger;
	
	public JobsUnitOfWork(IEFConnectionProvider connectionProvider)
	{
#if TRACK_OBJECTS
		IdJobsUnitOfWork = Guid.NewGuid();
		Trackers.ObjectLifetimeTracker.Track(this, IdJobsUnitOfWork.ToString());
#endif

		Throw.IfArgumentNull(connectionProvider);

		ConnectionProvider = connectionProvider;
		_isInternalConnectionProvider = false; //disposed by caller
	}

	public JobsUnitOfWork(Legion.EntityFrameworkCore.Model.Repositories.IDbUnitOfWork dbUnitOfWork)
	{
#if TRACK_OBJECTS
		IdJobsUnitOfWork = Guid.NewGuid();
		Trackers.ObjectLifetimeTracker.Track(this, IdJobsUnitOfWork.ToString());
#endif

		Throw.IfArgumentNull(dbUnitOfWork);

		ConnectionProvider = dbUnitOfWork.ConnectionProvider;
		_isInternalConnectionProvider = false; //disposed by dbUnitOfWork
	}

	public JobsUnitOfWork(Legion.EntityFrameworkCore.Model.Repositories.IDbQueryUnitOfWork dbQueryUnitOfWork)
	{
#if TRACK_OBJECTS
		IdJobsUnitOfWork = Guid.NewGuid();
		Trackers.ObjectLifetimeTracker.Track(this, IdJobsUnitOfWork.ToString());
#endif

		Throw.IfArgumentNull(dbQueryUnitOfWork);

		ConnectionProvider = dbQueryUnitOfWork.ConnectionProvider;
		_isInternalConnectionProvider = false; //disposed by dbQueryUnitOfWork
	}

	public JobsUnitOfWork(
		IServiceProvider serviceProvider,
		string connectionStirng,
		System.Data.IsolationLevel? isolationLevel,
		bool? allowLocking,
		bool createAuditEntryStore)
	{
#if TRACK_OBJECTS
		IdJobsUnitOfWork = Guid.NewGuid();
		Trackers.ObjectLifetimeTracker.Track(this, IdJobsUnitOfWork.ToString());
#endif

		Throw.IfArgumentNull(serviceProvider);
		Throw.IfArgumentNullOrWhiteSpace(connectionStirng);

		var connectionProviderFactory = serviceProvider.GetRequiredService<IEFConnectionProviderFactory>();
		ConnectionProvider = connectionProviderFactory.CreateWithNewTransaction(
			serviceProvider,
			connectionStirng,
			isolationLevel,
			allowLocking,
			createAuditEntryStore);
		_isInternalConnectionProvider = true;
	}

	public JobsUnitOfWork(
		IServiceProvider serviceProvider,
		string connectionStirng,
		bool? allowLocking,
		bool createAuditEntryStore)
	{
#if TRACK_OBJECTS
		IdJobsUnitOfWork = Guid.NewGuid();
		Trackers.ObjectLifetimeTracker.Track(this, IdJobsUnitOfWork.ToString());
#endif

		Throw.IfArgumentNull(serviceProvider);
		Throw.IfArgumentNullOrWhiteSpace(connectionStirng);

		var connectionProviderFactory = serviceProvider.GetRequiredService<IEFConnectionProviderFactory>();
		ConnectionProvider = connectionProviderFactory.CreateWithoutTransaction(
			serviceProvider,
			connectionStirng,
			allowLocking,
			createAuditEntryStore);
		_isInternalConnectionProvider = true;
	}
	protected Legion.ADF.ServiceBus.Jobs.SqlServer.IJobsDbContext GetContext(IScopeContext scopeContext)
		=> ConnectionProvider.GetOrCreateDbContext<Legion.ADF.ServiceBus.Jobs.SqlServer.IJobsDbContext>(scopeContext);

	public virtual IResult<int> Save(
		IScopeContext scopeContext,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
		=> Save(
			scopeContext,
			false,
			memberName,
			sourceFilePath,
			sourceLineNumber);

	public virtual IResult<int> Save(
		IScopeContext scopeContext,
		bool autoCommitAllTransactions,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
	{
		var result = new ResultBuilder<int>();

		try
		{
			var dbContext = GetContext(scopeContext);

			if (result.IsNull(scopeContext, dbContext))
				return result.Build();

			var saveResult = dbContext.Save(scopeContext.CreateNew(false, memberName, sourceFilePath, sourceLineNumber));

			if (autoCommitAllTransactions)
			{
				if (result.IsNull(scopeContext, ConnectionProvider.TransactionsController))
					return result.Build();

				var commitResult = ConnectionProvider.CommitAll(scopeContext, TransactionsControllerStatus.NotIdle);
				if (result.MergeHasError(commitResult))
					return result.Build();
			}

			return result.WithData(saveResult).Build();
		}
		catch (Exception ex)
		{
			result.WithError(scopeContext, null, x => x.ExceptionInfo(ex));

			if (autoCommitAllTransactions && ConnectionProvider.TransactionsController != null)
			{
				var rollbackResult = ConnectionProvider.RollbackAll(scopeContext, ex, TransactionsControllerStatus.CommitInProgress);
				if (result.MergeHasError(rollbackResult))
					return result.Build();
			}

			return result.Build();
		}
	}

	public virtual IResult<int> Save(
		IScopeContext scopeContext,
		bool autoCommitAllTransactions,
		SaveOptions? options,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
	{
		var result = new ResultBuilder<int>();

		try
		{
			var dbContext = GetContext(scopeContext);

			if (result.IsNull(scopeContext, dbContext))
				return result.Build();

			var saveResult = dbContext.Save(scopeContext.CreateNew(false, memberName, sourceFilePath, sourceLineNumber), options);

			if (autoCommitAllTransactions)
			{
				if (result.IsNull(scopeContext, ConnectionProvider.TransactionsController))
					return result.Build();

				var commitResult = ConnectionProvider.CommitAll(scopeContext, TransactionsControllerStatus.NotIdle);
				if (result.MergeHasError(commitResult))
					return result.Build();
			}

			return result.WithData(saveResult).Build();
		}
		catch (Exception ex)
		{
			result.WithError(scopeContext, null, x => x.ExceptionInfo(ex));

			if (autoCommitAllTransactions && ConnectionProvider.TransactionsController != null)
			{
				var rollbackResult = ConnectionProvider.RollbackAll(scopeContext, ex, TransactionsControllerStatus.CommitInProgress);
				if (result.MergeHasError(rollbackResult))
					return result.Build();
			}

			return result.Build();
		}
	}

	public virtual IResult<int> Save(
		IScopeContext scopeContext,
		bool autoCommitAllTransactions,
		bool acceptAllChangesOnSuccess,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
	{
		var result = new ResultBuilder<int>();

		try
		{
			var dbContext = GetContext(scopeContext);

			if (result.IsNull(scopeContext, dbContext))
				return result.Build();

			var saveResult = dbContext.Save(scopeContext.CreateNew(false, memberName, sourceFilePath, sourceLineNumber), acceptAllChangesOnSuccess);

			if (autoCommitAllTransactions)
			{
				if (result.IsNull(scopeContext, ConnectionProvider.TransactionsController))
					return result.Build();

				var commitResult = ConnectionProvider.CommitAll(scopeContext, TransactionsControllerStatus.NotIdle);
				if (result.MergeHasError(commitResult))
					return result.Build();
			}

			return result.WithData(saveResult).Build();
		}
		catch (Exception ex)
		{
			result.WithError(scopeContext, null, x => x.ExceptionInfo(ex));

			if (autoCommitAllTransactions && ConnectionProvider.TransactionsController != null)
			{
				var rollbackResult = ConnectionProvider.RollbackAll(scopeContext, ex, TransactionsControllerStatus.CommitInProgress);
				if (result.MergeHasError(rollbackResult))
					return result.Build();
			}

			return result.Build();
		}
	}

	public virtual IResult<int> Save(
		IScopeContext scopeContext,
		bool autoCommitAllTransactions,
		bool acceptAllChangesOnSuccess,
		SaveOptions? options,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
	{
		var result = new ResultBuilder<int>();

		try
		{
			var dbContext = GetContext(scopeContext);

			if (result.IsNull(scopeContext, dbContext))
				return result.Build();

			var saveResult = dbContext.Save(scopeContext.CreateNew(false, memberName, sourceFilePath, sourceLineNumber), acceptAllChangesOnSuccess, options);

			if (autoCommitAllTransactions)
			{
				if (result.IsNull(scopeContext, ConnectionProvider.TransactionsController))
					return result.Build();

				var commitResult = ConnectionProvider.CommitAll(scopeContext, TransactionsControllerStatus.NotIdle);
				if (result.MergeHasError(commitResult))
					return result.Build();
			}

			return result.WithData(saveResult).Build();
		}
		catch (Exception ex)
		{
			result.WithError(scopeContext, null, x => x.ExceptionInfo(ex));

			if (autoCommitAllTransactions && ConnectionProvider.TransactionsController != null)
			{
				var rollbackResult = ConnectionProvider.RollbackAll(scopeContext, ex, TransactionsControllerStatus.CommitInProgress);
				if (result.MergeHasError(rollbackResult))
					return result.Build();
			}

			return result.Build();
		}
	}

	public virtual Task<IResult<int>> SaveAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
		=> SaveAsync(
			scopeContext,
			false,
			cancellationToken,
			memberName,
			sourceFilePath,
			sourceLineNumber);

	public virtual async Task<IResult<int>> SaveAsync(
		IScopeContext scopeContext,
		bool autoCommitAllTransactions,
		CancellationToken cancellationToken = default,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
	{
		var result = new ResultBuilder<int>();

		try
		{
			var dbContext = GetContext(scopeContext);
			
			if (result.IsNull(scopeContext, dbContext))
				return result.Build();

			var saveResult = await dbContext.SaveAsync(scopeContext.CreateNew(false, memberName, sourceFilePath, sourceLineNumber), cancellationToken);

			if (autoCommitAllTransactions)
			{
				if (result.IsNull(scopeContext, ConnectionProvider.TransactionsController))
					return result.Build();

				var commitResult = await ConnectionProvider.CommitAllAsync(scopeContext, TransactionsControllerStatus.NotIdle, cancellationToken);
				if (result.MergeHasError(commitResult))
					return result.Build();
			}

			return result.WithData(saveResult).Build();
		}
		catch (Exception ex)
		{
			result.WithError(scopeContext, null, x => x.ExceptionInfo(ex));

			if (autoCommitAllTransactions && ConnectionProvider.TransactionsController != null)
			{
				var rollbackResult = await ConnectionProvider.RollbackAllAsync(scopeContext, ex, TransactionsControllerStatus.CommitInProgress);
				if (result.MergeHasError(rollbackResult))
					return result.Build();
			}

			return result.Build();
		}
	}

	public virtual async Task<IResult<int>> SaveAsync(
		IScopeContext scopeContext,
		bool autoCommitAllTransactions,
		SaveOptions? options,
		CancellationToken cancellationToken = default,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
	{
		var result = new ResultBuilder<int>();

		try
		{
			var dbContext = GetContext(scopeContext);

			if (result.IsNull(scopeContext, dbContext))
				return result.Build();

			var saveResult = await dbContext.SaveAsync(scopeContext.CreateNew(false, memberName, sourceFilePath, sourceLineNumber), options, cancellationToken);

			if (autoCommitAllTransactions)
			{
				if (result.IsNull(scopeContext, ConnectionProvider.TransactionsController))
					return result.Build();

				var commitResult = await ConnectionProvider.CommitAllAsync(scopeContext, TransactionsControllerStatus.NotIdle, cancellationToken);
				if (result.MergeHasError(commitResult))
					return result.Build();
			}

			return result.WithData(saveResult).Build();
		}
		catch (Exception ex)
		{
			result.WithError(scopeContext, null, x => x.ExceptionInfo(ex));

			if (autoCommitAllTransactions && ConnectionProvider.TransactionsController != null)
			{
				var rollbackResult = await ConnectionProvider.RollbackAllAsync(scopeContext, ex, TransactionsControllerStatus.CommitInProgress);
				if (result.MergeHasError(rollbackResult))
					return result.Build();
			}

			return result.Build();
		}
	}

	public virtual async Task<IResult<int>> SaveAsync(
		IScopeContext scopeContext,
		bool autoCommitAllTransactions,
		bool acceptAllChangesOnSuccess,
		CancellationToken cancellationToken = default,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
	{
		var result = new ResultBuilder<int>();

		try
		{
			var dbContext = GetContext(scopeContext);

			if (result.IsNull(scopeContext, dbContext))
				return result.Build();

			var saveResult = await dbContext.SaveAsync(scopeContext.CreateNew(false, memberName, sourceFilePath, sourceLineNumber), acceptAllChangesOnSuccess, cancellationToken);

			if (autoCommitAllTransactions)
			{
				if (result.IsNull(scopeContext, ConnectionProvider.TransactionsController))
					return result.Build();

				var commitResult = await ConnectionProvider.CommitAllAsync(scopeContext, TransactionsControllerStatus.NotIdle, cancellationToken);
				if (result.MergeHasError(commitResult))
					return result.Build();
			}

			return result.WithData(saveResult).Build();
		}
		catch (Exception ex)
		{
			result.WithError(scopeContext, null, x => x.ExceptionInfo(ex));

			if (autoCommitAllTransactions && ConnectionProvider.TransactionsController != null)
			{
				var rollbackResult = await ConnectionProvider.RollbackAllAsync(scopeContext, ex, TransactionsControllerStatus.CommitInProgress);
				if (result.MergeHasError(rollbackResult))
					return result.Build();
			}

			return result.Build();
		}
	}

	public virtual async Task<IResult<int>> SaveAsync(
		IScopeContext scopeContext,
		bool autoCommitAllTransactions,
		bool acceptAllChangesOnSuccess,
		SaveOptions? options,
		CancellationToken cancellationToken = default,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
	{
		var result = new ResultBuilder<int>();

		try
		{
			var dbContext = GetContext(scopeContext);

			if (result.IsNull(scopeContext, dbContext))
				return result.Build();

			var saveResult = await dbContext.SaveAsync(scopeContext.CreateNew(false, memberName, sourceFilePath, sourceLineNumber), acceptAllChangesOnSuccess, options, cancellationToken);

			if (autoCommitAllTransactions)
			{
				if (result.IsNull(scopeContext, ConnectionProvider.TransactionsController))
					return result.Build();

				var commitResult = await ConnectionProvider.CommitAllAsync(scopeContext, TransactionsControllerStatus.NotIdle, cancellationToken);
				if (result.MergeHasError(commitResult))
					return result.Build();
			}

			return result.WithData(saveResult).Build();
		}
		catch (Exception ex)
		{
			result.WithError(scopeContext, null, x => x.ExceptionInfo(ex));

			if (autoCommitAllTransactions && ConnectionProvider.TransactionsController != null)
			{
				var rollbackResult = await ConnectionProvider.RollbackAllAsync(scopeContext, ex, TransactionsControllerStatus.CommitInProgress);
				if (result.MergeHasError(rollbackResult))
					return result.Build();
			}

			return result.Build();
		}
	}

	public async Task<int> ExecuteSqlInterpolatedAsync(
		IScopeContext scopeContext,
		FormattableString sql,
		CancellationToken cancellationToken = default)
	{
		var dbContext = GetContext(scopeContext);
		return await dbContext.Database.ExecuteSqlInterpolatedAsync(sql, cancellationToken);
	}

	public async Task<int> ExecuteSqlRawAsync(
		IScopeContext scopeContext,
		string sql,
		IEnumerable<object> parameters,
		CancellationToken cancellationToken = default)
	{
		var dbContext = GetContext(scopeContext);
		return await dbContext.Database.ExecuteSqlRawAsync(sql, parameters, cancellationToken);
	}

	public int ExecuteSqlInterpolated(
		IScopeContext scopeContext,
		FormattableString sql)
	{
		var dbContext = GetContext(scopeContext);
		return dbContext.Database.ExecuteSqlInterpolated(sql);
	}

	public int ExecuteSqlRaw(
		IScopeContext scopeContext,
		string sql,
		IEnumerable<object> parameters)
	{
		var dbContext = GetContext(scopeContext);
		return dbContext.Database.ExecuteSqlRaw(sql, parameters);
	}

	public virtual IResult<bool?> Commit(
		IScopeContext scopeContext,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
		=> ConnectionProvider.CommitAll(scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber), TransactionsControllerStatus.NotIdle);

	public virtual async Task<IResult<bool?>> CommitAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
		=> await ConnectionProvider.CommitAllAsync(scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber), TransactionsControllerStatus.NotIdle, cancellationToken);

	public virtual IResult<bool?> Rollback(
		IScopeContext scopeContext,
		Exception? exception,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
		=> ConnectionProvider.RollbackAll(scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber), exception, TransactionsControllerStatus.CommitInProgress);

	public virtual async Task<IResult<bool?>> RollbackAsync(
		IScopeContext scopeContext,
		Exception? exception,
		CancellationToken cancellationToken = default,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
		=> await ConnectionProvider.RollbackAllAsync(scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber), exception, TransactionsControllerStatus.CommitInProgress, cancellationToken);

	private Legion.ADF.ServiceBus.Jobs.Model.Repositories.IJobRepository? job;
	public Legion.ADF.ServiceBus.Jobs.Model.Repositories.IJobRepository JobRepository
		=> job ??= new Legion.ADF.ServiceBus.Jobs.SqlServer.Model.Repositories.JobRepository(ConnectionProvider);


	private Legion.ADF.ServiceBus.Jobs.Model.Repositories.IJobDataRepository? jobData;
	public Legion.ADF.ServiceBus.Jobs.Model.Repositories.IJobDataRepository JobDataRepository
		=> jobData ??= new Legion.ADF.ServiceBus.Jobs.SqlServer.Model.Repositories.JobDataRepository(ConnectionProvider);


	private Legion.ADF.ServiceBus.Jobs.Model.Repositories.IJobLogRepository? jobLog;
	public Legion.ADF.ServiceBus.Jobs.Model.Repositories.IJobLogRepository JobLogRepository
		=> jobLog ??= new Legion.ADF.ServiceBus.Jobs.SqlServer.Model.Repositories.JobLogRepository(ConnectionProvider);


	private Legion.ADF.ServiceBus.Jobs.Model.Repositories.IJobMessageRepository? jobMessage;
	public Legion.ADF.ServiceBus.Jobs.Model.Repositories.IJobMessageRepository JobMessageRepository
		=> jobMessage ??= new Legion.ADF.ServiceBus.Jobs.SqlServer.Model.Repositories.JobMessageRepository(ConnectionProvider);


	private Legion.ADF.ServiceBus.Jobs.Model.Repositories.IJobMessageTypeRepository? jobMessageType;
	public Legion.ADF.ServiceBus.Jobs.Model.Repositories.IJobMessageTypeRepository JobMessageTypeRepository
		=> jobMessageType ??= new Legion.ADF.ServiceBus.Jobs.SqlServer.Model.Repositories.JobMessageTypeRepository(ConnectionProvider);


	private Legion.ADF.ServiceBus.Jobs.Model.Repositories.IJobRunTypeRepository? jobRunType;
	public Legion.ADF.ServiceBus.Jobs.Model.Repositories.IJobRunTypeRepository JobRunTypeRepository
		=> jobRunType ??= new Legion.ADF.ServiceBus.Jobs.SqlServer.Model.Repositories.JobRunTypeRepository(ConnectionProvider);


	private Legion.ADF.ServiceBus.Jobs.Model.Repositories.IJobStatusRepository? jobStatus;
	public Legion.ADF.ServiceBus.Jobs.Model.Repositories.IJobStatusRepository JobStatusRepository
		=> jobStatus ??= new Legion.ADF.ServiceBus.Jobs.SqlServer.Model.Repositories.JobStatusRepository(ConnectionProvider);

	public async ValueTask DisposeAsync()
	{
		if (_disposed)
			return;

		_disposed = true;

		await DisposeAsyncCoreAsync().ConfigureAwait(false);

		Dispose(disposing: false);
		GC.SuppressFinalize(this);
	}

	private async ValueTask DisposeAsyncCoreAsync()
	{
#if TRACK_OBJECTS
		Trackers.ObjectLifetimeTracker.SetDisposed(this, IdJobsUnitOfWork.ToString());
#endif

		if (_isInternalConnectionProvider && ConnectionProvider != null)
		{
			await ConnectionProvider.DisposeAsync();
		}
	}

	private void Dispose(bool disposing)
	{
		if (_disposed)
			return;

		_disposed = true;

		if (disposing)
		{
#if TRACK_OBJECTS
		Trackers.ObjectLifetimeTracker.SetDisposed(this, IdJobsUnitOfWork.ToString());
#endif

			if (_isInternalConnectionProvider)
			{
				ConnectionProvider?.Dispose();
			}
		}
	}

	public void Dispose()
	{
		Dispose(true);
		GC.SuppressFinalize(this);
	}
}
