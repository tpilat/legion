namespace Legion.ADF.Messaging.Inbox.IntegrationTests.Messages;

public record TestMessageWithNoHandler
{
	public int MyProperty1 { get; set; }
	public string MyProperty2 { get; set; }

	public TestMessageWithNoHandler(string value)
	{
		MyProperty2 = value;
	}
}
