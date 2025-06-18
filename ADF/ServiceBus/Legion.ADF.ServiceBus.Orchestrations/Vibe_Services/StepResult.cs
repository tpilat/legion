namespace Legion.ADF.ServiceBus.Orchestrations.Vibe_Services;

public class StepResult
{
	public bool Success { get; set; }
	public object? Output { get; set; }

	// Ak je krok "asynchrónny", definuje, na aký event čaká
	public string? WaitForEvent { get; set; }
}

