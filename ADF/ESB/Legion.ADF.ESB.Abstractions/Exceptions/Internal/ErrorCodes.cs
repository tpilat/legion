using Legion.Exceptions;

namespace Legion.ADF.ESB.Exceptions.Internal;

public static partial class ErrorCodes
{
	public static partial class AppSettingsException
	{
		public static IErrorCode InvalidInstance(string message)
			=> new ErrorCode(
				"ADFESB_APPSETT_0001",
				message);
	}

	public static partial class ConnectionStringProviderException
	{
		public static IErrorCode InvalidStoreId(string storeId)
			=> new ErrorCode(
				"ADFESB_CONN-STR_0001",
				$"Invalid connection string strore ID = {storeId}");
	}

	public static partial class HttpClientRequestResponseLoggerException
	{
		public static IErrorCode Default(string message)
			=> new ErrorCode(
				"ADFESB_REQRES_LOG_0001",
				message);

		public static IErrorCode InvalidUnitOfWork(string uowName)
			=> new ErrorCode(
				"ADFESB_REQRES_LOG_UOW_0001",
				$"Cannot create UnitOfWork {uowName}");
	}

	public static partial class ESBRegistrationException
	{
		public static IErrorCode MultipleAdapterRegistration(Guid idAdapter, string registeredClass, string newClass)
			=> new ErrorCode(
				"ADFESB_REG_0000",
				$"Cannot register Adapter with ID = {idAdapter} multiple times. | Registered Class = {registeredClass} | New Class = {newClass}");
	}

	public static partial class ESBInitializerException
	{
		public static IErrorCode InvalidInitStatus(string currentStatus)
			=> new ErrorCode(
				"ADFESB_INIT_0000",
				$"Invalid initialization status = {currentStatus}");

		public static IErrorCode CannotSetInitStatus(string currentStatus, string newStatus)
			=> new ErrorCode(
				"ADFESB_INIT_0001",
				$"Cannot set new initialization status = {newStatus} | CurrentStatus = {currentStatus}");

		public static IErrorCode InvalidAdapterClass(Guid idAdapter, string expectedClass, string foundClass)
			=> new ErrorCode(
				"ADFESB_INIT_0002",
				$"Invalid Class for Adapter ID = {idAdapter} | Expected Class = {expectedClass} | Found Class = {foundClass}");

		public static IErrorCode CannotInsertAdapter(Guid idAdapter)
			=> new ErrorCode(
				"ADFESB_INIT_0003",
				$"Cannot insert Adapter with ID = {idAdapter}");

		public static IErrorCode CannotUpdateAdapter(Guid idAdapter)
			=> new ErrorCode(
				"ADFESB_INIT_0004",
				$"Cannot update Adapter with ID = {idAdapter}");
	}
}
