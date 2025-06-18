namespace Legion.ADF.ServiceBus.Orchestrations.Vibe_Services;

public class CompensateNode : FlowNode
{
	public List<string> StepNamesToCompensate { get; set; } = new();

	public override async Task ExecuteAsync(OrchestrationState state, OrchestrationEngine engine)
	{
		ExecutionTracker.Log(state, this.Id, NodeStatus.Compensated);

		state.IsInCompensation = true;

		for (int i = StepNamesToCompensate.Count - 1; i >= 0; i--)
		{
			var stepName = StepNamesToCompensate[i];
			Console.WriteLine($"[CompensateNode] Compensating '{stepName}'...");

			await engine.CompensationFlow.CompensateAsync(stepName, state);
		}
	}
}
