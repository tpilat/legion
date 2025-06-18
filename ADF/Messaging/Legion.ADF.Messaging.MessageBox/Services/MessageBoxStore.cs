using Legion.ADF.Messaging.Settings;
using Legion.Database;
using Legion.Transactions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Legion.ADF.Messaging.MessageBox.Services;

public partial class MessageBoxStore : IMessageBoxStore, IDisposable, IAsyncDisposable
{
	protected readonly IServiceProvider _serviceProvider;
	protected readonly IConnectionProviderFactory? _connectionProviderFactory;
	protected readonly MessagingMessageBoxStoreOptions _options;
	protected readonly ILogger _logger;

	private readonly Lazy<IMessagingAccessControlManager?> _accessControlManager;

	private bool _disposed;

	protected IMessageBoxUnitOfWork UoW { get; private set; }
	protected IMessageBoxQueryUnitOfWork QUoW { get; private set; }
	protected IConnectionProvider? ConnectionProvider { get; private set; }
	protected bool IsInternalConnectionProvider { get; private set; }
	public bool AutoSaveChanges { get; set; } = true;
	public IMessagingAccessControlManager? AccessControlManager => _accessControlManager.Value;

	//public bool Initialized { get; private set; }

	public MessageBoxStore(
		IServiceProvider serviceProvider,
		IConnectionProviderFactory connectionProviderFactory,
		IOptions<MessagingMessageBoxStoreOptions> options,
		ILogger<MessageBoxStore> logger)
	{
		Throw.IfArgumentNull(serviceProvider);
		Throw.IfArgumentNull(connectionProviderFactory);
		Throw.IfArgumentNull(options);
		Throw.IfArgumentNull(logger);

		_serviceProvider = serviceProvider;
		_connectionProviderFactory = connectionProviderFactory;
		_options = options.Value;
		_logger = logger;

		var scopeContext = ScopeContext.Create("Legion.ADF.Messaging.MessageBox");

		CreateUnitOfWork(scopeContext);

		_accessControlManager = new(() => UoW!.ServiceProvider.GetService<IMessagingAccessControlManager>());
	}

	protected void CreateUnitOfWork(IScopeContext scopeContext)
	{
		IsInternalConnectionProvider = true;
		ConnectionProvider = _connectionProviderFactory!.CreateWithNewTransactionByStoreId<ConnectionStringProvider>(
			_serviceProvider!,
			_options.MessagingMessageBoxStoreId,
			transactionIsolationLevel: null,
			false,
			false);

		var messageBoxUowResult = ConnectionProvider.UnitOfWorkProvider.Create<IMessageBoxUnitOfWork>(scopeContext);

		if (messageBoxUowResult.HasError)
			messageBoxUowResult.ThrowIfErrorOrNullData(scopeContext, Legion.ADF.Messaging.Exceptions.Internal.ErrorCodes.MessageBoxUnitOfWorkException.InvalidUoW, true);

		UoW = messageBoxUowResult.Data!;

		var queryMessageBoxUowResult = ConnectionProvider.UnitOfWorkProvider.CreateQuery<IMessageBoxQueryUnitOfWork>(scopeContext);

		if (queryMessageBoxUowResult.HasError)
			queryMessageBoxUowResult.ThrowIfErrorOrNullData(scopeContext, Legion.ADF.Messaging.Exceptions.Internal.ErrorCodes.MessageBoxUnitOfWorkException.InvalidUoW, true);

		QUoW = queryMessageBoxUowResult.Data!;
	}

	public MessageBoxStore(
		IConnectionProvider connectionProvider,
		IOptions<MessagingMessageBoxStoreOptions> options,
		ILogger<MessageBoxStore> logger)
	{
		Throw.IfArgumentNull(connectionProvider);
		Throw.IfArgumentNull(options);
		Throw.IfArgumentNull(logger);

		_options = options.Value;
		_logger = logger;

		var scopeContext = ScopeContext.Create("Legion.ADF.Messaging.MessageBox");

		ConnectionProvider = connectionProvider;
		_serviceProvider = ConnectionProvider.ServiceProvider;

		IsInternalConnectionProvider = false;
		var messageBoxUowResult = ConnectionProvider.UnitOfWorkProvider.Create<IMessageBoxUnitOfWork>(scopeContext);

		if (messageBoxUowResult.HasError)
			messageBoxUowResult.ThrowIfErrorOrNullData(scopeContext, Legion.ADF.Messaging.Exceptions.Internal.ErrorCodes.MessageBoxUnitOfWorkException.InvalidUoW, true);

		UoW = messageBoxUowResult.Data!;

		_accessControlManager = new(() => UoW.ServiceProvider.GetService<IMessagingAccessControlManager>());

		var queryMessageBoxUowResult = ConnectionProvider.UnitOfWorkProvider.CreateQuery<IMessageBoxQueryUnitOfWork>(scopeContext);

		if (queryMessageBoxUowResult.HasError)
			queryMessageBoxUowResult.ThrowIfErrorOrNullData(scopeContext, Legion.ADF.Messaging.Exceptions.Internal.ErrorCodes.MessageBoxUnitOfWorkException.InvalidUoW, true);

		QUoW = queryMessageBoxUowResult.Data!;
	}

	//public async Task<IResult> InitializeAsync(
	//	IScopeContext scopeContext,
	//	string name,
	//	string version,
	//	int? maxDegreeOfQueueParallelism,
	//	LogLevel logLevel,
	//	CancellationToken cancellationToken = default)
	//{
	//	scopeContext = scopeContext.CreateNew()
	//		.AddContextProperty(nameof(name), name)
	//		.AddContextProperty(nameof(version), version);

