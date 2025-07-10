namespace Legion.ADF.ServiceBus.Orchestrations.Vibe_Services;

public class ParallelNode : FlowNode
{
	public List<FlowNode> Branches { get; set; } = new();
	public bool WaitAll { get; set; } = true;

	public override async Task ExecuteAsync(OrchestrationState state, OrchestrationEngine engine)
	{
		var tasks = Branches.Select(b => b.ExecuteAsync(state, engine));
		if (WaitAll)
			await Task.WhenAll(tasks);
		else
			await Task.WhenAny(tasks);
	}
}

