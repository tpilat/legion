using Legion;
using Legion.ADF.ESB.Components;
using Legion.ADF.ESB.ServiceBus;
using Legion.DependencyInjection;
using Legion.Model.Repositories;
using Legion.NetHttp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TestEnterpriseServiceBus.Adapters.RPO.Http;
using TestEnterpriseServiceBus.Adapters.RPO.Messages;

namespace TestEnterpriseServiceBus.Adapters.RPO;

public class RPOClientAdapter : AdapterBase<RPOClientAdapterConfig>, IESBAdapter, IServiceCollectionBuilder,
	IMessageSubscriber<RPORequest, RPOResponse>
{
	private readonly RPOHttpClient _rpoHttpClient;
	public static readonly Guid ADAPTER_ID = new("00000002-0000-0000-0000-000000000000");

	public RPOClientAdapter(IOptionsSnapshot<RPOClientAdapterConfig> options, IServiceProvider serviceProvider, IHttpClientFactory httpClientFactory)
		: base(options)
	{
		Throw.IfArgumentNull(serviceProvider);
		Throw.IfArgumentNull(httpClientFactory);

		var client = httpClientFactory.CreateClient(nameof(RPOHttpClient));
		var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
		_rpoHttpClient = new RPOHttpClient(client, serviceProvider, options.Value.RPOHttpClientOptions, loggerFactory.CreateLogger<HttpApiClient>());

		IdAdapter = ADAPTER_ID;
		Code = "RPOClient";
		Name = "RPOClient";
		Description = "RPOClient";
		IdAdapterStatus = Legion.ADF.ESB.Components.Model.AdapterStatus.Offline;
		Properties = SerializeProperties();
		IsInbound = false;
		IsOutbound = true;
	}

	public override RPOClientAdapter Default()
		=> throw new Legion.Exceptions.NotImplementedException(); //new(Microsoft.Extensions.Options.Options.Create(new RPOClientAdapterConfig()), new RPOHttpClient());

	public async Task<IResult<RPOResponse>> HandleAsync(
		IInvocationContext invocationContext,
		RPORequest message,
		IUnitOfWorkProvider unitOfWorkProvider,
		CancellationToken cancellationToken = default)
	{
		invocationContext = invocationContext.InvocationCreateNew();
		//.InvocationAddContextProperty(nameof(message.IdMessage), message.IdMessage.ToString());

		var result = new ResultBuilder<RPOResponse>();

		var res= await _rpoHttpClient.SearchByBusinessIdAsync(invocationContext, IdAdapter, "35975946", cancellationToken);

		result.WithData(new RPOResponse());

		return result.Build();
	}

	public static IServiceCollection ConfigureServiceCollection(IServiceCollection services, IConfiguration configuration)
	{
		Throw.IfArgumentNull(services);
		Throw.IfArgumentNull(configuration);

		ESBModelRegister.RegisterAdapter<RPOClientAdapter>(services, ADAPTER_ID);

		return services;
	}
}
