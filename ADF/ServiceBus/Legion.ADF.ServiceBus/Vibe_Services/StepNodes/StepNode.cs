namespace Legion.ADF.ServiceBus.Orchestrations.Vibe_Services;

public class StepNode : FlowNode
{
	public string StepName { get; set; } = default!;
	public string? CustomType { get; set; } // Zodpovedá atribútu type="" v XML

	public override async Task ExecuteAsync(OrchestrationState state, OrchestrationEngine engine)
	{
		IOrchestrationStep step;

		if (!string.IsNullOrWhiteSpace(CustomType))
		{
			var type = Type.GetType(CustomType!) ??
				throw new InvalidOperationException($"Custom step type '{CustomType}' not found.");

			step = (IOrchestrationStep)(Activator.CreateInstance(type) ??
				throw new InvalidOperationException($"Could not create instance of '{CustomType}'."));
		}
		else
		{
			step = StepRegistry.Resolve(StepName) ??
				throw new InvalidOperationException($"Step '{StepName}' not found in registry.");
		}

		ExecutionTracker.Log(state, this.Id, NodeStatus.Started);

		var result = await step.ExecuteAsync(state);
		state.Data[StepName] = result.Output;

		ExecutionTracker.Log(state, this.Id, NodeStatus.Succeeded);
	}
}
