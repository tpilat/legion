using Legion.NetHttp;

namespace Legion.ADF.ServiceBus.RestApi.Client;

public class ServiceBusRestApiClientOptions : HttpApiClientOptions
{
	public string ApiKey { get; set; }
}
