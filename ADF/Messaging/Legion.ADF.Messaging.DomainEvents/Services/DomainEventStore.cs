using Legion.ADF.Messaging.Settings;
using Legion.Database;
using Legion.Model.Messaging;
using Legion.Transactions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Legion.ADF.Messaging.DomainEvents.Services;

public partial class DomainEventStore : IDomainEventStore, IDisposable, IAsyncDisposable
{
	protected readonly IServiceProvider _serviceProvider;
	protected readonly IConnectionProviderFactory? _connectionProviderFactory;
	protected readonly MessagingDomainEventsStoreOptions _options;
	protected readonly ILogger _logger;

	private readonly Lazy<IMessagingAccessControlManager?> _accessControlManager;

	private bool _disposed;

	protected IDomainEventsUnitOfWork UoW { get; private set; }
	protected IDomainEventsQueryUnitOfWork QUoW { get; private set; }
	protected IConnectionProvider? ConnectionProvider { get; private set; }
	protected bool IsInternalConnectionProvider { get; private set; }
	public bool AutoSaveChanges { get; set; } = true;
	public IMessagingAccessControlManager? AccessControlManager => _accessControlManager.Value;

	public DomainEventStore(
		IServiceProvider serviceProvider,
		IConnectionProviderFactory connectionProviderFactory,
		IOptions<MessagingDomainEventsStoreOptions> options,
		ILogger<DomainEventStore> logger)
	{
		Throw.IfArgumentNull(serviceProvider);
		Throw.IfArgumentNull(connectionProviderFactory);
		Throw.IfArgumentNull(options);
		Throw.IfArgumentNull(logger);

		_serviceProvider = serviceProvider;
		_connectionProviderFactory = connectionProviderFactory;
		_options = options.Value;
		_logger = logger;

		var scopeContext = ScopeContext.Create("Legion.ADF.Messaging.DomainEvents");

		CreateUnitOfWork(scopeContext);

		_accessControlManager = new(() => UoW!.ServiceProvider.GetService<IMessagingAccessControlManager>());
	}

	protected void CreateUnitOfWork(IScopeContext scopeContext)
	{
		IsInternalConnectionProvider = true;
		ConnectionProvider = _connectionProviderFactory!.CreateWithNewTransactionByStoreId<ConnectionStringProvider>(
			_serviceProvider!,
			_options.MessagingDomainEventsStoreId,
			transactionIsolationLevel: null,
			false,
			createAuditEntryStore: false);

		var domainEventsUowResult = ConnectionProvider.UnitOfWorkProvider.Create<IDomainEventsUnitOfWork>(scopeContext);

		if (domainEventsUowResult.HasError)
			domainEventsUowResult.ThrowIfErrorOrNullData(scopeContext, Legion.ADF.Messaging.Exceptions.Internal.ErrorCodes.DomainEventsUnitOfWorkException.InvalidUoW, true);

		UoW = domainEventsUowResult.Data!;

		var queryDomainEventsUowResult = ConnectionProvider.UnitOfWorkProvider.CreateQuery<IDomainEventsQueryUnitOfWork>(scopeContext);

		if (queryDomainEventsUowResult.HasError)
			queryDomainEventsUowResult.ThrowIfErrorOrNullData(scopeContext, Legion.ADF.Messaging.Exceptions.Internal.ErrorCodes.DomainEventsUnitOfWorkException.InvalidUoW, true);

		QUoW = queryDomainEventsUowResult.Data!;
	}

