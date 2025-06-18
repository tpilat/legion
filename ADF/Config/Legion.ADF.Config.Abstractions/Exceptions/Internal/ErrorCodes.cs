using Legion.Exceptions;

namespace Legion.ADF.Config.Exceptions.Internal;

public static partial class ErrorCodes
{
	public static partial class ConnectionStringProviderException
	{
		public static IErrorCode InvalidStoreId(string storeId)
			=> new ErrorCode(
				"ADFCFG_CONN-STR_0001",
				$"Invalid connection string strore ID = {storeId}");
	}

	public static partial class DBConfigurationManagerException
	{
		public static IErrorCode Write => _write.Value;
		private static readonly Lazy<IErrorCode> _write = new(() =>
			new ErrorCode(
				"ADFCFG_WRITE_0000",
				"Exception was thrown."));
	}

	public static partial class ConfigUnitOfWorkException
	{
		public static IErrorCode InvalidUoW => _invalidUoW.Value;
		private static readonly Lazy<IErrorCode> _invalidUoW = new(() =>
			new ErrorCode(
				"ADFCFG_UoW_0001",
				$"Cannot create UnitOfWork"));
	}
}
