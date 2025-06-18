using Legion.Reflection.ObjectPaths;
using Legion.Validation;

namespace TestEnterpriseServiceBus.Orchestrations.SocPoist;

public class SocPoistOrchestrationConfig
{
	public string Test { get; set; }

	public class Validator : ValidatorBase<SocPoistOrchestrationConfig>
	{
		public Validator() { }
		public Validator(IObjectPath objectPath) : base(objectPath) { }

		public override void SetDefaultRuels(ValidatorBuilder<SocPoistOrchestrationConfig> builder)
			=> RulesBuilder(builder);


		public static void RulesBuilder(ValidatorBuilder<SocPoistOrchestrationConfig> builder)
		{
		}
	}
}
