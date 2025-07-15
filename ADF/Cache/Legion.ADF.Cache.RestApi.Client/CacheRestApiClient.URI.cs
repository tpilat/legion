using Legion.Caching;
using Legion.Locks;
using Legion.NetHttp;

namespace Legion.ADF.Cache.RestApi.Client;

public partial class CacheRestApiClient : HttpApiClient<CacheRestApiClientOptions>, ISimplePersistentCache, IDistributedLockProvider
{
	public static class URI
	{
		public static class Cache
		{
			public static class V1
			{
				public const string IsAlive = "/api/v1/Cache/IsAlive";
				public const string GetValue = "/api/v1/Cache/GetValue";
				public const string SetValuePermanently = "/api/v1/Cache/SetValuePermanently";
				public const string SetValueWithSlidingExpiration = "/api/v1/Cache/SetValueWithSlidingExpiration";
				public const string SetValueWithAbsoluteExpiration = "/api/v1/Cache/SetValueWithAbsoluteExpiration";
				public const string SetValueWithAbsoluteServerSideExpiration = "/api/v1/Cache/SetValueWithAbsoluteServerSideExpiration";
				public const string TryUpdateValuePermanently = "/api/v1/Cache/TryUpdateValuePermanently";
				public const string TryUpdateValueWithSlidingExpiration = "/api/v1/Cache/TryUpdateValueWithSlidingExpiration";
				public const string TryUpdateValueWithAbsoluteExpiration = "/api/v1/Cache/TryUpdateValueWithAbsoluteExpiration";
				public const string TryUpdateValueWithAbsoluteServerSideExpiration = "/api/v1/Cache/TryUpdateValueWithAbsoluteServerSideExpiration";
				public const string RemoveValue = "/api/v1/Cache/RemoveValue";
			}
		}

		public static class Lock
		{
			public static class V1
			{
				public const string Exists = "/api/v1/Lock/Exists";
				public const string GetMetadata = "/api/v1/Lock/GetMetadata";
				public const string TryAcquireLock = "/api/v1/Lock/TryAcquireLock";
				public const string ReleaseLock = "/api/v1/Lock/ReleaseLock";
				public const string RenewLock = "/api/v1/Lock/RenewLock";
			}
		}
	}
}
