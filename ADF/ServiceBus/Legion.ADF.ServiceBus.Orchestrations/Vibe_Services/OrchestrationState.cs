namespace Legion.ADF.ServiceBus.Orchestrations.Vibe_Services;

public class OrchestrationState
{
	public Guid CorrelationId { get; set; }
	public string CurrentStep { get; set; } = default!;
	public string? WaitingForEvent { get; set; }
	public Dictionary<string, object> Data { get; set; } = new();
	public List<string> ExecutedSteps { get; set; } = new();
	public bool IsInCompensation { get; set; } = false;

	public string? CorrelationProperty { get; set; } // Event correlation support
	public string? CorrelationValue { get; set; }    // Derived or explicit
	public TimeSpan? WaitTimeout { get; set; }       // Optional timeout for waiting

	public List<NodeExecutionStatus> ExecutionLog { get; set; } = new();

}
