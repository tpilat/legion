using Legion.ADF.Cache.DTOs;
using Legion.ADF.Cache.RestApi.Client.Requests;
using Legion.Caching;
using Legion.Locks;
using Legion.NetHttp;

namespace Legion.ADF.Cache.RestApi.Client;

public partial class CacheRestApiClient : HttpApiClient<CacheRestApiClientOptions>, ISimplePersistentCache, IDistributedLockProvider
{
	public async Task<IResult<bool>> IsAliveV1Async(
		IScopeContext scopeContext,
		int? timeoutInSeconds = null,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew()
			.AddContextProperty(nameof(Options.BaseAddress), Options?.BaseAddress)
			.AddContextProperty(nameof(URI), URI.Cache.V1.IsAlive);

		var result = new ResultBuilder<bool>();

		if (timeoutInSeconds.HasValue && result.IsArgumentLessThanOrEqual(scopeContext, timeoutInSeconds.Value, 0))
			return result.Build();

		var request = JsonRequestFactory.Create(
			Options!,
			Legion.Http.HttpMethod.Post,
			URI.Cache.V1.IsAlive,
			timeoutInSeconds,
			queryString: null);

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

	public async Task<IResult<CachedValueDto?>> GetValueV1Async(
		IScopeContext scopeContext,
		string key,
		int? timeoutInSeconds = null,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew()
			.AddContextProperty(nameof(Options.BaseAddress), Options?.BaseAddress)
			.AddContextProperty(nameof(URI), URI.Cache.V1.GetValue);

		var result = new ResultBuilder<CachedValueDto?>();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, key))
			return result.Build();

		if (timeoutInSeconds.HasValue && result.IsArgumentLessThanOrEqual(scopeContext, timeoutInSeconds.Value, 0))
			return result.Build();

		var request = JsonRequestFactory.Create(
			Options!,
			Legion.Http.HttpMethod.Post,
			URI.Cache.V1.GetValue,
			timeoutInSeconds,
			queryString: null,
			key);

