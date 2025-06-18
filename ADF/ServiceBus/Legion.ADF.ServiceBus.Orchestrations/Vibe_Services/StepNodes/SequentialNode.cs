namespace Legion.ADF.ServiceBus.Orchestrations.Vibe_Services;

public class SequentialNode : FlowNode
{
	public List<FlowNode> Children { get; set; } = new();

	public override async Task ExecuteAsync(OrchestrationState state, OrchestrationEngine engine)
	{
		ExecutionTracker.Log(state, this.Id, NodeStatus.Started);

		foreach (var node in Children)
		{
			await node.ExecuteAsync(state, engine);
			if (state.WaitingForEvent != null)
				break; // zastavíme, kým nepríde event
		}

		ExecutionTracker.Log(state, this.Id, NodeStatus.Succeeded);
	}
}

