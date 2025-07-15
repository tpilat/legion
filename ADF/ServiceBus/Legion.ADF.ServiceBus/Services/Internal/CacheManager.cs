using Legion.ADF.ServiceBus.Services.Internal.Dto;
using Legion.Caching;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Legion.ADF.ServiceBus.Services.Internal;

internal static partial class CacheManager
{
	public static async Task<bool> IsHostAliveAsync(
		IScopeContext scopeContext,
		string cacheKeySystemName,
		string hostName,
		IServiceProvider serviceProvider,
		ILogger logger,
		CancellationToken cancellationToken)
	{
		scopeContext = scopeContext.CreateNew();

		try
		{
			Throw.IfArgumentNullOrWhiteSpace(cacheKeySystemName);
			Throw.IfArgumentNullOrWhiteSpace(hostName);
			Throw.IfArgumentNull(serviceProvider);
			Throw.IfArgumentNull(logger);

			var simplePersistentCache = serviceProvider.GetRequiredService<ISimplePersistentCache>();
			var (value, rowVersion) = await simplePersistentCache.GetValueAsync(
				Model.Host.GetHostDistributedCacheKey(cacheKeySystemName, hostName, Code.ALIVE),
				cancellationToken);

			return !string.IsNullOrWhiteSpace(value);
		}
		catch (Exception ex)
		{
			logger.LogErrorMessage(
				scopeContext,
				Legion.ADF.ServiceBus.Exceptions.Internal.ErrorCodes.ServiceBusHostException.NoHostFound(hostName),
				x => x.ExceptionInfo(ex));

			return false;
		}
	}

	public static async Task<bool> WriteDistributedHeartbeatAsync(
		IScopeContext scopeContext,
		string cacheKeySystemName,
		Model.HostActivity hostActivity,
		DateTime utcNow,
		HostContext hostContext,
		IServiceProvider serviceProvider,
		Action<Exception> onException,
		CancellationToken cancellationToken)
	{
		scopeContext = scopeContext.CreateNew();

		try
		{
			var simplePersistentCache = serviceProvider.GetRequiredService<ISimplePersistentCache>();
			var set = await simplePersistentCache.SetValueWithAbsoluteServerSideExpirationAsync(
				hostActivity.Host.GetDistributedCacheKey(cacheKeySystemName, "ALIVE"),
				$"host-local-time: {utcNow:yyyy-MM-dd:HH:mm:ss.fff}",
				TimeSpan.FromSeconds(hostContext.HostConfiguration.HeartbeatInSeconds + HostContext._heartbeatDelayDeltaInSeconds),
				cancellationToken);

			return set;
		}
		catch (Exception ex)
		{
			onException?.Invoke(ex);

			return false;
		}
	}

	public static async Task<bool> IsAliveDistributedCacheAsync(
		IScopeContext scopeContext,
		string key,
		IServiceProvider serviceProvider,
		Action<Exception> onException,
		CancellationToken cancellationToken)
	{
		scopeContext = scopeContext.CreateNew();

		try
		{
			var simplePersistentCache = serviceProvider.GetRequiredService<ISimplePersistentCache>();
			var set = await simplePersistentCache.SetValuePermanentlyAsync(
				key,
				$"ALIVE: {GlobalContext.Instance.UtcNow:yyyy-MM-dd:HH:mm:ss.fff}",
				cancellationToken);

			return set;
		}
		catch (Exception ex)
		{
			onException?.Invoke(ex);

			return false;
		}
	}
}