	public DomainEventStore(
		IConnectionProvider connectionProvider,
		IOptions<MessagingDomainEventsStoreOptions> options,
		ILogger<DomainEventStore> logger)
	{
		Throw.IfArgumentNull(connectionProvider);
		Throw.IfArgumentNull(options);
		Throw.IfArgumentNull(logger);

		_options = options.Value;
		_logger = logger;

		var scopeContext = ScopeContext.Create("Legion.ADF.Messaging.DomainEvents");

		ConnectionProvider = connectionProvider;
		_serviceProvider = ConnectionProvider.ServiceProvider;

		IsInternalConnectionProvider = false;
		var domainEventsUowResult = ConnectionProvider.UnitOfWorkProvider.Create<IDomainEventsUnitOfWork>(scopeContext);

		if (domainEventsUowResult.HasError)
			domainEventsUowResult.ThrowIfErrorOrNullData(scopeContext, Legion.ADF.Messaging.Exceptions.Internal.ErrorCodes.DomainEventsUnitOfWorkException.InvalidUoW, true);

		UoW = domainEventsUowResult.Data!;

		_accessControlManager = new(() => UoW.ServiceProvider.GetService<IMessagingAccessControlManager>());

		var queryDomainEventsUowResult = ConnectionProvider.UnitOfWorkProvider.CreateQuery<IDomainEventsQueryUnitOfWork>(scopeContext);

		if (queryDomainEventsUowResult.HasError)
			queryDomainEventsUowResult.ThrowIfErrorOrNullData(scopeContext, Legion.ADF.Messaging.Exceptions.Internal.ErrorCodes.DomainEventsUnitOfWorkException.InvalidUoW, true);

		QUoW = queryDomainEventsUowResult.Data!;
	}

	public async Task<IResult> SaveAsync(IScopeContext scopeContext, CancellationToken cancellationToken = default)
		=> await SaveInternalAsync(scopeContext, true, cancellationToken);

	public IResult Save(IScopeContext scopeContext)
		=> SaveInternal(scopeContext, true);

	protected async Task<IResult<int>> SaveInternalAsync(IScopeContext scopeContext, bool force, CancellationToken cancellationToken = default)
	{
		var result = new ResultBuilder<int>();

		if (force || AutoSaveChanges)
		{
			var saveResult = await UoW.SaveAsync(scopeContext, cancellationToken);
			if (result.MergeAllWithDataHasError(saveResult))
				return result.Build();

			if (IsInternalConnectionProvider)
			{
				var commitResult = await ConnectionProvider!.TransactionsController!.CommitAllAsync(
					scopeContext,
					TransactionsControllerStatus.NotIdle,
					cancellationToken: default);

				await ConnectionProvider!.DisposeAsync();

				if (result.MergeHasError(commitResult))
					return result.Build();

				CreateUnitOfWork(scopeContext);
				//ConnectionProvider.ReCreateTransaction(scopeContext);
			}
		}

		return result.Build();
	}

	protected IResult<int> SaveInternal(IScopeContext scopeContext, bool force)
	{
		var result = new ResultBuilder<int>();

		if (force || AutoSaveChanges)
		{
			var saveResult = UoW.Save(scopeContext);
			if (result.MergeAllWithDataHasError(saveResult))
				return result.Build();

			if (IsInternalConnectionProvider)
			{
				var commitResult = ConnectionProvider!.TransactionsController!.CommitAll(
					scopeContext,
					TransactionsControllerStatus.NotIdle);

				ConnectionProvider!.Dispose();

				if (result.MergeHasError(commitResult))
					return result.Build();

				CreateUnitOfWork(scopeContext);
				//ConnectionProvider.ReCreateTransaction(scopeContext);
			}
		}

		return result.Build();
	}

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
		if (UoW != null || QUoW != null)
		{
			if (IsInternalConnectionProvider)
			{
				var scopeContext = ScopeContext.Create($"{this.GetType().FullName} {nameof(DisposeAsyncCoreAsync)}");

				var commitResult = await ConnectionProvider!.TransactionsController!.CommitAllAsync(
					scopeContext,
					TransactionsControllerStatus.None,
					cancellationToken: default);

				await ConnectionProvider!.DisposeAsync();
			}
		}

		if (UoW != null)
			await UoW.DisposeAsync();

		if (QUoW != null)
			await QUoW.DisposeAsync();
	}

	private void Dispose(bool disposing)
	{
		if (_disposed)
			return;

		_disposed = true;

		if (disposing)
		{
			if (UoW != null || QUoW != null)
			{
				if (IsInternalConnectionProvider)
				{
					var scopeContext = ScopeContext.Create($"{this.GetType().FullName} {nameof(Dispose)}");

					var commitResult = ConnectionProvider!.TransactionsController!.CommitAll(
						scopeContext,
						TransactionsControllerStatus.None);

					ConnectionProvider!.Dispose();
				}
			}

			UoW?.Dispose();
			QUoW?.Dispose();
		}
	}

	public void Dispose()
	{
		Dispose(true);
		GC.SuppressFinalize(this);
	}
}
