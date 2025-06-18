using Legion.Model;

namespace Legion.ADF.Config.Events;

public record ConfigValueChangedEvent : DomainEventBase
{
	public string Key { get; }

	public ConfigValueChangedEvent(string key)
	{
		Throw.IfArgumentNullOrWhiteSpace(key);

		Key = key;
	}
}
