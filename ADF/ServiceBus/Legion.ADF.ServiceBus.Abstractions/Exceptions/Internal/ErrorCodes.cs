using Legion.Exceptions;

namespace Legion.ADF.ServiceBus.Exceptions.Internal;

public static partial class ErrorCodes
{
	public static partial class ConnectionStringProviderException
	{
		public static IErrorCode InvalidStoreId(string storeId)
			=> new ErrorCode(
				"L_ESB_CONN-STR_0001",
				$"Invalid connection string strore ID = {storeId}");
	}

	public static partial class ServiceBusUnitOfWorkException
	{
		public static IErrorCode InvalidUoW => _invalidUoW.Value;
		private static readonly Lazy<IErrorCode> _invalidUoW = new(() =>
			new ErrorCode(
				"L_ESB_UoW_0001",
				$"Cannot create UnitOfWork"));
	}
}
