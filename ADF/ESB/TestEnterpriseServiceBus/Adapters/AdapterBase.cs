using Legion;
using Legion.ADF.ESB.Components;
using Legion.ADF.ESB.ServiceBus.Initializers;
using Microsoft.Extensions.Options;

namespace TestEnterpriseServiceBus.Adapters;

public abstract class AdapterBase<TOptions> : ESBAdapter<TOptions>, IESBAdapter
	where TOptions : class, IESBAdapterConfig
{
	protected AdapterBase(IOptions<TOptions> options)
		: base(options)
	{
	}

	public override IResult MergeProperties(IScopeContext scopeContext, string? savedProperties)
	{
		var currentInitStatus = ESBInitializer.ConfigsInitializationStatus;
		if (currentInitStatus != ESBInitializationStatus.Started)
			return new ResultBuilder()
				.WithInvalidOperationException(scopeContext, Legion.ADF.ESB.Exceptions.Internal.ErrorCodes.ESBInitializerException.InvalidInitStatus(currentInitStatus.ToString()));

		var resut = Options.Merge(scopeContext.CreateNew(), savedProperties);
		if (!resut.HasError)
			Properties = SerializeProperties();

		return resut;
	}
}
