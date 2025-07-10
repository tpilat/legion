using Legion.ADF.Cache.DTOs;
using Legion.AspNetCore.WebApi;
using Legion.Caching;
using Legion.Results;
using Microsoft.AspNetCore.Mvc;

namespace Legion.ADF.Cache.RestApi.Controllers.V1;

[ApiRoutePrefix("v1")]
[Route("[controller]")]
public class CacheController : ApiControllerBase
{
	[HttpPost, Route("GetValue")]
	public async Task<ResultDto<CachedValueDto>> GetValueAsync(
		[FromBody] string key,
		CancellationToken cancellationToken = default)
	{
		var scopeContext = GetNewScopeContext();

		var result = new ResultBuilder<CachedValueDto>();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, key))
			return result.Build().ToDto();

		try
		{
			var cache = GetRequiredService<ISimplePersistentCache>();
			var cachedValue = await cache.GetValue(key, cancellationToken);

			return string.IsNullOrWhiteSpace(cachedValue.Value)
				? result.WithData(null).Build().ToDto()
				: result.WithData(
					new CachedValueDto
					{
						Value = cachedValue.Value,
						RowVersion = cachedValue.RowVersion,
					})
					.Build().ToDto();
		}
		catch (Exception ex)
		{
			return result.WithInvalidOperationException(scopeContext, errorCode: null, detail: null, ex).ToDto();
		}
	}

	[HttpPost, Route("SetValuePermanently")]
	public async Task<ResultDto<bool>> SetValuePermanentlyAsync(
		[FromBody] SetCacheDataDto request,
		CancellationToken cancellationToken = default)
	{
		var scopeContext = GetNewScopeContext();

		var result = new ResultBuilder<bool>();

		if (result.IsArgumentNull(scopeContext, request))
			return result.Build().ToDto();

		try
		{
			var cache = GetRequiredService<ISimplePersistentCache>();
			var wasSet = await cache.SetValuePermanentlyAsync(request.Key, request.Value, cancellationToken);

			return result.WithData(wasSet).Build().ToDto();
		}
		catch (Exception ex)
		{
			return result.WithInvalidOperationException(scopeContext, errorCode: null, detail: null, ex).ToDto();
		}
	}

	[HttpPost, Route("SetValueWithSlidingExpiration")]
	public async Task<ResultDto<bool>> SetValueWithSlidingExpirationAsync(
		[FromBody] SetCacheDataDto request,
		CancellationToken cancellationToken = default)
	{
		var scopeContext = GetNewScopeContext();

		var result = new ResultBuilder<bool>();

		if (result.IsArgumentNull(scopeContext, request))
			return result.Build().ToDto();

		try
		{
			var cache = GetRequiredService<ISimplePersistentCache>();
			var wasSet = await cache.SetValueWithSlidingExpirationAsync(request.Key, request.Value, request.SlidingTime.Value, cancellationToken);

			return result.WithData(wasSet).Build().ToDto();
		}
		catch (Exception ex)
		{
			return result.WithInvalidOperationException(scopeContext, errorCode: null, detail: null, ex).ToDto();
		}
	}

	[HttpPost, Route("SetValueWithAbsoluteExpiration")]
	public async Task<ResultDto<bool>> SetValueWithAbsoluteExpirationAsync(
		[FromBody] SetCacheDataDto request,
		CancellationToken cancellationToken = default)
	{
		var scopeContext = GetNewScopeContext();

		var result = new ResultBuilder<bool>();

		if (result.IsArgumentNull(scopeContext, request))
			return result.Build().ToDto();

		Throw.IfArgumentNull(request.KeepUntil);
		Throw.IfArgumentIsLessThanOrEqual(request.KeepUntil.Value, GlobalContext.Instance.UtcNow);

		try
		{
			var cache = GetRequiredService<ISimplePersistentCache>();
			var wasSet = await cache.SetValueWithAbsoluteExpirationAsync(request.Key, request.Value, request.KeepUntil.Value, cancellationToken);

			return result.WithData(wasSet).Build().ToDto();
		}
		catch (Exception ex)
		{
			return result.WithInvalidOperationException(scopeContext, errorCode: null, detail: null, ex).ToDto();
		}
	}

	[HttpPost, Route("TryUpdateValuePermanently")]
	public async Task<ResultDto<bool>> TryUpdateValuePermanentlyAsync(
		[FromBody] UpdateCacheDataDto request,
		CancellationToken cancellationToken = default)
	{
		var scopeContext = GetNewScopeContext();

		var result = new ResultBuilder<bool>();

		if (result.IsArgumentNull(scopeContext, request))
			return result.Build().ToDto();

		try
		{
			var cache = GetRequiredService<ISimplePersistentCache>();
			var updated = await cache.TryUpdateValuePermanentlyAsync(request.Key, request.OldValue, request.NewValue, request.CurrentRowVersion, cancellationToken);

			return result.WithData(updated).Build().ToDto();
		}
		catch (Exception ex)
		{
			return result.WithInvalidOperationException(scopeContext, errorCode: null, detail: null, ex).ToDto();
		}
	}

	[HttpPost, Route("TryUpdateValueWithSlidingExpiration")]
	public async Task<ResultDto<bool>> TryUpdateValueWithSlidingExpirationAsync(
		[FromBody] UpdateCacheDataDto request,
		CancellationToken cancellationToken = default)
	{
		var scopeContext = GetNewScopeContext();

		var result = new ResultBuilder<bool>();

		if (result.IsArgumentNull(scopeContext, request))
			return result.Build().ToDto();

		try
		{
			var cache = GetRequiredService<ISimplePersistentCache>();
			var updated = await cache.TryUpdateValueWithSlidingExpirationAsync(request.Key, request.OldValue, request.NewValue, request.CurrentRowVersion, request.SlidingTime.Value, cancellationToken);

			return result.WithData(updated).Build().ToDto();
		}
		catch (Exception ex)
		{
			return result.WithInvalidOperationException(scopeContext, errorCode: null, detail: null, ex).ToDto();
		}
	}

	[HttpPost, Route("TryUpdateValueWithAbsoluteExpiration")]
	public async Task<ResultDto<bool>> TryUpdateValueWithAbsoluteExpirationAsync(
		[FromBody] UpdateCacheDataDto request,
		CancellationToken cancellationToken = default)
	{
		var scopeContext = GetNewScopeContext();

		var result = new ResultBuilder<bool>();

		if (result.IsArgumentNull(scopeContext, request))
			return result.Build().ToDto();

		try
		{
			var cache = GetRequiredService<ISimplePersistentCache>();
			var updated = await cache.TryUpdateValueWithAbsoluteExpirationAsync(request.Key, request.OldValue, request.NewValue, request.CurrentRowVersion, request.KeepUntil.Value, cancellationToken);

			return result.WithData(updated).Build().ToDto();
		}
		catch (Exception ex)
		{
			return result.WithInvalidOperationException(scopeContext, errorCode: null, detail: null, ex).ToDto();
		}
	}

	[HttpPost, Route("RemoveValue")]
	public async Task<ResultDto<bool>> RemoveValueAsync(
		[FromBody] string key,
		CancellationToken cancellationToken = default)
	{
		var scopeContext = GetNewScopeContext();

		var result = new ResultBuilder<bool>();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, key))
			return result.Build().ToDto();

		try
		{
			var cache = GetRequiredService<ISimplePersistentCache>();
			var removed = await cache.RemoveValueAsync(key, cancellationToken);

			return result.WithData(removed).Build().ToDto();
		}
		catch (Exception ex)
		{
			return result.WithInvalidOperationException(scopeContext, errorCode: null, detail: null, ex).ToDto();
		}
	}
}
