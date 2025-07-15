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

namespace Legion.ADF.Messaging.MessageBox.PostgreSQL;

internal partial class MessageBoxUnitOfWork : Legion.ADF.Messaging.MessageBox.IMessageBoxUnitOfWork, Legion.EntityFrameworkCore.Model.Repositories.IDbUnitOfWork, Legion.Model.Repositories.IUnitOfWork, IDisposable, IAsyncDisposable
{
	private bool _isInternalConnectionProvider;
	private bool _disposed;

#if TRACK_OBJECTS
	public Guid IdMessageBoxUnitOfWork { get; }
#endif

	public IEFConnectionProvider ConnectionProvider { get; }
	Legion.Database.IConnectionProvider Legion.Model.Repositories.IUnitOfWork.ConnectionProvider => ConnectionProvider;
	public IServiceProvider ServiceProvider => ConnectionProvider.ServiceProvider;

	public ILogger Logger => ConnectionProvider.Logger;
	
	public MessageBoxUnitOfWork(IEFConnectionProvider connectionProvider)
	{
#if TRACK_OBJECTS
		IdMessageBoxUnitOfWork = Legion.GlobalContext.Instance.NewGuid();
		Trackers.ObjectLifetimeTracker.Track(this, IdMessageBoxUnitOfWork.ToString());
#endif

		Throw.IfArgumentNull(connectionProvider);

		ConnectionProvider = connectionProvider;
		_isInternalConnectionProvider = false; //disposed by caller
	}

	public MessageBoxUnitOfWork(Legion.EntityFrameworkCore.Model.Repositories.IDbUnitOfWork dbUnitOfWork)
	{
#if TRACK_OBJECTS
		IdMessageBoxUnitOfWork = Legion.GlobalContext.Instance.NewGuid();
		Trackers.ObjectLifetimeTracker.Track(this, IdMessageBoxUnitOfWork.ToString());
#endif

		Throw.IfArgumentNull(dbUnitOfWork);

		ConnectionProvider = dbUnitOfWork.ConnectionProvider;
		_isInternalConnectionProvider = false; //disposed by dbUnitOfWork
	}

	public MessageBoxUnitOfWork(Legion.EntityFrameworkCore.Model.Repositories.IDbQueryUnitOfWork dbQueryUnitOfWork)
	{
#if TRACK_OBJECTS
		IdMessageBoxUnitOfWork = Legion.GlobalContext.Instance.NewGuid();
		Trackers.ObjectLifetimeTracker.Track(this, IdMessageBoxUnitOfWork.ToString());
#endif

		Throw.IfArgumentNull(dbQueryUnitOfWork);

		ConnectionProvider = dbQueryUnitOfWork.ConnectionProvider;
		_isInternalConnectionProvider = false; //disposed by dbQueryUnitOfWork
	}

