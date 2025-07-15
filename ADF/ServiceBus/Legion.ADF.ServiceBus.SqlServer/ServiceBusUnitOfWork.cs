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

namespace Legion.ADF.ServiceBus.SqlServer;

internal partial class ServiceBusUnitOfWork : Legion.ADF.ServiceBus.IServiceBusUnitOfWork, Legion.EntityFrameworkCore.Model.Repositories.IDbUnitOfWork, Legion.Model.Repositories.IUnitOfWork, IDisposable, IAsyncDisposable
{
	private bool _isInternalConnectionProvider;
	private bool _disposed;

#if TRACK_OBJECTS
	public Guid IdServiceBusUnitOfWork { get; }
#endif

	public IEFConnectionProvider ConnectionProvider { get; }
	Legion.Database.IConnectionProvider Legion.Model.Repositories.IUnitOfWork.ConnectionProvider => ConnectionProvider;
	public IServiceProvider ServiceProvider => ConnectionProvider.ServiceProvider;

	public ILogger Logger => ConnectionProvider.Logger;
	
	public ServiceBusUnitOfWork(IEFConnectionProvider connectionProvider)
	{
#if TRACK_OBJECTS
		IdServiceBusUnitOfWork = Legion.GlobalContext.Instance.NewGuid();
		Trackers.ObjectLifetimeTracker.Track(this, IdServiceBusUnitOfWork.ToString());
#endif

		Throw.IfArgumentNull(connectionProvider);

		ConnectionProvider = connectionProvider;
		_isInternalConnectionProvider = false; //disposed by caller
	}

	public ServiceBusUnitOfWork(Legion.EntityFrameworkCore.Model.Repositories.IDbUnitOfWork dbUnitOfWork)
	{
#if TRACK_OBJECTS
		IdServiceBusUnitOfWork = Legion.GlobalContext.Instance.NewGuid();
		Trackers.ObjectLifetimeTracker.Track(this, IdServiceBusUnitOfWork.ToString());
#endif

		Throw.IfArgumentNull(dbUnitOfWork);

		ConnectionProvider = dbUnitOfWork.ConnectionProvider;
		_isInternalConnectionProvider = false; //disposed by dbUnitOfWork
	}

	public ServiceBusUnitOfWork(Legion.EntityFrameworkCore.Model.Repositories.IDbQueryUnitOfWork dbQueryUnitOfWork)
	{
#if TRACK_OBJECTS
		IdServiceBusUnitOfWork = Legion.GlobalContext.Instance.NewGuid();
		Trackers.ObjectLifetimeTracker.Track(this, IdServiceBusUnitOfWork.ToString());
#endif

		Throw.IfArgumentNull(dbQueryUnitOfWork);

		ConnectionProvider = dbQueryUnitOfWork.ConnectionProvider;
		_isInternalConnectionProvider = false; //disposed by dbQueryUnitOfWork
	}

