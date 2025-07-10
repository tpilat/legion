namespace Legion.ADF.ServiceBus.Orchestrations.Vibe_Services.Steps;

public class CallFlowNode : FlowNode
{
	public string FlowName { get; set; } = default!;
	public FlowNode? ResolvedFlow { get; set; } // Ak načítané pri parsovaní, inak lazy-load v engine

	public override async Task ExecuteAsync(OrchestrationState state, OrchestrationEngine engine)
	{
		Console.WriteLine($"[CallFlow] Invoking subflow: {FlowName}");

		var subflow = ResolvedFlow ?? engine.LoadSubflow(FlowName);
		if (subflow == null)
			throw new InvalidOperationException($"Subflow '{FlowName}' not found.");

		var subEngine = new OrchestrationEngine(subflow, engine.CompensationFlow)
		{
			FlowRegistry = engine.FlowRegistry
		};

		//ExecutionTracker.Log(state, this.Id, NodeStatus.????);

		await subEngine.StartAsync(state); // beží na tom istom stave
	}
}

