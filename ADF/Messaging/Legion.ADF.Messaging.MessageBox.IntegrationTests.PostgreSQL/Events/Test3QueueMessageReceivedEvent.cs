using Legion.ADF.Messaging.MessageBox.Events;

namespace Legion.ADF.Messaging.MessageBox.IntegrationTests.Events;
internal record Test3QueueMessageReceivedEvent : MessageReceivedEvent
{
	public Test3QueueMessageReceivedEvent(Model.Message message)
		: base(message)
	{
	}
}
