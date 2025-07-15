using Legion.ADF.Cache.Settings;
using Legion.Caching;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Legion.ADF.Cache.Services.Internal;

/// <summary>
/// Service to periodically remove cache keys from IADFCache.
/// </summary>
public class CacheKeyRemoveService : BackgroundService
{
	private readonly IADFCache _cache;
	private readonly CacheKeyRemoveServiceOptions _options;
	private readonly IServiceProvider _serviceProvider;
	private readonly ILogger<CacheKeyRemoveService> _logger;

	public CacheKeyRemoveService(
		IADFCache cache,
		IOptions<CacheKeyRemoveServiceOptions> options,
		IServiceProvider serviceProvider,
		ILogger<CacheKeyRemoveService> logger)
	{
		Throw.IfArgumentNull(cache);
		Throw.IfArgumentNull(options);
		Throw.IfArgumentNull(serviceProvider);
		Throw.IfArgumentNull(logger);

		_cache = cache;
		_options = options.Value;
		_serviceProvider = serviceProvider;
		_logger = logger;
	}

	protected override async Task ExecuteAsync(CancellationToken cancellationToken)
	{
		var scopeContextGlobal = ScopeContext.Create(nameof(CacheKeyRemoveService));

		while (!cancellationToken.IsCancellationRequested)
		{
			var scopeContext = ScopeContext.Create(scopeContextGlobal, correlationId: GlobalContext.Instance.NewGuid());

			_logger.LogTraceMessage(scopeContext, x => x.InternalMessage($"{nameof(CacheKeyRemoveService)}.{nameof(ExecuteAsync)}: START"));

			try
			{
				await using var asyncServiceScope = _serviceProvider.CreateAsyncScope();
				var sp = asyncServiceScope.ServiceProvider;

				var reloadableCacheKeyStore = sp.GetRequiredService<ReloadableCacheKeyStore>();

				var reloadableCacheKeysResult = await reloadableCacheKeyStore.GetAllReloadableCacheKeyInternalAsync(scopeContext, checkPermissions: false, cancellationToken);

				var hasErrorOrNullData = reloadableCacheKeysResult.LogHasError(
					scopeContext,
					_logger,
					dataMustBeNotNull: true,
					errorCode: null,
					skipIfAlreadyLogged: true,
					logWarnings: true);

				if (hasErrorOrNullData)
					return;

				var reloadableCacheKeys = reloadableCacheKeysResult.Data!;

				foreach (var reloadableCacheKey in reloadableCacheKeys)
				{
					if (string.IsNullOrWhiteSpace(reloadableCacheKey.Key))
					{
						if (0 < reloadableCacheKey.Tags?.Count)
							_cache.RemoveValuesForWholeTags(reloadableCacheKey.Tags);
					}
					else
					{
						_cache.RemoveValue(reloadableCacheKey.Key);
					}

					reloadableCacheKeyStore.RemoveReloadableCacheKey(scopeContext, reloadableCacheKey);
				}

				await reloadableCacheKeyStore.SaveAsync(scopeContext, cancellationToken: default);
			}
			catch (Exception ex)
			{
				_logger.LogErrorMessage(scopeContext, Exceptions.Internal.ErrorCodes.CacheKeyRemoveService.Default, x => x.ExceptionInfo(ex));
			}
			finally
			{
				await Task.Delay(TimeSpan.FromSeconds(_options.IdleTimeoutInSeconds), cancellationToken);
			}
		}
	}
}
