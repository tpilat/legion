using Legion.ADF.ESB.Orchestrations;
using Microsoft.Extensions.Options;

namespace TestEnterpriseServiceBus.Orchestrations.SocPoist;

public class SocPoistOrchestration : ESBOrchestration<SocPoistOrchestrationConfig>
{
	public SocPoistOrchestration(IOptions<SocPoistOrchestrationConfig> options)
		: base(options)
	{
		IdOrchestration = new Guid("00000001-0000-0000-0000-000000000000");
		Code = "SocPoist";
		Name = "SocPoist";
		Description = "SocPoist";
		IsSingleton = false;
		Properties = SerializeProperties();
		TimeoutForMessageProcessingInSeconds = 60;
		MaxMessageProcessingRetryCount = 50;
		Version = "1.0.0.0";
		ValidTo = null;

		Steps.Add(new SocPoistOrchestration_Start());
	}

	public override SocPoistOrchestration Default()
		=> new(Microsoft.Extensions.Options.Options.Create(new SocPoistOrchestrationConfig()));
}
