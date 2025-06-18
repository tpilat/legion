namespace Legion.ADF.ServiceBus.Orchestrations.Vibe_Services;

public class IfNode : FlowNode
{
	public Func<OrchestrationState, bool> Condition { get; set; }
	public FlowNode Then { get; set; }
	public FlowNode? Else { get; set; }

	public override async Task ExecuteAsync(OrchestrationState state, OrchestrationEngine engine)
	{
		if (Condition(state))
			await Then.ExecuteAsync(state, engine);
		else if (Else != null)
			await Else.ExecuteAsync(state, engine);
	}
}

