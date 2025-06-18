namespace Legion.ADF.Messaging.Events;

public record ConfigKeyCreatedEvent : Legion.MessageBus.Messages.IEvent
{
	public string Key { get; }

	public ConfigKeyCreatedEvent(string key)
	{
		Throw.IfArgumentNullOrWhiteSpace(key);

		Key = key;
	}
}
