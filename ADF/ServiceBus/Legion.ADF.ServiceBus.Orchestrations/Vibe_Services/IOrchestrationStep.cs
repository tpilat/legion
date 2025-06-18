namespace Legion.ADF.ServiceBus.Orchestrations.Vibe_Services;

public interface IOrchestrationStep
{
	string Name { get; }

	Task<StepResult> ExecuteAsync(OrchestrationState state);
	//Task CompensateAsync(OrchestrationState state);
}
