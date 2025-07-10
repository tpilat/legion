using Legion.NetHttp;

namespace Legion.ADF.ServiceBus.RestApi.Client;

public partial class ServiceBusRestApiClient : HttpApiClient<ServiceBusRestApiClientOptions>
{
	public static class URI
	{
		public static class Hosts
		{
			public static class V1
			{
				public const string GetHostsSummary = "/api/v1/Hosts/GetHostsSummary";
				public const string GetHostConfiguration = "/api/v1/Hosts/GetHostConfiguration";
				public const string GetHostLogs = "/api/v1/Hosts/GetHostLogs";
			}
		}

		public static class Jobs
		{
			public static class V1
			{
				public const string GetJobsSummary = "/api/v1/Jobs/GetJobsSummary";
				public const string GetJobsStatistics = "/api/v1/Jobs/GetJobStatistics";
				public const string GetJobExecutions = "/api/v1/Jobs/GetJobExecutions";
				public const string GetJobLogs = "/api/v1/Jobs/GetJobLogs";
			}
		}
	}
}