	//	var result = new ResultBuilder<Guid>();

	//	if (Initialized)
	//		return result.Build();

	//	Initialized = true;

	//	if (result.IsCancellationRequested(cancellationToken, scopeContext))
	//		return result.Build();

	//	if (result.IsDisposed(_disposed, scopeContext))
	//		return result.Build();

	//	if (result.IsArgumentNullOrWhiteSpace(scopeContext, name))
	//		return result.Build();

	//	if (result.IsArgumentNullOrWhiteSpace(scopeContext, version))
	//		return result.Build();

	//	var exists = await UoW.MessageBoxInstanceRepository
	//		.ExistsMessageBoxInstanceById(new Queries.MessageBoxInstance.ExistsMessageBoxInstanceByIdQuery(EnvironmentInfo.RUNTIME_UNIQUE_KEY, false, AsNoTracking: false))
	//		.ToResultAsync(scopeContext, cancellationToken);

	//	if (exists)
	//		return result.Build();

	//	var createResult = Model.MessageBoxInstance.Create(
	//		scopeContext,
	//		name,
	//		version,
	//		maxDegreeOfQueueParallelism,
	//		logLevel);

	//	if (result.MergeHasError(createResult))
	//		return result.Build();

	//	var dbMessageBoxInstance = createResult.Data!;

	//	UoW.MessageBoxInstanceRepository.Add(scopeContext, dbMessageBoxInstance);

	//	var saveResult = await SaveInternalAsync(scopeContext, force: false, cancellationToken);

	//	return result.WithData(dbMessageBoxInstance.IdMessageBoxInstance).Build();
	//}

	//private async Task<IResult> InitializeInternalAsync(
	//	IScopeContext scopeContext,
	//	LogLevel logLevel,
	//	CancellationToken cancellationToken = default)
	//	=> await InitializeAsync(
	//		scopeContext,
	//		EnvironmentInfoProviderCache.Instance.EntryAssemblyName ?? nameof(MessageBoxStore),
	//		EnvironmentInfoProviderCache.Instance.CLRVersion ?? "0.0.0.0",
	//		null,
	//		logLevel,
	//		cancellationToken);

	//public IResult Initialize(
	//	IScopeContext scopeContext,
	//	string name,
	//	string version,
	//	int? maxDegreeOfQueueParallelism,
	//	LogLevel logLevel)
	//{
	//	scopeContext = scopeContext.CreateNew()
	//		.AddContextProperty(nameof(name), name)
	//		.AddContextProperty(nameof(version), version);

	//	var result = new ResultBuilder<Guid>();

	//	if (Initialized)
	//		return result.Build();

	//	Initialized = true;

	//	if (result.IsDisposed(_disposed, scopeContext))
	//		return result.Build();

	//	if (result.IsArgumentNullOrWhiteSpace(scopeContext, name))
	//		return result.Build();

	//	if (result.IsArgumentNullOrWhiteSpace(scopeContext, version))
	//		return result.Build();

	//	var exists = UoW.MessageBoxInstanceRepository
	//		.ExistsMessageBoxInstanceById(new Queries.MessageBoxInstance.ExistsMessageBoxInstanceByIdQuery(EnvironmentInfo.RUNTIME_UNIQUE_KEY, false, AsNoTracking: false))
	//		.ToResult(scopeContext);

	//	if (exists)
	//		return result.Build();

	//	var createResult = Model.MessageBoxInstance.Create(
	//		scopeContext,
	//		name,
	//		version,
	//		maxDegreeOfQueueParallelism,
	//		logLevel);

	//	if (result.MergeHasError(createResult))
	//		return result.Build();

	//	var dbMessageBoxInstance = createResult.Data!;

	//	UoW.MessageBoxInstanceRepository.Add(scopeContext, dbMessageBoxInstance);

	//	var saveResult = SaveInternal(scopeContext, force: false);

	//	return result.WithData(dbMessageBoxInstance.IdMessageBoxInstance).Build();
	//}

	//private IResult InitializeInternal(
	//	IScopeContext scopeContext,
	//	LogLevel logLevel)
	//	=> Initialize(
	//		scopeContext,
	//		EnvironmentInfoProviderCache.Instance.EntryAssemblyName ?? nameof(MessageBoxStore),
	//		EnvironmentInfoProviderCache.Instance.CLRVersion ?? "0.0.0.0",
	//		null,
	//		logLevel);

	public async Task<IResult> SaveAsync(IScopeContext scopeContext, CancellationToken cancellationToken = default)
		=> await SaveInternalAsync(scopeContext, true, cancellationToken);

	public IResult Save(IScopeContext scopeContext)
		=> SaveInternal(scopeContext, true);

	protected async Task<IResult> SaveInternalAsync(IScopeContext scopeContext, bool force, CancellationToken cancellationToken = default)
	{
		var result = new ResultBuilder();

		if (force || AutoSaveChanges)
		{
			//await InitializeInternalAsync(scopeContext, _options.LogLevel, cancellationToken);

			var saveResult = await UoW.SaveAsync(scopeContext, cancellationToken);
			if (result.MergeHasError(saveResult))
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

	protected IResult SaveInternal(IScopeContext scopeContext, bool force)
	{
		var result = new ResultBuilder();

		if (force || AutoSaveChanges)
		{
			//InitializeInternal(scopeContext, _options.LogLevel);

			var saveResult = UoW.Save(scopeContext);
			if (result.MergeHasError(saveResult))
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
