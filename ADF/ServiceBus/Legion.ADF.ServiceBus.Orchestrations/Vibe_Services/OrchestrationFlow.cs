namespace Legion.ADF.ServiceBus.Orchestrations.Vibe_Services;

public class OrchestrationFlow
{
	private readonly Dictionary<string, Func<StepResult, string?>> _transitions = new();

	public void AddStep(string currentStep, Func<StepResult, string?> nextStepResolver)
	{
		_transitions[currentStep] = nextStepResolver;
	}

	public string? GetNextStep(string currentStep, StepResult result)
	{
		return _transitions.TryGetValue(currentStep, out var resolver)
			? resolver(result)
			: null;
	}
}
