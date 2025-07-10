using Legion.ADF.Cache.DTOs;
using Legion.ADF.Cache.RestApi.Client.Requests;
using Legion.Caching;
using Legion.Locks;
using Legion.NetHttp;

namespace Legion.ADF.Cache.RestApi.Client;

public partial class CacheRestApiClient : HttpApiClient<CacheRestApiClientOptions>, ISimplePersistentCache, IDistributedLockProvider
{
	public async Task<IResult<bool>> ExistsV1Async(
		IScopeContext scopeContext,
		string key,
		int? timeoutInSeconds = null,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew()
			.AddContextProperty(nameof(Options.BaseAddress), Options?.BaseAddress)
			.AddContextProperty(nameof(URI), URI.Lock.V1.Exists);

		var result = new ResultBuilder<bool>();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, key))
			return result.Build();

		if (timeoutInSeconds.HasValue && result.IsArgumentLessThanOrEqual(scopeContext, timeoutInSeconds.Value, 0))
			return result.Build();

		var request = JsonRequestFactory.Create(
			Options!,
			Legion.Http.HttpMethod.Post,
			URI.Lock.V1.Exists,
			timeoutInSeconds,
			queryString: null,
			key);

		try
		{
			using var response = await SendAsync(request, scopeContext, serviceProvider: null, cancellationToken: cancellationToken);
			var jsonResponse = await ToJsonResultAsync<bool>(scopeContext, request, response, cancellationToken);
			return jsonResponse!;
		}
		catch (Exception ex)
		{
			return
				new ResultBuilder<bool>()
					.WithError(scopeContext, Legion.ADF.Cache.RestApi.Client.Internal.ErrorCodes.ApiClientException.Default(nameof(CacheRestApiClient)), x => x.ExceptionInfo(ex));
		}
	}

	public async Task<IResult<string?>> GetMetadataV1Async(
		IScopeContext scopeContext,
		string key,
		int? timeoutInSeconds = null,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew()
			.AddContextProperty(nameof(Options.BaseAddress), Options?.BaseAddress)
			.AddContextProperty(nameof(URI), URI.Lock.V1.GetMetadata);

		var result = new ResultBuilder<string?>();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, key))
			return result.Build();

		if (timeoutInSeconds.HasValue && result.IsArgumentLessThanOrEqual(scopeContext, timeoutInSeconds.Value, 0))
			return result.Build();

		var request = JsonRequestFactory.Create(
			Options!,
			Legion.Http.HttpMethod.Post,
			URI.Lock.V1.GetMetadata,
			timeoutInSeconds,
			queryString: null,
			key);

		try
		{
			using var response = await SendAsync(request, scopeContext, serviceProvider: null, cancellationToken: cancellationToken);
			var jsonResponse = await ToJsonResultAsync<string?>(scopeContext, request, response, cancellationToken);
			return jsonResponse!;
		}
		catch (Exception ex)
		{
			return
				new ResultBuilder<string?>()
					.WithError(scopeContext, Legion.ADF.Cache.RestApi.Client.Internal.ErrorCodes.ApiClientException.Default(nameof(CacheRestApiClient)), x => x.ExceptionInfo(ex));
		}
	}

	public async Task<IResult<string?>> TryAcquireLockV1Async(
		IScopeContext scopeContext,
		AcquireDistributedLockDto acquireDistributedLock,
		int? timeoutInSeconds = null,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew()
			.AddContextProperty(nameof(Options.BaseAddress), Options?.BaseAddress)
			.AddContextProperty(nameof(URI), URI.Lock.V1.TryAcquireLock);

		var result = new ResultBuilder<string?>();

		if (result.IsArgumentNull(scopeContext, acquireDistributedLock))
			return result.Build();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, acquireDistributedLock.Key))
			return result.Build();

		if (result.IsArgumentLessThanOrEqual(scopeContext, acquireDistributedLock.LockTimeout, TimeSpan.Zero))
			return result.Build();

		if (acquireDistributedLock.RetryDelay.HasValue && result.IsArgumentLessThanOrEqual(scopeContext, acquireDistributedLock.RetryDelay.Value, TimeSpan.Zero))
			return result.Build();

		if (acquireDistributedLock.MaxRetries.HasValue && result.IsArgumentLessThanOrEqual(scopeContext, acquireDistributedLock.MaxRetries.Value, 0))
			return result.Build();

		if (timeoutInSeconds.HasValue && result.IsArgumentLessThanOrEqual(scopeContext, timeoutInSeconds.Value, 0))
			return result.Build();

		var request = JsonRequestFactory.Create(
			Options!,
			Legion.Http.HttpMethod.Post,
			URI.Lock.V1.TryAcquireLock,
			timeoutInSeconds,
			queryString: null,
			acquireDistributedLock);

		try
		{
			using var response = await SendAsync(request, scopeContext, serviceProvider: null, cancellationToken: cancellationToken);
			var jsonResponse = await ToJsonResultAsync<string?>(scopeContext, request, response, cancellationToken);
			return jsonResponse!;
		}
		catch (Exception ex)
		{
			return
				new ResultBuilder<string?>()
					.WithError(scopeContext, Legion.ADF.Cache.RestApi.Client.Internal.ErrorCodes.ApiClientException.Default(nameof(CacheRestApiClient)), x => x.ExceptionInfo(ex));
		}
	}

	public async Task<IResult<bool>> ReleaseLockV1Async(
		IScopeContext scopeContext,
		DistributedLockDto distributedLock,
		int? timeoutInSeconds = null,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew()
			.AddContextProperty(nameof(Options.BaseAddress), Options?.BaseAddress)
			.AddContextProperty(nameof(URI), URI.Lock.V1.ReleaseLock);

		var result = new ResultBuilder<bool>();

		if (result.IsArgumentNull(scopeContext, distributedLock))
			return result.Build();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, distributedLock.Key))
			return result.Build();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, distributedLock.LockId))
			return result.Build();

		if (timeoutInSeconds.HasValue && result.IsArgumentLessThanOrEqual(scopeContext, timeoutInSeconds.Value, 0))
			return result.Build();

		var request = JsonRequestFactory.Create(
			Options!,
			Legion.Http.HttpMethod.Post,
			URI.Lock.V1.ReleaseLock,
			timeoutInSeconds,
			queryString: null,
			distributedLock);

		try
		{
			using var response = await SendAsync(request, scopeContext, serviceProvider: null, cancellationToken: cancellationToken);
			var jsonResponse = await ToJsonResultAsync<bool>(scopeContext, request, response, cancellationToken);
			return jsonResponse!;
		}
		catch (Exception ex)
		{
			return
				new ResultBuilder<bool>()
					.WithError(scopeContext, Legion.ADF.Cache.RestApi.Client.Internal.ErrorCodes.ApiClientException.Default(nameof(CacheRestApiClient)), x => x.ExceptionInfo(ex));
		}
	}

	public async Task<IResult<bool>> RenewLockV1Async(
		IScopeContext scopeContext,
		DistributedLockDto distributedLock,
		int? timeoutInSeconds = null,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew()
			.AddContextProperty(nameof(Options.BaseAddress), Options?.BaseAddress)
			.AddContextProperty(nameof(URI), URI.Lock.V1.RenewLock);

		var result = new ResultBuilder<bool>();

		if (result.IsArgumentNull(scopeContext, distributedLock))
			return result.Build();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, distributedLock.Key))
			return result.Build();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, distributedLock.LockId))
			return result.Build();

		if (result.IsArgumentLessThanOrEqual(scopeContext, distributedLock.LockTimeout, TimeSpan.Zero))
			return result.Build();

		if (timeoutInSeconds.HasValue && result.IsArgumentLessThanOrEqual(scopeContext, timeoutInSeconds.Value, 0))
			return result.Build();

		var request = JsonRequestFactory.Create(
			Options!,
			Legion.Http.HttpMethod.Post,
			URI.Lock.V1.RenewLock,
			timeoutInSeconds,
			queryString: null,
			distributedLock);

		try
		{
			using var response = await SendAsync(request, scopeContext, serviceProvider: null, cancellationToken: cancellationToken);
			var jsonResponse = await ToJsonResultAsync<bool>(scopeContext, request, response, cancellationToken);
			return jsonResponse!;
		}
		catch (Exception ex)
		{
			return
				new ResultBuilder<bool>()
					.WithError(scopeContext, Legion.ADF.Cache.RestApi.Client.Internal.ErrorCodes.ApiClientException.Default(nameof(CacheRestApiClient)), x => x.ExceptionInfo(ex));
		}
	}

	async Task<bool> IDistributedLockProvider.ExistsAsync(
		string key,
		CancellationToken cancellationToken)
	{
		var scopeContext = ScopeContext.Create("Legion.ADF.Cache.RestApi.Client");
		var result = await ExistsV1Async(scopeContext, key, null, cancellationToken).ConfigureAwait(false);
		result.ThrowIfErrorOrNullData(scopeContext, null, true);
		return result.Data;
	}

	async Task<string?> IDistributedLockProvider.GetMetadataAsync(
		string key,
		CancellationToken cancellationToken)
	{
		var scopeContext = ScopeContext.Create("Legion.ADF.Cache.RestApi.Client");
		var result = await GetMetadataV1Async(scopeContext, key, null, cancellationToken).ConfigureAwait(false);
		result.ThrowIfError(scopeContext, null, true);
		return result.Data;
	}

	async Task<string?> IDistributedLockProvider.TryAcquireLockAsync(
		string key,
		TimeSpan timeout,
		string? metadata,
		TimeSpan? retryDelay,
		int? maxRetries,
		CancellationToken cancellationToken)
	{
		var scopeContext = ScopeContext.Create("Legion.ADF.Cache.RestApi.Client");
		var result = await TryAcquireLockV1Async(
			scopeContext,
			new AcquireDistributedLockDto
			{
				Key = key,
				LockTimeout = timeout,
				Metadata = metadata,
				RetryDelay = retryDelay,
				MaxRetries = maxRetries,
			},
			null,
			cancellationToken).ConfigureAwait(false);

		result.ThrowIfError(scopeContext, null, true);
		return result.Data;
	}

	async Task<bool> IDistributedLockProvider.ReleaseLockAsync(
		string key,
		string lockId,
		CancellationToken cancellationToken)
	{
		var scopeContext = ScopeContext.Create("Legion.ADF.Cache.RestApi.Client");
		var result = await ReleaseLockV1Async(
			scopeContext,
			new DistributedLockDto
			{
				Key = key,
				LockId = lockId
			},
			null,
			cancellationToken).ConfigureAwait(false);

		result.ThrowIfErrorOrNullData(scopeContext, null, true);
		return result.Data;
	}

	async Task<bool> IDistributedLockProvider.RenewLockAsync(
		string key,
		string lockId,
		TimeSpan lockTimeout,
		CancellationToken cancellationToken)
	{
		var scopeContext = ScopeContext.Create("Legion.ADF.Cache.RestApi.Client");
		var result = await RenewLockV1Async(
			scopeContext,
			new DistributedLockDto
			{
				Key = key,
				LockId = lockId,
				LockTimeout = lockTimeout
			},
			null,
			cancellationToken).ConfigureAwait(false);

		result.ThrowIfErrorOrNullData(scopeContext, null, true);
		return result.Data;
	}
}
