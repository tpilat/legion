using Legion.ADF.Cache.Settings;
using Legion.Caching;
using Legion.Database;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Legion.ADF.Cache.Services;

internal class PersistentCache : IPersistentCache, IDisposable
{
	private readonly ADFPersistentCacheOptions _options;
	protected readonly IServiceProvider _serviceProvider;
	protected readonly IConnectionProviderFactory _connectionProviderFactory;

	private bool _disposed;

	public PersistentCache(
		IOptions<ADFPersistentCacheOptions> options,
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

	private async Task<Model.CacheData?> GetCacheDataAsync(
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

		var cacheData = await uow.CacheDataRepository.TryGetCacheDataAsync(scopeContext, key, cancellationToken);

		return cacheData;
	}

	public async Task<bool> IsAliveAsync(CancellationToken cancellationToken = default)
	{
		var scopeContext = ScopeContext.Create("Legion.ADF.Cache");

		await using var asyncServiceScope = _serviceProvider.CreateAsyncScope();
		var scopedServiceProvider = asyncServiceScope.ServiceProvider;
		var uow = CreateUnitOfWork(scopeContext, scopedServiceProvider);
		await using var connectionProvider = uow.ConnectionProvider;

		var isAlive = await uow.CacheDataRepository.IsAliveAsync(scopeContext, cancellationToken);

		return isAlive;
	}

	public async Task<(string? Value, Guid? RowVersion)> GetValueAsync(
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

		var cacheData = await uow.CacheDataRepository.TryGetCacheDataAsync(scopeContext, key, cancellationToken);

		return (cacheData?.Value, cacheData?.RowVersion);
	}

	public async Task<bool> SetValueAsync(
		string key,
		string value,
		MemoryCacheEntryOptions? options,
		CancellationToken cancellationToken = default)
	{
		var scopeContext = ScopeContext.Create("Legion.ADF.Cache")
			.AddContextProperty(nameof(key), key)
			.AddContextProperty(nameof(options.SlidingExpiration), options?.SlidingExpiration?.ToString())
			.AddContextProperty(nameof(options.AbsoluteExpiration), options?.AbsoluteExpiration?.ToString())
			.AddContextProperty(nameof(options.AbsoluteExpirationRelativeToNow), options?.AbsoluteExpirationRelativeToNow?.ToString());

		Throw.IfArgumentNullOrWhiteSpace(key);
		Throw.IfArgumentNullOrWhiteSpace(value);

		await using var asyncServiceScope = _serviceProvider.CreateAsyncScope();
		var scopedServiceProvider = asyncServiceScope.ServiceProvider;
		var uow = CreateUnitOfWork(scopeContext, scopedServiceProvider);
		await using var connectionProvider = uow.ConnectionProvider;

		var upadated = await uow.CacheDataRepository.SetAsync(scopeContext, key, value, currentRowVersion: null, options, cancellationToken);

		return upadated;
	}

	public Task<bool> SetValuePermanentlyAsync(
		string key,
		string value,
		CancellationToken cancellationToken = default)
		 => SetValueAsync(
				key,
				value,
				options: null,
				cancellationToken);

	public Task<bool> SetValueWithSlidingExpirationAsync(
		string key,
		string value,
		TimeSpan slidingTime,
		CancellationToken cancellationToken = default)
		 => SetValueAsync(
				key,
				value,
				new MemoryCacheEntryOptions
				{
					SlidingExpiration = slidingTime
				},
				cancellationToken);

	public Task<bool> SetValueWithAbsoluteExpirationAsync(
		string key,
		string value,
		DateTime keepUntil,
		CancellationToken cancellationToken = default)
		 => SetValueAsync(
				key,
				value,
				new MemoryCacheEntryOptions
				{
					AbsoluteExpiration = keepUntil
				},
				cancellationToken);

	public Task<bool> SetValueWithAbsoluteServerSideExpirationAsync(
		string key,
		string value,
		TimeSpan deltaToNowUtc,
		CancellationToken cancellationToken = default)
		 => SetValueAsync(
				key,
				value,
				new MemoryCacheEntryOptions
				{
					AbsoluteExpiration = GlobalContext.Instance.UtcNow.Add(deltaToNowUtc)
				},
				cancellationToken);

	public async Task<string> GetOrSetValueAsync(
		string key,
		Func<string> value,
		MemoryCacheEntryOptions? options,
		bool forceSet = false,
		CancellationToken cancellationToken = default)
	{
		var scopeContext = ScopeContext.Create("Legion.ADF.Cache")
			.AddContextProperty(nameof(key), key)
			.AddContextProperty(nameof(options.SlidingExpiration), options?.SlidingExpiration?.ToString())
			.AddContextProperty(nameof(options.AbsoluteExpiration), options?.AbsoluteExpiration?.ToString())
			.AddContextProperty(nameof(options.AbsoluteExpirationRelativeToNow), options?.AbsoluteExpirationRelativeToNow?.ToString());

		Throw.IfArgumentNullOrWhiteSpace(key);
		Throw.IfArgumentNull(value);

		var cacheData = await GetCacheDataAsync(key, cancellationToken);

		if (!forceSet && cacheData != null)
			return cacheData.Value;

		var result = value();

		await using var asyncServiceScope = _serviceProvider.CreateAsyncScope();
		var scopedServiceProvider = asyncServiceScope.ServiceProvider;
		var uow = CreateUnitOfWork(scopeContext, scopedServiceProvider);
		await using var connectionProvider = uow.ConnectionProvider;

		var updated = await uow.CacheDataRepository.SetAsync(scopeContext, key, result, cacheData?.RowVersion, options, cancellationToken);

		if (!updated)
			Throw.InvalidOperationException(
				Legion.ADF.Cache.Exceptions.Internal.ErrorCodes.CacheDataRepositoryException.CacheDataConcurrentUpdate(key),
				scopeContext);

		return result;
	}

	public Task<string> GetOrSetValuePermanentlyAsync(
		string key,
		Func<string> value,
		bool forceSet = false,
		CancellationToken cancellationToken = default)
		=> GetOrSetValueAsync(
			key,
			value,
			options: null,
			forceSet,
			cancellationToken);

	public Task<string> GetOrSetValueWithSlidingExpirationAsync(
		string key,
		Func<string> value,
		TimeSpan slidingTime,
		bool forceSet = false,
		CancellationToken cancellationToken = default)
		=> GetOrSetValueAsync(
			key,
			value,
			new MemoryCacheEntryOptions
			{
				SlidingExpiration = slidingTime
			},
			forceSet,
			cancellationToken);

	public Task<string> GetOrSetValueWithAbsoluteExpirationAsync(
		string key,
		Func<string> value,
		DateTime keepUntil,
		bool forceSet = false,
		CancellationToken cancellationToken = default)
		=> GetOrSetValueAsync(
			key,
			value,
			new MemoryCacheEntryOptions
			{
				AbsoluteExpiration = keepUntil
			},
			forceSet,
			cancellationToken);

	public async Task<string> GetOrSetValueAsync(
		string key,
		Func<CancellationToken, Task<string>> value,
		MemoryCacheEntryOptions? options,
		bool forceSet = false,
		CancellationToken cancellationToken = default)
	{
		var scopeContext = ScopeContext.Create("Legion.ADF.Cache")
			.AddContextProperty(nameof(key), key)
			.AddContextProperty(nameof(options.SlidingExpiration), options?.SlidingExpiration?.ToString())
			.AddContextProperty(nameof(options.AbsoluteExpiration), options?.AbsoluteExpiration?.ToString())
			.AddContextProperty(nameof(options.AbsoluteExpirationRelativeToNow), options?.AbsoluteExpirationRelativeToNow?.ToString());

		Throw.IfArgumentNullOrWhiteSpace(key);
		Throw.IfArgumentNull(value);

		var cacheData = await GetCacheDataAsync(key, cancellationToken);

		if (!forceSet && cacheData != null)
			return cacheData.Value;

		var result = await value(cancellationToken);

		await using var asyncServiceScope = _serviceProvider.CreateAsyncScope();
		var scopedServiceProvider = asyncServiceScope.ServiceProvider;
		var uow = CreateUnitOfWork(scopeContext, scopedServiceProvider);
		await using var connectionProvider = uow.ConnectionProvider;

		var updated = await uow.CacheDataRepository.SetAsync(scopeContext, key, result, cacheData?.RowVersion, options, cancellationToken);

		if (!updated)
			Throw.InvalidOperationException(
				Legion.ADF.Cache.Exceptions.Internal.ErrorCodes.CacheDataRepositoryException.CacheDataConcurrentUpdate(key),
				scopeContext);

		return result;
	}

	public Task<string> GetOrSetValuePermanentlyAsync(
		string key,
		Func<CancellationToken, Task<string>> value,
		bool forceSet = false,
		CancellationToken cancellationToken = default)
		=> GetOrSetValueAsync(
			key,
			value,
			options: null,
			forceSet,
			cancellationToken);

	public Task<string> GetOrSetValueWithSlidingExpirationAsync(
		string key,
		Func<CancellationToken, Task<string>> value,
		TimeSpan slidingTime,
		bool forceSet = false,
		CancellationToken cancellationToken = default)
		=> GetOrSetValueAsync(
			key,
			value,
			new MemoryCacheEntryOptions
			{
				SlidingExpiration = slidingTime
			},
			forceSet,
			cancellationToken);

	public Task<string> GetOrSetValueWithAbsoluteExpirationAsync(
		string key,
		Func<CancellationToken, Task<string>> value,
		DateTime keepUntil,
		bool forceSet = false,
		CancellationToken cancellationToken = default)
		=> GetOrSetValueAsync(
			key,
			value,
			new MemoryCacheEntryOptions
			{
				AbsoluteExpiration = keepUntil
			},
			forceSet,
			cancellationToken);

	public async Task<bool> TryUpdateAsync(
		string key,
		string oldValue,
		string newValue,
		Guid currentRowVersion,
		MemoryCacheEntryOptions? options,
		CancellationToken cancellationToken = default)
	{
		var scopeContext = ScopeContext.Create("Legion.ADF.Cache")
			.AddContextProperty(nameof(key), key)
			.AddContextProperty(nameof(options.SlidingExpiration), options?.SlidingExpiration?.ToString())
			.AddContextProperty(nameof(options.AbsoluteExpiration), options?.AbsoluteExpiration?.ToString())
			.AddContextProperty(nameof(options.AbsoluteExpirationRelativeToNow), options?.AbsoluteExpirationRelativeToNow?.ToString());

		Throw.IfArgumentNullOrWhiteSpace(key);
		Throw.IfArgumentNullOrWhiteSpace(oldValue);
		Throw.IfArgumentNullOrWhiteSpace(newValue);

		await using var asyncServiceScope = _serviceProvider.CreateAsyncScope();
		var scopedServiceProvider = asyncServiceScope.ServiceProvider;
		var uow = CreateUnitOfWork(scopeContext, scopedServiceProvider);
		await using var connectionProvider = uow.ConnectionProvider;

		var upadated = await uow.CacheDataRepository.TryUpdateAsync(scopeContext, key, oldValue, newValue, currentRowVersion, options, cancellationToken);

		return upadated;
	}

	public Task<bool> TryUpdateValuePermanentlyAsync(
		string key,
		string oldValue,
		string newValue,
		Guid currentRowVersion,
		CancellationToken cancellationToken = default)
		 => TryUpdateAsync(
				key,
				oldValue,
				newValue,
				currentRowVersion,
				options: null,
				cancellationToken);

	public Task<bool> TryUpdateValueWithSlidingExpirationAsync(
		string key,
		string oldValue,
		string newValue,
		Guid currentRowVersion,
		TimeSpan slidingTime,
		CancellationToken cancellationToken = default)
		 => TryUpdateAsync(
				key,
				oldValue,
				newValue,
				currentRowVersion,
				new MemoryCacheEntryOptions
				{
					SlidingExpiration = slidingTime
				},
				cancellationToken);

	public Task<bool> TryUpdateValueWithAbsoluteExpirationAsync(
		string key,
		string oldValue,
		string newValue,
		Guid currentRowVersion,
		DateTime keepUntil,
		CancellationToken cancellationToken = default)
		 => TryUpdateAsync(
				key,
				oldValue,
				newValue,
				currentRowVersion,
				new MemoryCacheEntryOptions
				{
					AbsoluteExpiration = keepUntil
				},
				cancellationToken);

	public Task<bool> TryUpdateValueWithAbsoluteServerSideExpirationAsync(
		string key,
		string oldValue,
		string newValue,
		Guid currentRowVersion,
		TimeSpan deltaToNowUtc,
		CancellationToken cancellationToken = default)
		 => TryUpdateAsync(
				key,
				oldValue,
				newValue,
				currentRowVersion,
				new MemoryCacheEntryOptions
				{
					AbsoluteExpiration = GlobalContext.Instance.UtcNow.Add(deltaToNowUtc)
				},
				cancellationToken);

	public async Task<bool> RemoveValueAsync(
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

		return await uow.CacheDataRepository.RemoveAsync(scopeContext, key, cancellationToken);
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
