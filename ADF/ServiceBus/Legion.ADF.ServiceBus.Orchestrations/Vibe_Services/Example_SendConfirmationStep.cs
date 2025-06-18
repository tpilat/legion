namespace Legion.ADF.ServiceBus.Orchestrations.Vibe_Services;

public class SendConfirmationStep : IOrchestrationStep
{
	public string Name => "SendConfirmation";

	public async Task<StepResult> ExecuteAsync(OrchestrationState state)
	{
		Console.WriteLine("Sending confirmation email...");
		return new StepResult
		{
			Success = true,
			WaitForEvent = "UserConfirmed"
		};
	}
}

