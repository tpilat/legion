using Legion;
using Legion.EntityFrameworkCore;
using Legion.Model.Audit;
using Legion.EntityFrameworkCore.Queries;
using Legion.Exceptions;
using Legion.Transactions;
using Microsoft.Extensions.DependencyInjection;

namespace Legion.ADF.Messaging.Inbox.PostgreSQL;

internal partial class InboxQueryUnitOfWork : Legion.ADF.Messaging.Inbox.IInboxQueryUnitOfWork, Legion.EntityFrameworkCore.Model.Repositories.IDbQueryUnitOfWork, Legion.Model.Repositories.IQueryUnitOfWork, IDisposable, IAsyncDisposable
{
	private bool _isInternalConnectionProvider;
	private bool _disposed;

#if TRACK_OBJECTS
	public Guid IdInboxQueryUnitOfWork { get; }
#endif

	public IEFConnectionProvider ConnectionProvider { get; }
	Legion.Database.IConnectionProvider Legion.Model.Repositories.IQueryUnitOfWork.ConnectionProvider => ConnectionProvider;
	System.IServiceProvider Legion.Model.Repositories.IQueryUnitOfWork.ServiceProvider => ConnectionProvider.ServiceProvider;
	
	public InboxQueryUnitOfWork(IEFConnectionProvider connectionProvider)
	{
#if TRACK_OBJECTS
		IdInboxQueryUnitOfWork = Legion.GlobalContext.Instance.NewGuid();
		Trackers.ObjectLifetimeTracker.Track(this, IdInboxQueryUnitOfWork.ToString());
#endif

		Throw.IfArgumentNull(connectionProvider);

		ConnectionProvider = connectionProvider;
		_isInternalConnectionProvider = false; //disposed by caller
	}

	public InboxQueryUnitOfWork(Legion.EntityFrameworkCore.Model.Repositories.IDbUnitOfWork dbUnitOfWork)
	{
#if TRACK_OBJECTS
		IdInboxQueryUnitOfWork = Legion.GlobalContext.Instance.NewGuid();
		Trackers.ObjectLifetimeTracker.Track(this, IdInboxQueryUnitOfWork.ToString());
#endif

		Throw.IfArgumentNull(dbUnitOfWork);

		ConnectionProvider = dbUnitOfWork.ConnectionProvider;
		_isInternalConnectionProvider = false; //disposed by dbUnitOfWork
	}

	public InboxQueryUnitOfWork(Legion.EntityFrameworkCore.Model.Repositories.IDbQueryUnitOfWork dbQueryUnitOfWork)
	{
#if TRACK_OBJECTS
		IdInboxQueryUnitOfWork = Legion.GlobalContext.Instance.NewGuid();
		Trackers.ObjectLifetimeTracker.Track(this, IdInboxQueryUnitOfWork.ToString());
#endif

		Throw.IfArgumentNull(dbQueryUnitOfWork);

		ConnectionProvider = dbQueryUnitOfWork.ConnectionProvider;
		_isInternalConnectionProvider = false; //disposed by dbQueryUnitOfWork
	}

