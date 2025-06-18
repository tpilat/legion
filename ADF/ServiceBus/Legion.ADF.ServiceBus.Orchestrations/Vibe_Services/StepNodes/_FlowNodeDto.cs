namespace Legion.ADF.ServiceBus.Orchestrations.Vibe_Services.Steps;

public class FlowNodeDto
{
	public string Type { get; set; } = default!;
	public string? StepName { get; set; }
	public string? EventName { get; set; }
	public string? Condition { get; set; }
	public List<FlowNodeDto>? Children { get; set; }
	public FlowNodeDto? Then { get; set; }
	public FlowNodeDto? Else { get; set; }
}
