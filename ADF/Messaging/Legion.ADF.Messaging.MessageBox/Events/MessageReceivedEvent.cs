namespace Legion.ADF.Messaging.MessageBox.Events;

public abstract record MessageReceivedEvent : Legion.MessageBus.Messages.IEvent
{
	public Model.Message Message { get; }

	public MessageReceivedEvent(Model.Message message)
	{
		Throw.IfArgumentNull(message);

		Message = message;
	}
}