	public InboxQueryUnitOfWork(
		IServiceProvider serviceProvider,
		string connectionStirng,
		System.Data.IsolationLevel? isolationLevel,
		bool? allowLocking,
		bool createAuditEntryStore)
	{
#if TRACK_OBJECTS
		IdInboxQueryUnitOfWork = Legion.GlobalContext.Instance.NewGuid();
		Trackers.ObjectLifetimeTracker.Track(this, IdInboxQueryUnitOfWork.ToString());
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

	public InboxQueryUnitOfWork(
		IServiceProvider serviceProvider,
		string connectionStirng,
		bool? allowLocking,
		bool createAuditEntryStore)
	{
#if TRACK_OBJECTS
		IdInboxQueryUnitOfWork = Legion.GlobalContext.Instance.NewGuid();
		Trackers.ObjectLifetimeTracker.Track(this, IdInboxQueryUnitOfWork.ToString());
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

	protected Legion.ADF.Messaging.Inbox.PostgreSQL.IInboxQueryDbContext GetContext(IScopeContext scopeContext)
		=> ConnectionProvider.GetOrCreateDbContext<Legion.ADF.Messaging.Inbox.PostgreSQL.IInboxQueryDbContext>(scopeContext);

	private Legion.ADF.Messaging.Inbox.Model.Repositories.IVwBlockedInboxMessageTypeRepository? vwBlockedInboxMessageType;
	public Legion.ADF.Messaging.Inbox.Model.Repositories.IVwBlockedInboxMessageTypeRepository VwBlockedInboxMessageTypeRepository
		=> vwBlockedInboxMessageType ??= new Legion.ADF.Messaging.Inbox.PostgreSQL.Model.Repositories.VwBlockedInboxMessageTypeRepository(ConnectionProvider);


	private Legion.ADF.Messaging.Inbox.Model.Repositories.IVwInboxMessageRepository? vwInboxMessage;
	public Legion.ADF.Messaging.Inbox.Model.Repositories.IVwInboxMessageRepository VwInboxMessageRepository
		=> vwInboxMessage ??= new Legion.ADF.Messaging.Inbox.PostgreSQL.Model.Repositories.VwInboxMessageRepository(ConnectionProvider);


	private Legion.ADF.Messaging.Inbox.Model.Repositories.IVwInboxMessageArchiveRepository? vwInboxMessageArchive;
	public Legion.ADF.Messaging.Inbox.Model.Repositories.IVwInboxMessageArchiveRepository VwInboxMessageArchiveRepository
		=> vwInboxMessageArchive ??= new Legion.ADF.Messaging.Inbox.PostgreSQL.Model.Repositories.VwInboxMessageArchiveRepository(ConnectionProvider);


	private Legion.ADF.Messaging.Inbox.Model.Repositories.IVwInboxMessageContentRepository? vwInboxMessageContent;
	public Legion.ADF.Messaging.Inbox.Model.Repositories.IVwInboxMessageContentRepository VwInboxMessageContentRepository
		=> vwInboxMessageContent ??= new Legion.ADF.Messaging.Inbox.PostgreSQL.Model.Repositories.VwInboxMessageContentRepository(ConnectionProvider);


	private Legion.ADF.Messaging.Inbox.Model.Repositories.IVwInboxMessageProcessingLogRepository? vwInboxMessageProcessingLog;
	public Legion.ADF.Messaging.Inbox.Model.Repositories.IVwInboxMessageProcessingLogRepository VwInboxMessageProcessingLogRepository
		=> vwInboxMessageProcessingLog ??= new Legion.ADF.Messaging.Inbox.PostgreSQL.Model.Repositories.VwInboxMessageProcessingLogRepository(ConnectionProvider);


	private Legion.ADF.Messaging.Inbox.Model.Repositories.IVwInboxQueueRepository? vwInboxQueue;
	public Legion.ADF.Messaging.Inbox.Model.Repositories.IVwInboxQueueRepository VwInboxQueueRepository
		=> vwInboxQueue ??= new Legion.ADF.Messaging.Inbox.PostgreSQL.Model.Repositories.VwInboxQueueRepository(ConnectionProvider);


	private Legion.ADF.Messaging.Inbox.Model.Repositories.IVwInboxQueueMessagesRepository? vwInboxQueueMessages;
	public Legion.ADF.Messaging.Inbox.Model.Repositories.IVwInboxQueueMessagesRepository VwInboxQueueMessagesRepository
		=> vwInboxQueueMessages ??= new Legion.ADF.Messaging.Inbox.PostgreSQL.Model.Repositories.VwInboxQueueMessagesRepository(ConnectionProvider);

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
		Trackers.ObjectLifetimeTracker.SetDisposed(this, IdInboxQueryUnitOfWork.ToString());
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
		Trackers.ObjectLifetimeTracker.SetDisposed(this, IdInboxQueryUnitOfWork.ToString());
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
