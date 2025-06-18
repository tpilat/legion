namespace Legion.ADF.Messaging.Outbox.Events;

public abstract record OutboxMessageReceivedEvent : Legion.MessageBus.Messages.IEvent
{
	public Model.OutboxMessage Message { get; }

	public OutboxMessageReceivedEvent(Model.OutboxMessage message)
	{
		Throw.IfArgumentNull(message);

		Message = message;
	}
}
