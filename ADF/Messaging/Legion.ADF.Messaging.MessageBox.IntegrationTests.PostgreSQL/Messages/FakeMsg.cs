namespace Legion.ADF.Messaging.MessageBox.IntegrationTests.Messages;

public record FakeMsg
{
	public int MyProperty1 { get; set; }
	public string MyProperty2 { get; set; }

	public FakeMsg(string value)
	{
		MyProperty2 = value;
	}
}
