using Legion;
using Legion.EntityFrameworkCore;
using Legion.Model.Audit;
using Legion.EntityFrameworkCore.Queries;
using Legion.Exceptions;
using Legion.Transactions;
using Microsoft.Extensions.DependencyInjection;

namespace Legion.ADF.Messaging.MessageBox.PostgreSQL;

internal partial class MessageBoxQueryUnitOfWork : Legion.ADF.Messaging.MessageBox.IMessageBoxQueryUnitOfWork, Legion.EntityFrameworkCore.Model.Repositories.IDbQueryUnitOfWork, Legion.Model.Repositories.IQueryUnitOfWork, IDisposable, IAsyncDisposable
{
	private bool _isInternalConnectionProvider;
	private bool _disposed;

#if TRACK_OBJECTS
	public Guid IdMessageBoxQueryUnitOfWork { get; }
#endif

	public IEFConnectionProvider ConnectionProvider { get; }
	Legion.Database.IConnectionProvider Legion.Model.Repositories.IQueryUnitOfWork.ConnectionProvider => ConnectionProvider;
	System.IServiceProvider Legion.Model.Repositories.IQueryUnitOfWork.ServiceProvider => ConnectionProvider.ServiceProvider;
	
	public MessageBoxQueryUnitOfWork(IEFConnectionProvider connectionProvider)
	{
#if TRACK_OBJECTS
		IdMessageBoxQueryUnitOfWork = Guid.NewGuid();
		Trackers.ObjectLifetimeTracker.Track(this, IdMessageBoxQueryUnitOfWork.ToString());
#endif

		Throw.IfArgumentNull(connectionProvider);

		ConnectionProvider = connectionProvider;
		_isInternalConnectionProvider = false; //disposed by caller
	}

	public MessageBoxQueryUnitOfWork(Legion.EntityFrameworkCore.Model.Repositories.IDbUnitOfWork dbUnitOfWork)
	{
#if TRACK_OBJECTS
		IdMessageBoxQueryUnitOfWork = Guid.NewGuid();
		Trackers.ObjectLifetimeTracker.Track(this, IdMessageBoxQueryUnitOfWork.ToString());
#endif

		Throw.IfArgumentNull(dbUnitOfWork);

		ConnectionProvider = dbUnitOfWork.ConnectionProvider;
		_isInternalConnectionProvider = false; //disposed by dbUnitOfWork
	}

	public MessageBoxQueryUnitOfWork(Legion.EntityFrameworkCore.Model.Repositories.IDbQueryUnitOfWork dbQueryUnitOfWork)
	{
#if TRACK_OBJECTS
		IdMessageBoxQueryUnitOfWork = Guid.NewGuid();
		Trackers.ObjectLifetimeTracker.Track(this, IdMessageBoxQueryUnitOfWork.ToString());
#endif

		Throw.IfArgumentNull(dbQueryUnitOfWork);

		ConnectionProvider = dbQueryUnitOfWork.ConnectionProvider;
		_isInternalConnectionProvider = false; //disposed by dbQueryUnitOfWork
	}

