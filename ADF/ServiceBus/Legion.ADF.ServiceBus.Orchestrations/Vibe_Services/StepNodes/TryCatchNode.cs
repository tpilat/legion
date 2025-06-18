namespace Legion.ADF.ServiceBus.Orchestrations.Vibe_Services;

public class TryCatchNode : FlowNode
{
	public FlowNode Try { get; set; } = default!;
	public FlowNode Catch { get; set; } = default!;

	public override async Task ExecuteAsync(OrchestrationState state, OrchestrationEngine engine)
	{
		try
		{
			ExecutionTracker.Log(state, this.Id, NodeStatus.Started);

			await Try.ExecuteAsync(state, engine);

			ExecutionTracker.Log(state, this.Id, NodeStatus.Started);
		}
		catch (Exception ex)
		{
			ExecutionTracker.Log(state, this.Id, NodeStatus.Failed, ex.Message);

			Console.WriteLine($"[TryCatchNode] Exception caught: {ex.Message}");
			await Catch.ExecuteAsync(state, engine);
		}
	}
}

