namespace Legion.ADF.Messaging.Inbox.IntegrationTests.Messages;

public record TestMessage
{
	public int MyProperty1 { get; set; }
	public string MyProperty2 { get; set; }

	public TestMessage(string value)
	{
		MyProperty2 = value;
	}
}
