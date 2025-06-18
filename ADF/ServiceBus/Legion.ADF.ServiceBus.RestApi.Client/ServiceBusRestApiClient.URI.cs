using Legion.NetHttp;

namespace Legion.ADF.ServiceBus.RestApi.Client;

public partial class ServiceBusRestApiClient : HttpApiClient<ServiceBusRestApiClientOptions>
{
	public static class URI
	{
		public const string GetJobsSummary = "/api/GetJobsSummary";
	}
}