	public MessageBoxQueryUnitOfWork(
		IServiceProvider serviceProvider,
		string connectionStirng,
		System.Data.IsolationLevel? isolationLevel,
		bool? allowLocking,
		bool createAuditEntryStore)
	{
#if TRACK_OBJECTS
		IdMessageBoxQueryUnitOfWork = Guid.NewGuid();
		Trackers.ObjectLifetimeTracker.Track(this, IdMessageBoxQueryUnitOfWork.ToString());
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

	public MessageBoxQueryUnitOfWork(
		IServiceProvider serviceProvider,
		string connectionStirng,
		bool? allowLocking,
		bool createAuditEntryStore)
	{
#if TRACK_OBJECTS
		IdMessageBoxQueryUnitOfWork = Guid.NewGuid();
		Trackers.ObjectLifetimeTracker.Track(this, IdMessageBoxQueryUnitOfWork.ToString());
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

	protected Legion.ADF.Messaging.MessageBox.PostgreSQL.IMessageBoxQueryDbContext GetContext(IScopeContext scopeContext)
		=> ConnectionProvider.GetOrCreateDbContext<Legion.ADF.Messaging.MessageBox.PostgreSQL.IMessageBoxQueryDbContext>(scopeContext);

	private Legion.ADF.Messaging.MessageBox.Model.Repositories.IVwBlockedMessageTypeRepository? vwBlockedMessageType;
	public Legion.ADF.Messaging.MessageBox.Model.Repositories.IVwBlockedMessageTypeRepository VwBlockedMessageTypeRepository
		=> vwBlockedMessageType ??= new Legion.ADF.Messaging.MessageBox.PostgreSQL.Model.Repositories.VwBlockedMessageTypeRepository(ConnectionProvider);


	private Legion.ADF.Messaging.MessageBox.Model.Repositories.IVwMessageRepository? vwMessage;
	public Legion.ADF.Messaging.MessageBox.Model.Repositories.IVwMessageRepository VwMessageRepository
		=> vwMessage ??= new Legion.ADF.Messaging.MessageBox.PostgreSQL.Model.Repositories.VwMessageRepository(ConnectionProvider);


	private Legion.ADF.Messaging.MessageBox.Model.Repositories.IVwMessageArchiveRepository? vwMessageArchive;
	public Legion.ADF.Messaging.MessageBox.Model.Repositories.IVwMessageArchiveRepository VwMessageArchiveRepository
		=> vwMessageArchive ??= new Legion.ADF.Messaging.MessageBox.PostgreSQL.Model.Repositories.VwMessageArchiveRepository(ConnectionProvider);


	private Legion.ADF.Messaging.MessageBox.Model.Repositories.IVwMessageContentRepository? vwMessageContent;
	public Legion.ADF.Messaging.MessageBox.Model.Repositories.IVwMessageContentRepository VwMessageContentRepository
		=> vwMessageContent ??= new Legion.ADF.Messaging.MessageBox.PostgreSQL.Model.Repositories.VwMessageContentRepository(ConnectionProvider);


	private Legion.ADF.Messaging.MessageBox.Model.Repositories.IVwMessageProcessingLogRepository? vwMessageProcessingLog;
	public Legion.ADF.Messaging.MessageBox.Model.Repositories.IVwMessageProcessingLogRepository VwMessageProcessingLogRepository
		=> vwMessageProcessingLog ??= new Legion.ADF.Messaging.MessageBox.PostgreSQL.Model.Repositories.VwMessageProcessingLogRepository(ConnectionProvider);


	private Legion.ADF.Messaging.MessageBox.Model.Repositories.IVwQueueRepository? vwQueue;
	public Legion.ADF.Messaging.MessageBox.Model.Repositories.IVwQueueRepository VwQueueRepository
		=> vwQueue ??= new Legion.ADF.Messaging.MessageBox.PostgreSQL.Model.Repositories.VwQueueRepository(ConnectionProvider);


	private Legion.ADF.Messaging.MessageBox.Model.Repositories.IVwQueuedMessageRepository? vwQueuedMessage;
	public Legion.ADF.Messaging.MessageBox.Model.Repositories.IVwQueuedMessageRepository VwQueuedMessageRepository
		=> vwQueuedMessage ??= new Legion.ADF.Messaging.MessageBox.PostgreSQL.Model.Repositories.VwQueuedMessageRepository(ConnectionProvider);


	private Legion.ADF.Messaging.MessageBox.Model.Repositories.IVwQueueMessagesRepository? vwQueueMessages;
	public Legion.ADF.Messaging.MessageBox.Model.Repositories.IVwQueueMessagesRepository VwQueueMessagesRepository
		=> vwQueueMessages ??= new Legion.ADF.Messaging.MessageBox.PostgreSQL.Model.Repositories.VwQueueMessagesRepository(ConnectionProvider);


	private Legion.ADF.Messaging.MessageBox.Model.Repositories.IVwSubscribedMessageRepository? vwSubscribedMessage;
	public Legion.ADF.Messaging.MessageBox.Model.Repositories.IVwSubscribedMessageRepository VwSubscribedMessageRepository
		=> vwSubscribedMessage ??= new Legion.ADF.Messaging.MessageBox.PostgreSQL.Model.Repositories.VwSubscribedMessageRepository(ConnectionProvider);


	private Legion.ADF.Messaging.MessageBox.Model.Repositories.IVwTopicRepository? vwTopic;
	public Legion.ADF.Messaging.MessageBox.Model.Repositories.IVwTopicRepository VwTopicRepository
		=> vwTopic ??= new Legion.ADF.Messaging.MessageBox.PostgreSQL.Model.Repositories.VwTopicRepository(ConnectionProvider);


	private Legion.ADF.Messaging.MessageBox.Model.Repositories.IVwTopicSubscriptionRepository? vwTopicSubscription;
	public Legion.ADF.Messaging.MessageBox.Model.Repositories.IVwTopicSubscriptionRepository VwTopicSubscriptionRepository
		=> vwTopicSubscription ??= new Legion.ADF.Messaging.MessageBox.PostgreSQL.Model.Repositories.VwTopicSubscriptionRepository(ConnectionProvider);


	private Legion.ADF.Messaging.MessageBox.Model.Repositories.IVwTopicSubscriptionMessagesRepository? vwTopicSubscriptionMessages;
	public Legion.ADF.Messaging.MessageBox.Model.Repositories.IVwTopicSubscriptionMessagesRepository VwTopicSubscriptionMessagesRepository
		=> vwTopicSubscriptionMessages ??= new Legion.ADF.Messaging.MessageBox.PostgreSQL.Model.Repositories.VwTopicSubscriptionMessagesRepository(ConnectionProvider);

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
		Trackers.ObjectLifetimeTracker.SetDisposed(this, IdMessageBoxQueryUnitOfWork.ToString());
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
		Trackers.ObjectLifetimeTracker.SetDisposed(this, IdMessageBoxQueryUnitOfWork.ToString());
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
