namespace Legion.ADF.Messaging.DTOs;

public class TopicSubscriptionDto
{
	public string SubscriptionName { get; set; }
	public string ReceivedEventNamespace { get; set; }
	public bool IsSequentialFIFO { get; set; }
	public int MessagesBatchCount { get; set; }
	public int? MaxDegreeOfParallelism { get; set; }
	public TimeSpan TimeoutForMessageProcessing { get; set; }
	public int MaxMessageProcessingRetryCount { get; set; }
	public string? Properties { get; set; }
	public Guid IdProcessingMode { get; set; }
	public Guid IdSuspendingMode { get; set; }
	public Guid? IdJob { get; set; }
	public Guid? IdOrchestration { get; set; }

}
