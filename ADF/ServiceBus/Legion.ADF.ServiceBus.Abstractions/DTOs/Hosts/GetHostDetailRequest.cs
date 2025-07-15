namespace Legion.ADF.ServiceBus.DTOs.Hosts;

public record GetHostDetailRequest
{
	public Guid IdHost { get; set; }
}
