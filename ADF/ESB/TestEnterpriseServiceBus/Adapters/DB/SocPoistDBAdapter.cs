using Legion;
using Legion.ADF.ESB.Components;
using Legion.ADF.ESB.ServiceBus;
using Legion.DependencyInjection;
using Legion.Model.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using TestEnterpriseServiceBus.Adapters.DB.Messages;

namespace TestEnterpriseServiceBus.Adapters.DB;

public class SocPoistDBAdapter : AdapterBase<SocPoistDBAdapterConfig>, IESBAdapter, IServiceCollectionBuilder,
	IMessageSubscriber<SocPoistDBRequest>
{
	public static readonly Guid ADAPTER_ID = new("00000003-0000-0000-0000-000000000000");

	public SocPoistDBAdapter(IOptions<SocPoistDBAdapterConfig> options)
		: base(options)
	{
		IdAdapter = ADAPTER_ID;
		Code = "SocPoistDB";
		Name = "SocPoistDB";
		Description = "SocPoistDB";
		IdAdapterStatus = Legion.ADF.ESB.Components.Model.AdapterStatus.Offline;
		Properties = SerializeProperties();
		IsInbound = false;
		IsOutbound = true;
	}

	public override SocPoistDBAdapter Default()
		=> new(Microsoft.Extensions.Options.Options.Create(new SocPoistDBAdapterConfig()));

	public async Task<IResult> HandleAsync(
		IInvocationContext invocationContext,
		SocPoistDBRequest message,
		IUnitOfWorkProvider unitOfWorkProvider,
		CancellationToken cancellationToken = default)
	{
		invocationContext = invocationContext.InvocationCreateNew();
		//.InvocationAddContextProperty(nameof(message.IdMessage), message.IdMessage.ToString());

		var result = new ResultBuilder();

		return result.Build();
	}

	public static IServiceCollection ConfigureServiceCollection(IServiceCollection services, IConfiguration configuration)
	{
		Throw.IfArgumentNull(services);
		Throw.IfArgumentNull(configuration);

		ESBModelRegister.RegisterAdapter<SocPoistDBAdapter>(services, ADAPTER_ID);

		return services;
	}
}
