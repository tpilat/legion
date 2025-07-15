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

	public static partial class ServiceBusMonitorException
	{
		public static IErrorCode DistributedManagerError => _distributedManagerError.Value;
		private static readonly Lazy<IErrorCode> _distributedManagerError = new(() =>
			new ErrorCode(
				"L_ESB_Mon_0001",
				$"Monitor: DistributedManager error"));
	}

	public static partial class ServiceBusUnitOfWorkException
	{
		public static IErrorCode InvalidUoW => _invalidUoW.Value;
		private static readonly Lazy<IErrorCode> _invalidUoW = new(() =>
			new ErrorCode(
				"L_ESB_UoW_0001",
				$"Cannot create UnitOfWork"));
	}

	public static partial class ServiceBusHostException
	{
		public static IErrorCode UnhandledError(string hostName, Guid? idHost)
			=> new ErrorCode(
				"L_ESB_Host_0000",
				$"Unhandled error in host with name \"{hostName}\" with id {idHost}");

		public static IErrorCode NoHostFound(string hostName)
			=> new ErrorCode(
				"L_ESB_Host_0001",
				$"No host found with name \"{hostName}\"");

		public static IErrorCode InvalidHostConfig(string hostName)
			=> new ErrorCode(
				"L_ESB_Host_0002",
				$"Host \"{hostName}\" has invalid config");
	}

	public static partial class ServiceBusJobException
	{
		public static IErrorCode UnhandledError(string hostName, Guid? idHost, string jobName, Guid idJob)
			=> new ErrorCode(
				"L_ESB_Job_0000",
				$"Unhandled error in job with name \"{jobName}\" with id {idJob} on host with name \"{hostName}\" with id {idHost}");
	}

	public static partial class HostConfigurationException
	{
		public static IErrorCode DuplicatedRetryCount => _duplicatedRetryCount.Value;
		private static readonly Lazy<IErrorCode> _duplicatedRetryCount = new(() =>
			new ErrorCode(
				"L_ESB_Conf_0001",
				$"Duplicated retry count"));
	}
}
