using Legion.ADF.Cache.DTOs;
using Legion.AspNetCore.WebApi;
using Legion.Locks;
using Legion.Results;
using Microsoft.AspNetCore.Mvc;

namespace Legion.ADF.Cache.RestApi.Controllers.V1;

[ApiRoutePrefix("v1")]
[Route("[controller]")]
public class LockController : ApiControllerBase
{
	[HttpPost, Route("Exists")]
	public async Task<ResultDto<bool>> ExistsAsync(
		[FromBody] string key,
		CancellationToken cancellationToken = default)
	{
		var scopeContext = GetNewScopeContext();
		
		var result = new ResultBuilder<bool>();

		try
		{
			var cache = GetRequiredService<IDistributedLockProvider>();
			var exists = await cache.ExistsAsync(key, cancellationToken);

			return result.WithData(exists).Build().ToDto();
		}
		catch (Exception ex)
		{
			return result.WithInvalidOperationException(scopeContext, errorCode: null, detail: null, ex).ToDto();
		}
	}

	[HttpPost, Route("GetMetadata")]
	public async Task<ResultDto<string>> GetMetadataAsync(
		[FromBody] string key,
		CancellationToken cancellationToken = default)
	{
		var scopeContext = GetNewScopeContext();

		var result = new ResultBuilder<string>();

		try
		{
			var cache = GetRequiredService<IDistributedLockProvider>();
			var metadata = await cache.GetMetadataAsync(key, cancellationToken);

			return result.WithData(metadata).Build().ToDto();
		}
		catch (Exception ex)
		{
			return result.WithInvalidOperationException(scopeContext, errorCode: null, detail: null, ex).ToDto();
		}
	}

	[HttpPost, Route("TryAcquireLock")]
	public async Task<ResultDto<string>> TryAcquireLockAsync(
		[FromBody] AcquireDistributedLockDto request,
		CancellationToken cancellationToken = default)
	{
		var scopeContext = GetNewScopeContext();

		var result = new ResultBuilder<string>();

		if (result.IsArgumentNull(scopeContext, request))
			return result.Build().ToDto();
		
		try
		{
			var cache = GetRequiredService<IDistributedLockProvider>();
			var lockId = await cache.TryAcquireLockAsync(request.Key, request.LockTimeout, request.Metadata, request.RetryDelay, request.MaxRetries, cancellationToken);

			return result.WithData(lockId).Build().ToDto();
		}
		catch (Exception ex)
		{
			return result.WithInvalidOperationException(scopeContext, errorCode: null, detail: null, ex).ToDto();
		}
	}

	[HttpPost, Route("ReleaseLock")]
	public async Task<ResultDto<bool>> ReleaseLockAsync(
		[FromBody] DistributedLockDto request,
		CancellationToken cancellationToken = default)
	{
		var scopeContext = GetNewScopeContext();

		var result = new ResultBuilder<bool>();

		if (result.IsArgumentNull(scopeContext, request))
			return result.Build().ToDto();

		try
		{
			var cache = GetRequiredService<IDistributedLockProvider>();
			var released = await cache.ReleaseLockAsync(request.Key, request.LockId, cancellationToken);

			return result.WithData(released).Build().ToDto();
		}
		catch (Exception ex)
		{
			return result.WithInvalidOperationException(scopeContext, errorCode: null, detail: null, ex).ToDto();
		}
	}

	[HttpPost, Route("RenewLock")]
	public async Task<ResultDto<bool>> RenewLockAsync(
		[FromBody] DistributedLockDto request,
		CancellationToken cancellationToken = default)
	{
		var scopeContext = GetNewScopeContext();

		var result = new ResultBuilder<bool>();

		if (result.IsArgumentNull(scopeContext, request))
			return result.Build().ToDto();

		try
		{
			var cache = GetRequiredService<IDistributedLockProvider>();
			var renewed = await cache.RenewLockAsync(request.Key, request.LockId, request.LockTimeout, cancellationToken);

			return result.WithData(renewed).Build().ToDto();
		}
		catch (Exception ex)
		{
			return result.WithInvalidOperationException(scopeContext, errorCode: null, detail: null, ex).ToDto();
		}
	}
}
