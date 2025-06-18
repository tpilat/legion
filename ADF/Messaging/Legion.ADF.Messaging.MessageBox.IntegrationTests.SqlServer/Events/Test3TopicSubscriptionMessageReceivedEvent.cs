using Legion.ADF.Messaging.MessageBox.Events;

namespace Legion.ADF.Messaging.MessageBox.IntegrationTests.Events;
internal record Test3TopicSubscriptionMessageReceivedEvent : MessageReceivedEvent
{
	public Test3TopicSubscriptionMessageReceivedEvent(Model.Message message)
		: base(message)
	{
	}
}
