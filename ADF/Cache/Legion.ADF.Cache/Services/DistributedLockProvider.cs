using Legion.ADF.Cache.Settings;
using Legion.Database;
using Legion.Locks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Legion.ADF.Cache.Services;

internal class DistributedLockProvider : IDistributedLockProvider, IDisposable
{
	private readonly DistributedLockOptions _options;
	protected readonly IServiceProvider _serviceProvider;
	protected readonly IConnectionProviderFactory _connectionProviderFactory;

	private bool _disposed;

	public DistributedLockProvider(
		IOptions<DistributedLockOptions> options,
		IServiceProvider serviceProvider,
		IConnectionProviderFactory connectionProviderFactory)
	{
		Throw.IfArgumentNull(options);
		Throw.IfArgumentNull(serviceProvider);
		Throw.IfArgumentNull(connectionProviderFactory);

		_options = options.Value;
		_serviceProvider = serviceProvider;
		_connectionProviderFactory = connectionProviderFactory;
	}

	private ICacheUnitOfWork CreateUnitOfWork(IScopeContext scopeContext, IServiceProvider serviceProvider)
	{
		var connectionProvider = _connectionProviderFactory.CreateWithoutTransactionByStoreId<ConnectionStringProvider>(
			serviceProvider,
			_options.CacheStoreId,
			false,
			false);

		var cacheUowResult = connectionProvider.UnitOfWorkProvider.Create<ICacheUnitOfWork>(scopeContext);

		if (cacheUowResult.HasError)
			cacheUowResult.ThrowIfErrorOrNullData(scopeContext, Legion.ADF.Cache.Exceptions.Internal.ErrorCodes.CacheUnitOfWorkException.InvalidUoW, true);

		return cacheUowResult.Data!;
	}

	//private async Task<Model.DistributedLock?> TryAcquireDistributedLockObjectAsync(
	//	string key,
	//	TimeSpan timeout,
	//	string? metadata,
	//	TimeSpan? retryDelay = null,
	//	int? maxRetries = null,
	//	CancellationToken cancellationToken = default)
	//{
	//	Throw.IfArgumentNullOrWhiteSpace(key);

	//	var scopeContext = ScopeContext.Create("Legion.ADF.Cache")
	//		.AddContextProperty(nameof(key), key);

	//	await using var asyncServiceScope = _serviceProvider.CreateAsyncScope();
	//	var scopedServiceProvider = asyncServiceScope.ServiceProvider;
	//	var uow = CreateUnitOfWork(scopeContext, scopedServiceProvider);
	//	await using var connectionProvider = uow.ConnectionProvider;

	//	var distributedLock = await uow.DistributedLockRepository.TryAcquireDistributedLockAsync(
	//		scopeContext,
	//		key,
	//		timeout,
	//		metadata,
	//		retryDelay,
	//		maxRetries,
	//		cancellationToken);

	//	return distributedLock;
	//}

	public async Task<bool> ExistsAsync(
		string key,
		CancellationToken cancellationToken = default)
	{
		var scopeContext = ScopeContext.Create("Legion.ADF.Cache")
			.AddContextProperty(nameof(key), key);

		Throw.IfArgumentNullOrWhiteSpace(key);

		await using var asyncServiceScope = _serviceProvider.CreateAsyncScope();
		var scopedServiceProvider = asyncServiceScope.ServiceProvider;
		var uow = CreateUnitOfWork(scopeContext, scopedServiceProvider);
		await using var connectionProvider = uow.ConnectionProvider;

		var distributedLock = await uow.DistributedLockRepository.TryGetDistributedLockAsync(scopeContext, key);

		return distributedLock != null;
	}

	public async Task<string?> GetMetadataAsync(
		string key,
		CancellationToken cancellationToken = default)
	{
		var scopeContext = ScopeContext.Create("Legion.ADF.Cache")
			.AddContextProperty(nameof(key), key);

		Throw.IfArgumentNullOrWhiteSpace(key);

		await using var asyncServiceScope = _serviceProvider.CreateAsyncScope();
		var scopedServiceProvider = asyncServiceScope.ServiceProvider;
		var uow = CreateUnitOfWork(scopeContext, scopedServiceProvider);
		await using var connectionProvider = uow.ConnectionProvider;

		var distributedLock = await uow.DistributedLockRepository.TryGetDistributedLockAsync(scopeContext, key);

		return distributedLock?.Metadata;
	}

	public async Task<string?> TryAcquireLockAsync(
		string key,
		TimeSpan timeout,
		string? metadata,
		TimeSpan? retryDelay = null,
		int? maxRetries = null,
		CancellationToken cancellationToken = default)
	{
		var scopeContext = ScopeContext.Create("Legion.ADF.Cache")
			.AddContextProperty(nameof(key), key);

		Throw.IfArgumentNullOrWhiteSpace(key);

		await using var asyncServiceScope = _serviceProvider.CreateAsyncScope();
		var scopedServiceProvider = asyncServiceScope.ServiceProvider;
		var uow = CreateUnitOfWork(scopeContext, scopedServiceProvider);
		await using var connectionProvider = uow.ConnectionProvider;

		var lockId = await uow.DistributedLockRepository.TryAcquireDistributedLockIdAsync(
			scopeContext,
			key,
			timeout,
			metadata,
			retryDelay,
			maxRetries,
			cancellationToken);

		return lockId;
	}

	public async Task<bool> ReleaseLockAsync(
		string key,
		string lockId,
		CancellationToken cancellationToken = default)
	{
		var scopeContext = ScopeContext.Create("Legion.ADF.Cache")
			.AddContextProperty(nameof(key), key);

		Throw.IfArgumentNullOrWhiteSpace(key);
		Throw.IfArgumentNullOrWhiteSpace(lockId);

		await using var asyncServiceScope = _serviceProvider.CreateAsyncScope();
		var scopedServiceProvider = asyncServiceScope.ServiceProvider;
		var uow = CreateUnitOfWork(scopeContext, scopedServiceProvider);
		await using var connectionProvider = uow.ConnectionProvider;

		var released = await uow.DistributedLockRepository.ReleaseDistributedLockAsync(
			scopeContext,
			key,
			lockId,
			cancellationToken);

		return released;
	}

	public async Task<bool> RenewLockAsync(
		string key,
		string lockId,
		TimeSpan timeout,
		CancellationToken cancellationToken = default)
	{
		var scopeContext = ScopeContext.Create("Legion.ADF.Cache")
			.AddContextProperty(nameof(key), key);

		Throw.IfArgumentNullOrWhiteSpace(key);

		await using var asyncServiceScope = _serviceProvider.CreateAsyncScope();
		var scopedServiceProvider = asyncServiceScope.ServiceProvider;
		var uow = CreateUnitOfWork(scopeContext, scopedServiceProvider);
		await using var connectionProvider = uow.ConnectionProvider;

		var released = await uow.DistributedLockRepository.RenewDistributedLockAsync(
			scopeContext,
			key,
			lockId,
			timeout,
			cancellationToken);

		return released;
	}

	protected virtual void Dispose(bool disposing)
	{
		if (!_disposed)
		{
			if (disposing)
			{
				//
			}

			_disposed = true;
		}
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}
}