	public MessageBoxUnitOfWork(
		IServiceProvider serviceProvider,
		string connectionStirng,
		System.Data.IsolationLevel? isolationLevel,
		bool? allowLocking,
		bool createAuditEntryStore)
	{
#if TRACK_OBJECTS
		IdMessageBoxUnitOfWork = Legion.GlobalContext.Instance.NewGuid();
		Trackers.ObjectLifetimeTracker.Track(this, IdMessageBoxUnitOfWork.ToString());
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

	public MessageBoxUnitOfWork(
		IServiceProvider serviceProvider,
		string connectionStirng,
		bool? allowLocking,
		bool createAuditEntryStore)
	{
#if TRACK_OBJECTS
		IdMessageBoxUnitOfWork = Legion.GlobalContext.Instance.NewGuid();
		Trackers.ObjectLifetimeTracker.Track(this, IdMessageBoxUnitOfWork.ToString());
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
	protected Legion.ADF.Messaging.MessageBox.PostgreSQL.IMessageBoxDbContext GetContext(IScopeContext scopeContext)
		=> ConnectionProvider.GetOrCreateDbContext<Legion.ADF.Messaging.MessageBox.PostgreSQL.IMessageBoxDbContext>(scopeContext);

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

	private Legion.ADF.Messaging.MessageBox.Model.Repositories.IBlockedMessageTypeRepository? blockedMessageType;
	public Legion.ADF.Messaging.MessageBox.Model.Repositories.IBlockedMessageTypeRepository BlockedMessageTypeRepository
		=> blockedMessageType ??= new Legion.ADF.Messaging.MessageBox.PostgreSQL.Model.Repositories.BlockedMessageTypeRepository(ConnectionProvider);


	private Legion.ADF.Messaging.MessageBox.Model.Repositories.IMessageRepository? message;
	public Legion.ADF.Messaging.MessageBox.Model.Repositories.IMessageRepository MessageRepository
		=> message ??= new Legion.ADF.Messaging.MessageBox.PostgreSQL.Model.Repositories.MessageRepository(ConnectionProvider);


	private Legion.ADF.Messaging.MessageBox.Model.Repositories.IMessageArchiveRepository? messageArchive;
	public Legion.ADF.Messaging.MessageBox.Model.Repositories.IMessageArchiveRepository MessageArchiveRepository
		=> messageArchive ??= new Legion.ADF.Messaging.MessageBox.PostgreSQL.Model.Repositories.MessageArchiveRepository(ConnectionProvider);


	private Legion.ADF.Messaging.MessageBox.Model.Repositories.IMessageBoxInstanceRepository? messageBoxInstance;
	public Legion.ADF.Messaging.MessageBox.Model.Repositories.IMessageBoxInstanceRepository MessageBoxInstanceRepository
		=> messageBoxInstance ??= new Legion.ADF.Messaging.MessageBox.PostgreSQL.Model.Repositories.MessageBoxInstanceRepository(ConnectionProvider);


	private Legion.ADF.Messaging.MessageBox.Model.Repositories.IMessageBoxProcessingLogRepository? messageBoxProcessingLog;
	public Legion.ADF.Messaging.MessageBox.Model.Repositories.IMessageBoxProcessingLogRepository MessageBoxProcessingLogRepository
		=> messageBoxProcessingLog ??= new Legion.ADF.Messaging.MessageBox.PostgreSQL.Model.Repositories.MessageBoxProcessingLogRepository(ConnectionProvider);


	private Legion.ADF.Messaging.MessageBox.Model.Repositories.IMessageContentRepository? messageContent;
	public Legion.ADF.Messaging.MessageBox.Model.Repositories.IMessageContentRepository MessageContentRepository
		=> messageContent ??= new Legion.ADF.Messaging.MessageBox.PostgreSQL.Model.Repositories.MessageContentRepository(ConnectionProvider);


	private Legion.ADF.Messaging.MessageBox.Model.Repositories.IMessageProcessingLogRepository? messageProcessingLog;
	public Legion.ADF.Messaging.MessageBox.Model.Repositories.IMessageProcessingLogRepository MessageProcessingLogRepository
		=> messageProcessingLog ??= new Legion.ADF.Messaging.MessageBox.PostgreSQL.Model.Repositories.MessageProcessingLogRepository(ConnectionProvider);


	private Legion.ADF.Messaging.MessageBox.Model.Repositories.IMessageProcessingStatusRepository? messageProcessingStatus;
	public Legion.ADF.Messaging.MessageBox.Model.Repositories.IMessageProcessingStatusRepository MessageProcessingStatusRepository
		=> messageProcessingStatus ??= new Legion.ADF.Messaging.MessageBox.PostgreSQL.Model.Repositories.MessageProcessingStatusRepository(ConnectionProvider);


	private Legion.ADF.Messaging.MessageBox.Model.Repositories.IMessageStatusRepository? messageStatus;
	public Legion.ADF.Messaging.MessageBox.Model.Repositories.IMessageStatusRepository MessageStatusRepository
		=> messageStatus ??= new Legion.ADF.Messaging.MessageBox.PostgreSQL.Model.Repositories.MessageStatusRepository(ConnectionProvider);


	private Legion.ADF.Messaging.MessageBox.Model.Repositories.IMessageTypeRepository? messageType;
	public Legion.ADF.Messaging.MessageBox.Model.Repositories.IMessageTypeRepository MessageTypeRepository
		=> messageType ??= new Legion.ADF.Messaging.MessageBox.PostgreSQL.Model.Repositories.MessageTypeRepository(ConnectionProvider);


	private Legion.ADF.Messaging.MessageBox.Model.Repositories.IQueueRepository? queue;
	public Legion.ADF.Messaging.MessageBox.Model.Repositories.IQueueRepository QueueRepository
		=> queue ??= new Legion.ADF.Messaging.MessageBox.PostgreSQL.Model.Repositories.QueueRepository(ConnectionProvider);


	private Legion.ADF.Messaging.MessageBox.Model.Repositories.IQueuedMessageRepository? queuedMessage;
	public Legion.ADF.Messaging.MessageBox.Model.Repositories.IQueuedMessageRepository QueuedMessageRepository
		=> queuedMessage ??= new Legion.ADF.Messaging.MessageBox.PostgreSQL.Model.Repositories.QueuedMessageRepository(ConnectionProvider);


	private Legion.ADF.Messaging.MessageBox.Model.Repositories.IQueueProcessingModeRepository? queueProcessingMode;
	public Legion.ADF.Messaging.MessageBox.Model.Repositories.IQueueProcessingModeRepository QueueProcessingModeRepository
		=> queueProcessingMode ??= new Legion.ADF.Messaging.MessageBox.PostgreSQL.Model.Repositories.QueueProcessingModeRepository(ConnectionProvider);


	private Legion.ADF.Messaging.MessageBox.Model.Repositories.ISubscribedMessageRepository? subscribedMessage;
	public Legion.ADF.Messaging.MessageBox.Model.Repositories.ISubscribedMessageRepository SubscribedMessageRepository
		=> subscribedMessage ??= new Legion.ADF.Messaging.MessageBox.PostgreSQL.Model.Repositories.SubscribedMessageRepository(ConnectionProvider);


	private Legion.ADF.Messaging.MessageBox.Model.Repositories.ITopicRepository? topic;
	public Legion.ADF.Messaging.MessageBox.Model.Repositories.ITopicRepository TopicRepository
		=> topic ??= new Legion.ADF.Messaging.MessageBox.PostgreSQL.Model.Repositories.TopicRepository(ConnectionProvider);


	private Legion.ADF.Messaging.MessageBox.Model.Repositories.ITopicSubscriptionRepository? topicSubscription;
	public Legion.ADF.Messaging.MessageBox.Model.Repositories.ITopicSubscriptionRepository TopicSubscriptionRepository
		=> topicSubscription ??= new Legion.ADF.Messaging.MessageBox.PostgreSQL.Model.Repositories.TopicSubscriptionRepository(ConnectionProvider);

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
		Trackers.ObjectLifetimeTracker.SetDisposed(this, IdMessageBoxUnitOfWork.ToString());
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
		Trackers.ObjectLifetimeTracker.SetDisposed(this, IdMessageBoxUnitOfWork.ToString());
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
