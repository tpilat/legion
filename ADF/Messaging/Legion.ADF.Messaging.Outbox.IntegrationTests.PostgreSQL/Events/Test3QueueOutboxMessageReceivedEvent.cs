using Legion.ADF.Messaging.Outbox.Events;

namespace Legion.ADF.Messaging.Outbox.IntegrationTests.Events;
internal record Test3QueueOutboxMessageReceivedEvent : OutboxMessageReceivedEvent
{
	public Test3QueueOutboxMessageReceivedEvent(Model.OutboxMessage outboxMessage)
		: base(outboxMessage)
	{
	}
}
