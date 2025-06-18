namespace Legion.ADF.Messaging.Outbox.IntegrationTests.Messages;

public record FailureTestMessage
{
	public int MyProperty1 { get; set; }
	public string MyProperty2 { get; set; }

	public FailureTestMessage(string value)
	{
		MyProperty2 = value;
	}
}
