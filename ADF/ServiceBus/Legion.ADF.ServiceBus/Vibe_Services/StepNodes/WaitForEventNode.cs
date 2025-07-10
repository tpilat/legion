namespace Legion.ADF.ServiceBus.Orchestrations.Vibe_Services;

public class WaitForEventNode : FlowNode
{
	public string EventName { get; set; } = default!;
	public TimeSpan? Timeout { get; set; }
	public string? CorrelationProperty { get; set; }
	public string? CorrelationValue { get; set; }

	public override Task ExecuteAsync(OrchestrationState state, OrchestrationEngine engine)
	{
		Console.WriteLine($"[Orchestration] Waiting for event '{EventName}'...");

		ExecutionTracker.Log(state, this.Id, NodeStatus.Waiting, $"Waiting for {EventName}");

		state.WaitingForEvent = EventName;
		state.CurrentStep = this.Id;

		if (!string.IsNullOrWhiteSpace(CorrelationProperty))
		{
			state.CorrelationProperty = CorrelationProperty;
			state.CorrelationValue = CorrelationValue ?? state.Data.GetValueOrDefault(CorrelationProperty)?.ToString();
		}

		state.WaitTimeout = Timeout;
		OrchestrationStore.Save(state);

		return Task.CompletedTask;
	}
}
