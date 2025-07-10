namespace Legion.ADF.ServiceBus.Orchestrations.Vibe_Services;

public abstract class FlowNode
{
	public string Id { get; set; } = Guid.NewGuid().ToString();
	public abstract Task ExecuteAsync(OrchestrationState state, OrchestrationEngine engine);
}
