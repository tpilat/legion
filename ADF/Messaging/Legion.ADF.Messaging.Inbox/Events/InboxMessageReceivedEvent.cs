namespace Legion.ADF.Messaging.Inbox.Events;

public abstract record InboxMessageReceivedEvent : Legion.MessageBus.Messages.IEvent
{
	public Model.InboxMessage Message { get; }

	public InboxMessageReceivedEvent(Model.InboxMessage message)
	{
		Throw.IfArgumentNull(message);

		Message = message;
	}
}
