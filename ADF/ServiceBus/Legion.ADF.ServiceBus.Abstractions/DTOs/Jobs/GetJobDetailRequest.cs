namespace Legion.ADF.ServiceBus.DTOs.Jobs;

public record GetJobDetailRequest
{
	public Guid IdJob { get; set; }
}
