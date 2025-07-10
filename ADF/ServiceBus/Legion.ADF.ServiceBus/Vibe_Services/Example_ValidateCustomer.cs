namespace Legion.ADF.ServiceBus.Orchestrations.Vibe_Services;

public class ValidateCustomerStep : IOrchestrationStep
{
	public string Name => "ValidateCustomer";

	public async Task<StepResult> ExecuteAsync(OrchestrationState state)
	{
		Console.WriteLine("Validating customer...");
		return new StepResult
		{
			Success = true,
			Output = new { Valid = true }
		};
	}
}

