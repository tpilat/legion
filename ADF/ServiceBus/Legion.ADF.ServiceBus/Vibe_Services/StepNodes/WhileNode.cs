namespace Legion.ADF.ServiceBus.Orchestrations.Vibe_Services;

public class WhileNode : FlowNode
{
	public Func<OrchestrationState, bool> Condition { get; set; }
	public FlowNode Body { get; set; }

	public override async Task ExecuteAsync(OrchestrationState state, OrchestrationEngine engine)
	{
		ExecutionTracker.Log(state, this.Id, NodeStatus.Started);

		while (Condition(state))
		{
			await Body.ExecuteAsync(state, engine);
		}

		ExecutionTracker.Log(state, this.Id, NodeStatus.Succeeded);
	}
}

