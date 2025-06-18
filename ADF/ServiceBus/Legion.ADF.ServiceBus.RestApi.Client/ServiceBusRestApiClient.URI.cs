using Legion.NetHttp;

namespace Legion.ADF.ServiceBus.RestApi.Client;

public partial class ServiceBusRestApiClient : HttpApiClient<ServiceBusRestApiClientOptions>
{
	public static class URI
	{
		public const string GetJobsSummary = "/api/GetJobsSummary";
		public const string GetJobsStatistics = "/api/GetJobStatistics";
		public const string GetJobExecutions = "/api/GetJobExecutions";
		public const string GetJobLogs = "/api/GetJobLogs";
	}
}
