using System.Collections.Concurrent;
using System.Diagnostics.Tracing;

namespace Legion.Metrics;

#if NET6_0_OR_GREATER
[Legion.Serializer.JsonPolymorphicConverter]
#endif
public interface IEventListener
{
	public string EventSourceName { get; }
	int EventCounterIntervalSec { get; }
	EventLevel EventLevel { get; }
	EventKeywords EventKeywords { get; }
	List<string>? AllowedCounters { get; }
	bool Enabled { get; }

	Dictionary<string, Func<double?>> ActualValues { get; }

	ConcurrentDictionary<string, bool> UnhandledPayloads { get; }

	void AddOnUpdateEvent(Action<EventCounterData>? onUpdate);

	void Enable();
	void DisableWrite();

	void Update(IDictionary<string, object> payload);
}
