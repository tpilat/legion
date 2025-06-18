namespace Legion.ADF.ServiceBus.Orchestrations.Vibe_Services;

public class GoToNode : FlowNode
{
	public string TargetNodeId { get; set; }

	public override async Task ExecuteAsync(OrchestrationState state, OrchestrationEngine engine)
	{
		//Console.WriteLine($"[GoTo] Jumping to {TargetNodeId}");
		//state.CurrentStep = TargetNodeId;
		//OrchestrationStore.Save(state);
		//return Task.CompletedTask;

		await engine.GoToAsync(state, TargetNodeId);
	}
}
