using Legion.Model;

namespace Legion.ADF.Config.Events;

public record ConfigKeyRemovedEvent : DomainEventBase
{
	public string Key { get; }

	public ConfigKeyRemovedEvent(string key)
	{
		Throw.IfArgumentNullOrWhiteSpace(key);

		Key = key;
	}
}