	public ServiceBusUnitOfWork(
		IServiceProvider serviceProvider,
		string connectionStirng,
		System.Data.IsolationLevel? isolationLevel,
		bool? allowLocking,
		bool createAuditEntryStore)
	{
#if TRACK_OBJECTS
		IdServiceBusUnitOfWork = Legion.GlobalContext.Instance.NewGuid();
		Trackers.ObjectLifetimeTracker.Track(this, IdServiceBusUnitOfWork.ToString());
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

	public ServiceBusUnitOfWork(
		IServiceProvider serviceProvider,
		string connectionStirng,
		bool? allowLocking,
		bool createAuditEntryStore)
	{
#if TRACK_OBJECTS
		IdServiceBusUnitOfWork = Legion.GlobalContext.Instance.NewGuid();
		Trackers.ObjectLifetimeTracker.Track(this, IdServiceBusUnitOfWork.ToString());
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
	protected Legion.ADF.ServiceBus.SqlServer.IServiceBusDbContext GetContext(IScopeContext scopeContext)
		=> ConnectionProvider.GetOrCreateDbContext<Legion.ADF.ServiceBus.SqlServer.IServiceBusDbContext>(scopeContext);

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

	private Legion.ADF.ServiceBus.Model.Repositories.IHostRepository? host;
	public Legion.ADF.ServiceBus.Model.Repositories.IHostRepository HostRepository
		=> host ??= new Legion.ADF.ServiceBus.SqlServer.Model.Repositories.HostRepository(ConnectionProvider);


	private Legion.ADF.ServiceBus.Model.Repositories.IHostActivityRepository? hostActivity;
	public Legion.ADF.ServiceBus.Model.Repositories.IHostActivityRepository HostActivityRepository
		=> hostActivity ??= new Legion.ADF.ServiceBus.SqlServer.Model.Repositories.HostActivityRepository(ConnectionProvider);


	private Legion.ADF.ServiceBus.Model.Repositories.IHostLogRepository? hostLog;
	public Legion.ADF.ServiceBus.Model.Repositories.IHostLogRepository HostLogRepository
		=> hostLog ??= new Legion.ADF.ServiceBus.SqlServer.Model.Repositories.HostLogRepository(ConnectionProvider);


	private Legion.ADF.ServiceBus.Model.Repositories.IJobRepository? job;
	public Legion.ADF.ServiceBus.Model.Repositories.IJobRepository JobRepository
		=> job ??= new Legion.ADF.ServiceBus.SqlServer.Model.Repositories.JobRepository(ConnectionProvider);


	private Legion.ADF.ServiceBus.Model.Repositories.IJobActivityRepository? jobActivity;
	public Legion.ADF.ServiceBus.Model.Repositories.IJobActivityRepository JobActivityRepository
		=> jobActivity ??= new Legion.ADF.ServiceBus.SqlServer.Model.Repositories.JobActivityRepository(ConnectionProvider);


	private Legion.ADF.ServiceBus.Model.Repositories.IJobDataRepository? jobData;
	public Legion.ADF.ServiceBus.Model.Repositories.IJobDataRepository JobDataRepository
		=> jobData ??= new Legion.ADF.ServiceBus.SqlServer.Model.Repositories.JobDataRepository(ConnectionProvider);


	private Legion.ADF.ServiceBus.Model.Repositories.IJobExecutionRepository? jobExecution;
	public Legion.ADF.ServiceBus.Model.Repositories.IJobExecutionRepository JobExecutionRepository
		=> jobExecution ??= new Legion.ADF.ServiceBus.SqlServer.Model.Repositories.JobExecutionRepository(ConnectionProvider);


	private Legion.ADF.ServiceBus.Model.Repositories.IJobLogRepository? jobLog;
	public Legion.ADF.ServiceBus.Model.Repositories.IJobLogRepository JobLogRepository
		=> jobLog ??= new Legion.ADF.ServiceBus.SqlServer.Model.Repositories.JobLogRepository(ConnectionProvider);


	private Legion.ADF.ServiceBus.Model.Repositories.IJobMessageRepository? jobMessage;
	public Legion.ADF.ServiceBus.Model.Repositories.IJobMessageRepository JobMessageRepository
		=> jobMessage ??= new Legion.ADF.ServiceBus.SqlServer.Model.Repositories.JobMessageRepository(ConnectionProvider);


	private Legion.ADF.ServiceBus.Model.Repositories.IJobMessageTypeRepository? jobMessageType;
	public Legion.ADF.ServiceBus.Model.Repositories.IJobMessageTypeRepository JobMessageTypeRepository
		=> jobMessageType ??= new Legion.ADF.ServiceBus.SqlServer.Model.Repositories.JobMessageTypeRepository(ConnectionProvider);


	private Legion.ADF.ServiceBus.Model.Repositories.IJobRunTypeRepository? jobRunType;
	public Legion.ADF.ServiceBus.Model.Repositories.IJobRunTypeRepository JobRunTypeRepository
		=> jobRunType ??= new Legion.ADF.ServiceBus.SqlServer.Model.Repositories.JobRunTypeRepository(ConnectionProvider);


	private Legion.ADF.ServiceBus.Model.Repositories.IJobStatisticsRepository? jobStatistics;
	public Legion.ADF.ServiceBus.Model.Repositories.IJobStatisticsRepository JobStatisticsRepository
		=> jobStatistics ??= new Legion.ADF.ServiceBus.SqlServer.Model.Repositories.JobStatisticsRepository(ConnectionProvider);


	private Legion.ADF.ServiceBus.Model.Repositories.IJobStatusRepository? jobStatus;
	public Legion.ADF.ServiceBus.Model.Repositories.IJobStatusRepository JobStatusRepository
		=> jobStatus ??= new Legion.ADF.ServiceBus.SqlServer.Model.Repositories.JobStatusRepository(ConnectionProvider);


	private Legion.ADF.ServiceBus.Model.Repositories.IOrchestrationRepository? orchestration;
	public Legion.ADF.ServiceBus.Model.Repositories.IOrchestrationRepository OrchestrationRepository
		=> orchestration ??= new Legion.ADF.ServiceBus.SqlServer.Model.Repositories.OrchestrationRepository(ConnectionProvider);


	private Legion.ADF.ServiceBus.Model.Repositories.IOrchestrationInstanceRepository? orchestrationInstance;
	public Legion.ADF.ServiceBus.Model.Repositories.IOrchestrationInstanceRepository OrchestrationInstanceRepository
		=> orchestrationInstance ??= new Legion.ADF.ServiceBus.SqlServer.Model.Repositories.OrchestrationInstanceRepository(ConnectionProvider);


	private Legion.ADF.ServiceBus.Model.Repositories.IOrchestrationStatusRepository? orchestrationStatus;
	public Legion.ADF.ServiceBus.Model.Repositories.IOrchestrationStatusRepository OrchestrationStatusRepository
		=> orchestrationStatus ??= new Legion.ADF.ServiceBus.SqlServer.Model.Repositories.OrchestrationStatusRepository(ConnectionProvider);


	private Legion.ADF.ServiceBus.Model.Repositories.IOrchestrationStepRepository? orchestrationStep;
	public Legion.ADF.ServiceBus.Model.Repositories.IOrchestrationStepRepository OrchestrationStepRepository
		=> orchestrationStep ??= new Legion.ADF.ServiceBus.SqlServer.Model.Repositories.OrchestrationStepRepository(ConnectionProvider);


	private Legion.ADF.ServiceBus.Model.Repositories.IOrchestrationStepProcessingRepository? orchestrationStepProcessing;
	public Legion.ADF.ServiceBus.Model.Repositories.IOrchestrationStepProcessingRepository OrchestrationStepProcessingRepository
		=> orchestrationStepProcessing ??= new Legion.ADF.ServiceBus.SqlServer.Model.Repositories.OrchestrationStepProcessingRepository(ConnectionProvider);


	private Legion.ADF.ServiceBus.Model.Repositories.IOrchestrationStepProcessingDirectionRepository? orchestrationStepProcessingDirection;
	public Legion.ADF.ServiceBus.Model.Repositories.IOrchestrationStepProcessingDirectionRepository OrchestrationStepProcessingDirectionRepository
		=> orchestrationStepProcessingDirection ??= new Legion.ADF.ServiceBus.SqlServer.Model.Repositories.OrchestrationStepProcessingDirectionRepository(ConnectionProvider);


	private Legion.ADF.ServiceBus.Model.Repositories.IOrchestrationStepProcessingLogRepository? orchestrationStepProcessingLog;
	public Legion.ADF.ServiceBus.Model.Repositories.IOrchestrationStepProcessingLogRepository OrchestrationStepProcessingLogRepository
		=> orchestrationStepProcessingLog ??= new Legion.ADF.ServiceBus.SqlServer.Model.Repositories.OrchestrationStepProcessingLogRepository(ConnectionProvider);


	private Legion.ADF.ServiceBus.Model.Repositories.IOrchestrationStepProcessingMessageRepository? orchestrationStepProcessingMessage;
	public Legion.ADF.ServiceBus.Model.Repositories.IOrchestrationStepProcessingMessageRepository OrchestrationStepProcessingMessageRepository
		=> orchestrationStepProcessingMessage ??= new Legion.ADF.ServiceBus.SqlServer.Model.Repositories.OrchestrationStepProcessingMessageRepository(ConnectionProvider);


	private Legion.ADF.ServiceBus.Model.Repositories.IOrchestrationStepProcessingMessageTypeRepository? orchestrationStepProcessingMessageType;
	public Legion.ADF.ServiceBus.Model.Repositories.IOrchestrationStepProcessingMessageTypeRepository OrchestrationStepProcessingMessageTypeRepository
		=> orchestrationStepProcessingMessageType ??= new Legion.ADF.ServiceBus.SqlServer.Model.Repositories.OrchestrationStepProcessingMessageTypeRepository(ConnectionProvider);


	private Legion.ADF.ServiceBus.Model.Repositories.IOrchestrationStepProcessingStatusRepository? orchestrationStepProcessingStatus;
	public Legion.ADF.ServiceBus.Model.Repositories.IOrchestrationStepProcessingStatusRepository OrchestrationStepProcessingStatusRepository
		=> orchestrationStepProcessingStatus ??= new Legion.ADF.ServiceBus.SqlServer.Model.Repositories.OrchestrationStepProcessingStatusRepository(ConnectionProvider);

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
		Trackers.ObjectLifetimeTracker.SetDisposed(this, IdServiceBusUnitOfWork.ToString());
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
		Trackers.ObjectLifetimeTracker.SetDisposed(this, IdServiceBusUnitOfWork.ToString());
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
