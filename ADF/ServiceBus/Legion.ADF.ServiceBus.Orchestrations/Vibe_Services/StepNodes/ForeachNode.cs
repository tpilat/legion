namespace Legion.ADF.ServiceBus.Orchestrations.Vibe_Services;

public class ForeachNode : FlowNode
{
	public string Collection { get; set; } = default!; // napr. "data.Items"
	public string ItemVar { get; set; } = "item";      // meno premennej v iterácii
	public FlowNode Body { get; set; } = default!;

	public override async Task ExecuteAsync(OrchestrationState state, OrchestrationEngine engine)
	{
		ExecutionTracker.Log(state, this.Id, NodeStatus.Started);

		if (!state.Data.TryGetValue(Collection, out var listObj) || listObj is not IEnumerable<object> list)
			throw new InvalidOperationException($"Collection '{Collection}' is not enumerable.");

		int index = 0;
		foreach (var item in list)
		{
			state.Data[ItemVar] = item;
			state.Data[$"{ItemVar}Index"] = index++;
			await Body.ExecuteAsync(state, engine);

			if (state.WaitingForEvent != null)
				break; // pozastavenie v prípade WaitForEvent vnútri
		}

		ExecutionTracker.Log(state, this.Id, NodeStatus.Succeeded);
	}
}
