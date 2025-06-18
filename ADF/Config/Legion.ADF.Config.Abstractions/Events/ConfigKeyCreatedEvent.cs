using Legion.Model;

namespace Legion.ADF.Config.Events;

public record ConfigKeyCreatedEvent : DomainEventBase
{
	public string Key { get; }

	public ConfigKeyCreatedEvent(string key)
	{
		Throw.IfArgumentNullOrWhiteSpace(key);

		Key = key;
	}
}
