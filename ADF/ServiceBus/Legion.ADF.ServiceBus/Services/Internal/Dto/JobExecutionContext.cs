namespace Legion.ADF.ServiceBus.Services.Internal.Dto;

internal class JobExecutionContext
{
	public bool Terminate { get; set; }
	public bool? ExecutedSuccessfully { get; set; }
	public DateTime? DelayedToUtc { get; set; }
	public string? ErrorDetail { get; set; }
}
