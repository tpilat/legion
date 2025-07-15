using Legion;
using Legion.EntityFrameworkCore;
using Legion.Model.Audit;
using Legion.EntityFrameworkCore.Queries;
using Legion.Exceptions;
using Legion.Transactions;
using Microsoft.Extensions.DependencyInjection;

namespace Legion.ADF.Messaging.Outbox.PostgreSQL;

internal partial class OutboxQueryUnitOfWork : Legion.ADF.Messaging.Outbox.IOutboxQueryUnitOfWork, Legion.EntityFrameworkCore.Model.Repositories.IDbQueryUnitOfWork, Legion.Model.Repositories.IQueryUnitOfWork, IDisposable, IAsyncDisposable
{
	private bool _isInternalConnectionProvider;
	private bool _disposed;

#if TRACK_OBJECTS
	public Guid IdOutboxQueryUnitOfWork { get; }
#endif

	public IEFConnectionProvider ConnectionProvider { get; }
	Legion.Database.IConnectionProvider Legion.Model.Repositories.IQueryUnitOfWork.ConnectionProvider => ConnectionProvider;
	System.IServiceProvider Legion.Model.Repositories.IQueryUnitOfWork.ServiceProvider => ConnectionProvider.ServiceProvider;
	
	public OutboxQueryUnitOfWork(IEFConnectionProvider connectionProvider)
	{
#if TRACK_OBJECTS
		IdOutboxQueryUnitOfWork = Legion.GlobalContext.Instance.NewGuid();
		Trackers.ObjectLifetimeTracker.Track(this, IdOutboxQueryUnitOfWork.ToString());
#endif

		Throw.IfArgumentNull(connectionProvider);

		ConnectionProvider = connectionProvider;
		_isInternalConnectionProvider = false; //disposed by caller
	}

	public OutboxQueryUnitOfWork(Legion.EntityFrameworkCore.Model.Repositories.IDbUnitOfWork dbUnitOfWork)
	{
#if TRACK_OBJECTS
		IdOutboxQueryUnitOfWork = Legion.GlobalContext.Instance.NewGuid();
		Trackers.ObjectLifetimeTracker.Track(this, IdOutboxQueryUnitOfWork.ToString());
#endif

		Throw.IfArgumentNull(dbUnitOfWork);

		ConnectionProvider = dbUnitOfWork.ConnectionProvider;
		_isInternalConnectionProvider = false; //disposed by dbUnitOfWork
	}

	public OutboxQueryUnitOfWork(Legion.EntityFrameworkCore.Model.Repositories.IDbQueryUnitOfWork dbQueryUnitOfWork)
	{
#if TRACK_OBJECTS
		IdOutboxQueryUnitOfWork = Legion.GlobalContext.Instance.NewGuid();
		Trackers.ObjectLifetimeTracker.Track(this, IdOutboxQueryUnitOfWork.ToString());
#endif

		Throw.IfArgumentNull(dbQueryUnitOfWork);

		ConnectionProvider = dbQueryUnitOfWork.ConnectionProvider;
		_isInternalConnectionProvider = false; //disposed by dbQueryUnitOfWork
	}