		try
		{
			using var response = await SendAsync(request, scopeContext, serviceProvider: null, cancellationToken: cancellationToken);
			var jsonResponse = await ToJsonResultAsync<CachedValueDto?>(scopeContext, request, response, cancellationToken);
			return jsonResponse!;
		}
		catch (Exception ex)
		{
			return
				new ResultBuilder<CachedValueDto?>()
					.WithError(scopeContext, Legion.ADF.Cache.RestApi.Client.Internal.ErrorCodes.ApiClientException.Default(nameof(CacheRestApiClient)), x => x.ExceptionInfo(ex));
		}
	}

	public async Task<IResult<bool>> SetValuePermanentlyV1Async(
		IScopeContext scopeContext,
		SetCacheDataDto setCacheData,
		int? timeoutInSeconds = null,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew()
			.AddContextProperty(nameof(Options.BaseAddress), Options?.BaseAddress)
			.AddContextProperty(nameof(URI), URI.Cache.V1.SetValuePermanently);

		var result = new ResultBuilder<bool>();

		if (result.IsArgumentNull(scopeContext, setCacheData))
			return result.Build();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, setCacheData.Key))
			return result.Build();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, setCacheData.Value))
			return result.Build();

		if (timeoutInSeconds.HasValue && result.IsArgumentLessThanOrEqual(scopeContext, timeoutInSeconds.Value, 0))
			return result.Build();

		var request = JsonRequestFactory.Create(
			Options!,
			Legion.Http.HttpMethod.Post,
			URI.Cache.V1.SetValuePermanently,
			timeoutInSeconds,
			queryString: null,
			setCacheData);

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

	public async Task<IResult<bool>> SetValueWithSlidingExpirationV1Async(
		IScopeContext scopeContext,
		SetCacheDataDto setCacheData,
		int? timeoutInSeconds = null,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew()
			.AddContextProperty(nameof(Options.BaseAddress), Options?.BaseAddress)
			.AddContextProperty(nameof(URI), URI.Cache.V1.SetValueWithSlidingExpiration);

		var result = new ResultBuilder<bool>();

		if (result.IsArgumentNull(scopeContext, setCacheData))
			return result.Build();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, setCacheData.Key))
			return result.Build();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, setCacheData.Value))
			return result.Build();

		if (result.IsArgumentNull(scopeContext, setCacheData.SlidingTime))
			return result.Build();

		if (result.IsArgumentLessThanOrEqual(scopeContext, setCacheData.SlidingTime.Value, TimeSpan.Zero))
			return result.Build();

		if (timeoutInSeconds.HasValue && result.IsArgumentLessThanOrEqual(scopeContext, timeoutInSeconds.Value, 0))
			return result.Build();

		var request = JsonRequestFactory.Create(
			Options!,
			Legion.Http.HttpMethod.Post,
			URI.Cache.V1.SetValueWithSlidingExpiration,
			timeoutInSeconds,
			queryString: null,
			setCacheData);

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

	public async Task<IResult<bool>> SetValueWithAbsoluteExpirationV1Async(
		IScopeContext scopeContext,
		SetCacheDataDto setCacheData,
		int? timeoutInSeconds = null,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew()
			.AddContextProperty(nameof(Options.BaseAddress), Options?.BaseAddress)
			.AddContextProperty(nameof(URI), URI.Cache.V1.SetValueWithAbsoluteExpiration);

		var result = new ResultBuilder<bool>();

		if (result.IsArgumentNull(scopeContext, setCacheData))
			return result.Build();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, setCacheData.Key))
			return result.Build();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, setCacheData.Value))
			return result.Build();

		if (result.IsArgumentNull(scopeContext, setCacheData.KeepUntil))
			return result.Build();

		if (result.IsArgumentLessThanOrEqual(scopeContext, setCacheData.KeepUntil.Value, GlobalContext.Instance.UtcNow))
			return result.Build();

		if (timeoutInSeconds.HasValue && result.IsArgumentLessThanOrEqual(scopeContext, timeoutInSeconds.Value, 0))
			return result.Build();

		var request = JsonRequestFactory.Create(
			Options!,
			Legion.Http.HttpMethod.Post,
			URI.Cache.V1.SetValueWithAbsoluteExpiration,
			timeoutInSeconds,
			queryString: null,
			setCacheData);

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

	public async Task<IResult<bool>> SetValueWithAbsoluteServerSideExpirationV1Async(
		IScopeContext scopeContext,
		SetCacheDataDto setCacheData,
		int? timeoutInSeconds = null,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew()
			.AddContextProperty(nameof(Options.BaseAddress), Options?.BaseAddress)
			.AddContextProperty(nameof(URI), URI.Cache.V1.SetValueWithAbsoluteServerSideExpiration);

		var result = new ResultBuilder<bool>();

		if (result.IsArgumentNull(scopeContext, setCacheData))
			return result.Build();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, setCacheData.Key))
			return result.Build();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, setCacheData.Value))
			return result.Build();

		if (result.IsArgumentNull(scopeContext, setCacheData.SlidingTime))
			return result.Build();

		if (result.IsArgumentLessThanOrEqual(scopeContext, setCacheData.SlidingTime.Value, TimeSpan.Zero))
			return result.Build();

		if (timeoutInSeconds.HasValue && result.IsArgumentLessThanOrEqual(scopeContext, timeoutInSeconds.Value, 0))
			return result.Build();

		var request = JsonRequestFactory.Create(
			Options!,
			Legion.Http.HttpMethod.Post,
			URI.Cache.V1.SetValueWithAbsoluteServerSideExpiration,
			timeoutInSeconds,
			queryString: null,
			setCacheData);

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

	public async Task<IResult<bool>> TryUpdateValuePermanentlyV1Async(
		IScopeContext scopeContext,
		UpdateCacheDataDto updateCacheData,
		int? timeoutInSeconds = null,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew()
			.AddContextProperty(nameof(Options.BaseAddress), Options?.BaseAddress)
			.AddContextProperty(nameof(URI), URI.Cache.V1.TryUpdateValuePermanently);

		var result = new ResultBuilder<bool>();

		if (result.IsArgumentNull(scopeContext, updateCacheData))
			return result.Build();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, updateCacheData.Key))
			return result.Build();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, updateCacheData.OldValue))
			return result.Build();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, updateCacheData.NewValue))
			return result.Build();

		if (timeoutInSeconds.HasValue && result.IsArgumentLessThanOrEqual(scopeContext, timeoutInSeconds.Value, 0))
			return result.Build();

		var request = JsonRequestFactory.Create(
			Options!,
			Legion.Http.HttpMethod.Post,
			URI.Cache.V1.TryUpdateValuePermanently,
			timeoutInSeconds,
			queryString: null,
			updateCacheData);

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

	public async Task<IResult<bool>> TryUpdateValueWithSlidingExpirationV1Async(
		IScopeContext scopeContext,
		UpdateCacheDataDto updateCacheData,
		int? timeoutInSeconds = null,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew()
			.AddContextProperty(nameof(Options.BaseAddress), Options?.BaseAddress)
			.AddContextProperty(nameof(URI), URI.Cache.V1.TryUpdateValueWithSlidingExpiration);

		var result = new ResultBuilder<bool>();

		if (result.IsArgumentNull(scopeContext, updateCacheData))
			return result.Build();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, updateCacheData.Key))
			return result.Build();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, updateCacheData.OldValue))
			return result.Build();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, updateCacheData.NewValue))
			return result.Build();

		if (result.IsArgumentNull(scopeContext, updateCacheData.SlidingTime))
			return result.Build();

		if (result.IsArgumentLessThanOrEqual(scopeContext, updateCacheData.SlidingTime.Value, TimeSpan.Zero))
			return result.Build();

		if (timeoutInSeconds.HasValue && result.IsArgumentLessThanOrEqual(scopeContext, timeoutInSeconds.Value, 0))
			return result.Build();

		var request = JsonRequestFactory.Create(
			Options!,
			Legion.Http.HttpMethod.Post,
			URI.Cache.V1.TryUpdateValueWithSlidingExpiration,
			timeoutInSeconds,
			queryString: null,
			updateCacheData);

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

	public async Task<IResult<bool>> TryUpdateValueWithAbsoluteExpirationV1Async(
		IScopeContext scopeContext,
		UpdateCacheDataDto updateCacheData,
		int? timeoutInSeconds = null,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew()
			.AddContextProperty(nameof(Options.BaseAddress), Options?.BaseAddress)
			.AddContextProperty(nameof(URI), URI.Cache.V1.TryUpdateValueWithAbsoluteExpiration);

		var result = new ResultBuilder<bool>();

		if (result.IsArgumentNull(scopeContext, updateCacheData))
			return result.Build();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, updateCacheData.Key))
			return result.Build();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, updateCacheData.OldValue))
			return result.Build();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, updateCacheData.NewValue))
			return result.Build();

		if (result.IsArgumentNull(scopeContext, updateCacheData.KeepUntil))
			return result.Build();

		if (result.IsArgumentLessThanOrEqual(scopeContext, updateCacheData.KeepUntil.Value, GlobalContext.Instance.UtcNow))
			return result.Build();

		if (timeoutInSeconds.HasValue && result.IsArgumentLessThanOrEqual(scopeContext, timeoutInSeconds.Value, 0))
			return result.Build();

		var request = JsonRequestFactory.Create(
			Options!,
			Legion.Http.HttpMethod.Post,
			URI.Cache.V1.TryUpdateValueWithAbsoluteExpiration,
			timeoutInSeconds,
			queryString: null,
			updateCacheData);

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

	public async Task<IResult<bool>> TryUpdateValueWithAbsoluteServerSideExpirationV1Async(
		IScopeContext scopeContext,
		UpdateCacheDataDto updateCacheData,
		int? timeoutInSeconds = null,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew()
			.AddContextProperty(nameof(Options.BaseAddress), Options?.BaseAddress)
			.AddContextProperty(nameof(URI), URI.Cache.V1.TryUpdateValueWithAbsoluteServerSideExpiration);

		var result = new ResultBuilder<bool>();

		if (result.IsArgumentNull(scopeContext, updateCacheData))
			return result.Build();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, updateCacheData.Key))
			return result.Build();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, updateCacheData.OldValue))
			return result.Build();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, updateCacheData.NewValue))
			return result.Build();

		if (result.IsArgumentNull(scopeContext, updateCacheData.SlidingTime))
			return result.Build();

		if (result.IsArgumentLessThanOrEqual(scopeContext, updateCacheData.SlidingTime.Value, TimeSpan.Zero))
			return result.Build();

		if (timeoutInSeconds.HasValue && result.IsArgumentLessThanOrEqual(scopeContext, timeoutInSeconds.Value, 0))
			return result.Build();

		var request = JsonRequestFactory.Create(
			Options!,
			Legion.Http.HttpMethod.Post,
			URI.Cache.V1.TryUpdateValueWithAbsoluteServerSideExpiration,
			timeoutInSeconds,
			queryString: null,
			updateCacheData);

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

	public async Task<IResult<bool>> RemoveValueV1Async(
		IScopeContext scopeContext,
		string key,
		int? timeoutInSeconds = null,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew()
			.AddContextProperty(nameof(Options.BaseAddress), Options?.BaseAddress)
			.AddContextProperty(nameof(URI), URI.Cache.V1.RemoveValue);

		var result = new ResultBuilder<bool>();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, key))
			return result.Build();

		if (timeoutInSeconds.HasValue && result.IsArgumentLessThanOrEqual(scopeContext, timeoutInSeconds.Value, 0))
			return result.Build();

		var request = JsonRequestFactory.Create(
			Options!,
			Legion.Http.HttpMethod.Post,
			URI.Cache.V1.RemoveValue,
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
	async Task<bool> ISimplePersistentCache.IsAliveAsync(CancellationToken cancellationToken)
	{
		var scopeContext = ScopeContext.Create("Legion.ADF.Cache.RestApi.Client");
		var result = await IsAliveV1Async(scopeContext, timeoutInSeconds: null, cancellationToken).ConfigureAwait(false);
		result.ThrowIfError(scopeContext, null, true);
		return result.Data;
	}

	async Task<(string? Value, Guid? RowVersion)> ISimplePersistentCache.GetValueAsync(
		string key,
		CancellationToken cancellationToken)
	{
		var scopeContext = ScopeContext.Create("Legion.ADF.Cache.RestApi.Client");
		var result = await GetValueV1Async(scopeContext, key, null, cancellationToken).ConfigureAwait(false);
		result.ThrowIfError(scopeContext, null, true);
		return (result.Data?.Value, result.Data?.RowVersion);
	}

	async Task<bool> ISimplePersistentCache.SetValuePermanentlyAsync(
		string key,
		string value,
		CancellationToken cancellationToken)
	{
		var scopeContext = ScopeContext.Create("Legion.ADF.Cache.RestApi.Client");
		var result = await SetValuePermanentlyV1Async(
			scopeContext,
			new SetCacheDataDto
			{
				Key = key,
				Value = value
			},
			null,
			cancellationToken).ConfigureAwait(false);

		result.ThrowIfErrorOrNullData(scopeContext, null, true);
		return result.Data;
	}

	async Task<bool> ISimplePersistentCache.SetValueWithSlidingExpirationAsync(
		string key,
		string value,
		TimeSpan slidingTime,
		CancellationToken cancellationToken)
	{
		var scopeContext = ScopeContext.Create("Legion.ADF.Cache.RestApi.Client");
		var result = await SetValueWithSlidingExpirationV1Async(
			scopeContext,
			new SetCacheDataDto
			{
				Key = key,
				Value = value,
				SlidingTime = slidingTime
			},
			null,
			cancellationToken).ConfigureAwait(false);

		result.ThrowIfErrorOrNullData(scopeContext, null, true);
		return result.Data;
	}

	async Task<bool> ISimplePersistentCache.SetValueWithAbsoluteExpirationAsync(
		string key,
		string value,
		DateTime keepUntil,
		CancellationToken cancellationToken)
	{
		var scopeContext = ScopeContext.Create("Legion.ADF.Cache.RestApi.Client");
		var result = await SetValueWithAbsoluteExpirationV1Async(
			scopeContext,
			new SetCacheDataDto
			{
				Key = key,
				Value = value,
				KeepUntil = keepUntil
			},
			null,
			cancellationToken).ConfigureAwait(false);
		result.ThrowIfErrorOrNullData(scopeContext, null, true);
		return result.Data;
	}

	async Task<bool> ISimplePersistentCache.SetValueWithAbsoluteServerSideExpirationAsync(
		string key,
		string value,
		TimeSpan deltaToNowUtc,
		CancellationToken cancellationToken)
	{
		var scopeContext = ScopeContext.Create("Legion.ADF.Cache.RestApi.Client");
		var result = await SetValueWithAbsoluteServerSideExpirationV1Async(
			scopeContext,
			new SetCacheDataDto
			{
				Key = key,
				Value = value,
				SlidingTime = deltaToNowUtc
			},
			null,
			cancellationToken).ConfigureAwait(false);
		result.ThrowIfErrorOrNullData(scopeContext, null, true);
		return result.Data;
	}

	async Task<bool> ISimplePersistentCache.TryUpdateValuePermanentlyAsync(
		string key,
		string oldValue,
		string newValue,
		Guid currentRowVersion,
		CancellationToken cancellationToken)
	{
		var scopeContext = ScopeContext.Create("Legion.ADF.Cache.RestApi.Client");
		var result = await TryUpdateValuePermanentlyV1Async(
			scopeContext,
			new UpdateCacheDataDto
			{
				Key = key,
				OldValue = oldValue,
				NewValue = newValue,
				CurrentRowVersion = currentRowVersion
			},
			null,
			cancellationToken).ConfigureAwait(false);

		result.ThrowIfErrorOrNullData(scopeContext, null, true);
		return result.Data;
	}

	async Task<bool> ISimplePersistentCache.TryUpdateValueWithSlidingExpirationAsync(
		string key,
		string oldValue,
		string newValue,
		Guid currentRowVersion,
		TimeSpan slidingTime,
		CancellationToken cancellationToken)
	{
		var scopeContext = ScopeContext.Create("Legion.ADF.Cache.RestApi.Client");
		var result = await TryUpdateValueWithSlidingExpirationV1Async(
			scopeContext,
			new UpdateCacheDataDto
			{
				Key = key,
				OldValue = oldValue,
				NewValue = newValue,
				CurrentRowVersion = currentRowVersion,
				SlidingTime = slidingTime
			},
			null,
			cancellationToken).ConfigureAwait(false);
		result.ThrowIfErrorOrNullData(scopeContext, null, true);
		return result.Data;
	}

	async Task<bool> ISimplePersistentCache.TryUpdateValueWithAbsoluteExpirationAsync(
		string key,
		string oldValue,
		string newValue,
		Guid currentRowVersion,
		DateTime keepUntil,
		CancellationToken cancellationToken)
	{
		var scopeContext = ScopeContext.Create("Legion.ADF.Cache.RestApi.Client");
		var result = await TryUpdateValueWithAbsoluteExpirationV1Async(
			scopeContext,
			new UpdateCacheDataDto
			{
				Key = key,
				OldValue = oldValue,
				NewValue = newValue,
				CurrentRowVersion = currentRowVersion,
				KeepUntil = keepUntil
			},
			null,
			cancellationToken).ConfigureAwait(false);
		result.ThrowIfErrorOrNullData(scopeContext, null, true);
		return result.Data;
	}

	async Task<bool> ISimplePersistentCache.TryUpdateValueWithAbsoluteServerSideExpirationAsync(
		string key,
		string oldValue,
		string newValue,
		Guid currentRowVersion,
		TimeSpan deltaToNowUtc,
		CancellationToken cancellationToken)
	{
		var scopeContext = ScopeContext.Create("Legion.ADF.Cache.RestApi.Client");
		var result = await TryUpdateValueWithAbsoluteServerSideExpirationV1Async(
			scopeContext,
			new UpdateCacheDataDto
			{
				Key = key,
				OldValue = oldValue,
				NewValue = newValue,
				CurrentRowVersion = currentRowVersion,
				SlidingTime = deltaToNowUtc
			},
			null,
			cancellationToken).ConfigureAwait(false);
		result.ThrowIfErrorOrNullData(scopeContext, null, true);
		return result.Data;
	}

	async Task<bool> ISimplePersistentCache.RemoveValueAsync(
		string key,
		CancellationToken cancellationToken)
	{
		var scopeContext = ScopeContext.Create("Legion.ADF.Cache.RestApi.Client");
		var result = await RemoveValueV1Async(scopeContext, key, null, cancellationToken).ConfigureAwait(false);
		result.ThrowIfErrorOrNullData(scopeContext, null, true);
		return result.Data;
	}
}
