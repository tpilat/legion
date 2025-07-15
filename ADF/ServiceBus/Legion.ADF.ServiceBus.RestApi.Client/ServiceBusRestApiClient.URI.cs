using Legion.NetHttp;

namespace Legion.ADF.ServiceBus.RestApi.Client;

public partial class ServiceBusRestApiClient : HttpApiClient<ServiceBusRestApiClientOptions>, IServiceBusMonitor
{
	public static class URI
	{
		public static class ServiceBus
		{
			public static class V1
			{
				public const string IsAlive = "/api/v1/ServiceBus/IsAlive";
				public const string GetInstances = "/api/v1/ServiceBus/GetInstances";
			}
		}


		public static class Host
		{
			public static class V1
			{
				public const string GetDetail = "/api/v1/Host/GetDetail";
				public const string GetLogs = "/api/v1/Host/GetLogs";
			}
		}

		public static class Job
		{
			public static class V1
			{
				public const string GetDetail = "/api/v1/Job/GetDetail";
				public const string GetStatistics = "/api/v1/Job/GetStatistics";
				public const string GetExecutions = "/api/v1/Job/GetExecutions";
				public const string GetLogs = "/api/v1/Job/GetLogs";
			}
		}
	}
}
