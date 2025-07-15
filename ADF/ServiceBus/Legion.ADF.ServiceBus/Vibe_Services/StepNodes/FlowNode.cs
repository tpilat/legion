namespace Legion.ADF.ServiceBus.Orchestrations.Vibe_Services;

public abstract class FlowNode
{
	public string Id { get; set; } = GlobalContext.Instance.NewGuid().ToString();
	public abstract Task ExecuteAsync(OrchestrationState state, OrchestrationEngine engine);
}
