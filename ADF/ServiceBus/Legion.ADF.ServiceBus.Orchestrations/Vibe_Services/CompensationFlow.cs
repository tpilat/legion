namespace Legion.ADF.ServiceBus.Orchestrations.Vibe_Services;

public class CompensationFlow
{
	private readonly Dictionary<string, Func<OrchestrationState, Task>> _compensators = new();

	public void AddCompensator(string stepName, Func<OrchestrationState, Task> handler)
	{
		_compensators[stepName] = handler;
	}

	public Task? CompensateAsync(string stepName, OrchestrationState state)
	{
		return _compensators.TryGetValue(stepName, out var handler)
			? handler(state)
			: Task.CompletedTask;
	}
}
