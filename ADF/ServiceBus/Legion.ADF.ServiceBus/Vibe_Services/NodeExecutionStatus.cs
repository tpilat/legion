namespace Legion.ADF.ServiceBus.Orchestrations.Vibe_Services;

public record NodeExecutionStatus
{
	public string NodeId { get; init; } = default!;
	public NodeStatus Status { get; init; }
	public DateTime TimestampUtc { get; init; } = GlobalContext.Instance.UtcNow;
	public string? Message { get; init; }
}
