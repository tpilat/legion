using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Legion.NetHttp;

public abstract class HttpApiClient<TOPtions> : HttpApiClient
where TOPtions : HttpApiClientOptions
{
	protected new TOPtions Options => (TOPtions)base.Options;

	public HttpApiClient(
		HttpClient client,
		IServiceProvider serviceProvider,
		IOptions<TOPtions> options,
		ILogger<HttpApiClient> logger)
		: base(client, serviceProvider, options.Value, logger)
	{
	}

	public HttpApiClient(
		HttpClient client,
		IServiceProvider serviceProvider,
		TOPtions options,
		ILogger<HttpApiClient> logger)
		: base(client, serviceProvider, options, logger)
	{
	}
}
