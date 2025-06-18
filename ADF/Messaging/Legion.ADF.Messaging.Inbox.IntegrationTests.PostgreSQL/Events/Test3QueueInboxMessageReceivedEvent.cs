using Legion.ADF.Messaging.Inbox.Events;

namespace Legion.ADF.Messaging.Inbox.IntegrationTests.Events;
internal record Test3QueueInboxMessageReceivedEvent : InboxMessageReceivedEvent
{
	public Test3QueueInboxMessageReceivedEvent(Model.InboxMessage inboxMessage)
		: base(inboxMessage)
	{
	}
}