	public OutboxQueryUnitOfWork(
		IServiceProvider serviceProvider,
		string connectionStirng,
		System.Data.IsolationLevel? isolationLevel,
		bool? allowLocking,
		bool createAuditEntryStore)
	{
#if TRACK_OBJECTS
		IdOutboxQueryUnitOfWork = Legion.GlobalContext.Instance.NewGuid();
		Trackers.ObjectLifetimeTracker.Track(this, IdOutboxQueryUnitOfWork.ToString());
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

	public OutboxQueryUnitOfWork(
		IServiceProvider serviceProvider,
		string connectionStirng,
		bool? allowLocking,
		bool createAuditEntryStore)
	{
#if TRACK_OBJECTS
		IdOutboxQueryUnitOfWork = Legion.GlobalContext.Instance.NewGuid();
		Trackers.ObjectLifetimeTracker.Track(this, IdOutboxQueryUnitOfWork.ToString());
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

	protected Legion.ADF.Messaging.Outbox.PostgreSQL.IOutboxQueryDbContext GetContext(IScopeContext scopeContext)
		=> ConnectionProvider.GetOrCreateDbContext<Legion.ADF.Messaging.Outbox.PostgreSQL.IOutboxQueryDbContext>(scopeContext);

	private Legion.ADF.Messaging.Outbox.Model.Repositories.IVwBlockedOutboxMessageTypeRepository? vwBlockedOutboxMessageType;
	public Legion.ADF.Messaging.Outbox.Model.Repositories.IVwBlockedOutboxMessageTypeRepository VwBlockedOutboxMessageTypeRepository
		=> vwBlockedOutboxMessageType ??= new Legion.ADF.Messaging.Outbox.PostgreSQL.Model.Repositories.VwBlockedOutboxMessageTypeRepository(ConnectionProvider);


	private Legion.ADF.Messaging.Outbox.Model.Repositories.IVwOutboxMessageRepository? vwOutboxMessage;
	public Legion.ADF.Messaging.Outbox.Model.Repositories.IVwOutboxMessageRepository VwOutboxMessageRepository
		=> vwOutboxMessage ??= new Legion.ADF.Messaging.Outbox.PostgreSQL.Model.Repositories.VwOutboxMessageRepository(ConnectionProvider);


	private Legion.ADF.Messaging.Outbox.Model.Repositories.IVwOutboxMessageArchiveRepository? vwOutboxMessageArchive;
	public Legion.ADF.Messaging.Outbox.Model.Repositories.IVwOutboxMessageArchiveRepository VwOutboxMessageArchiveRepository
		=> vwOutboxMessageArchive ??= new Legion.ADF.Messaging.Outbox.PostgreSQL.Model.Repositories.VwOutboxMessageArchiveRepository(ConnectionProvider);


	private Legion.ADF.Messaging.Outbox.Model.Repositories.IVwOutboxMessageContentRepository? vwOutboxMessageContent;
	public Legion.ADF.Messaging.Outbox.Model.Repositories.IVwOutboxMessageContentRepository VwOutboxMessageContentRepository
		=> vwOutboxMessageContent ??= new Legion.ADF.Messaging.Outbox.PostgreSQL.Model.Repositories.VwOutboxMessageContentRepository(ConnectionProvider);


	private Legion.ADF.Messaging.Outbox.Model.Repositories.IVwOutboxMessageProcessingLogRepository? vwOutboxMessageProcessingLog;
	public Legion.ADF.Messaging.Outbox.Model.Repositories.IVwOutboxMessageProcessingLogRepository VwOutboxMessageProcessingLogRepository
		=> vwOutboxMessageProcessingLog ??= new Legion.ADF.Messaging.Outbox.PostgreSQL.Model.Repositories.VwOutboxMessageProcessingLogRepository(ConnectionProvider);


	private Legion.ADF.Messaging.Outbox.Model.Repositories.IVwOutboxQueueRepository? vwOutboxQueue;
	public Legion.ADF.Messaging.Outbox.Model.Repositories.IVwOutboxQueueRepository VwOutboxQueueRepository
		=> vwOutboxQueue ??= new Legion.ADF.Messaging.Outbox.PostgreSQL.Model.Repositories.VwOutboxQueueRepository(ConnectionProvider);


	private Legion.ADF.Messaging.Outbox.Model.Repositories.IVwOutboxQueueMessagesRepository? vwOutboxQueueMessages;
	public Legion.ADF.Messaging.Outbox.Model.Repositories.IVwOutboxQueueMessagesRepository VwOutboxQueueMessagesRepository
		=> vwOutboxQueueMessages ??= new Legion.ADF.Messaging.Outbox.PostgreSQL.Model.Repositories.VwOutboxQueueMessagesRepository(ConnectionProvider);

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
		Trackers.ObjectLifetimeTracker.SetDisposed(this, IdOutboxQueryUnitOfWork.ToString());
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
		Trackers.ObjectLifetimeTracker.SetDisposed(this, IdOutboxQueryUnitOfWork.ToString());
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
