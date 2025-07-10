using Legion.Exceptions;

namespace Legion.ADF.Cache.Exceptions.Internal;

public static partial class ErrorCodes
{
	public static partial class ConnectionStringProviderException
	{
		public static IErrorCode InvalidStoreId(string storeId)
			=> new ErrorCode(
				"ADFCache_CONN-STR_0001",
				$"Invalid connection string strore ID = {storeId}");
	}

	public static partial class DBConfigurationManagerException
	{
		public static IErrorCode Write => _write.Value;
		private static readonly Lazy<IErrorCode> _write = new(() =>
			new ErrorCode(
				"ADFCache_WRITE_0000",
				"Exception was thrown."));
	}

	public static partial class CacheUnitOfWorkException
	{
		public static IErrorCode InvalidUoW => _invalidUoW.Value;
		private static readonly Lazy<IErrorCode> _invalidUoW = new(() =>
			new ErrorCode(
				"ADFCache_UoW_0001",
				$"Cannot create UnitOfWork"));
	}

	public static partial class CacheKeyRemoveService
	{
		public static IErrorCode Default => _default.Value;
		private static readonly Lazy<IErrorCode> _default = new(() =>
			new ErrorCode(
				"ADFCache_SVC_0001",
				$"Unhandled exception in CacheKeyRemoveService"));
	}

	public static partial class CacheDataRemoveService
	{
		public static IErrorCode Default => _default.Value;
		private static readonly Lazy<IErrorCode> _default = new(() =>
			new ErrorCode(
				"ADFPersCache_SVC_0000",
				$"Unhandled exception in CacheDataRemoveService"));
	}

	public static partial class CacheDataRepositoryException
	{
		public static IErrorCode CacheDataConcurrentUpdate(string key)
			=> new ErrorCode(
				"ADFPersCache_0000",
				$"Concurrent updates occured on key {key}");
	}

	public static partial class DistributedLockRemoveService
	{
		public static IErrorCode Default => _default.Value;
		private static readonly Lazy<IErrorCode> _default = new(() =>
			new ErrorCode(
				"ADFDistLock_SVC_0000",
				$"Unhandled exception in DistributedLockRemoveService"));
	}
}
