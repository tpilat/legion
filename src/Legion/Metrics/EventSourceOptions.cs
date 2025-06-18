using System.Diagnostics.Tracing;

namespace Legion.Metrics;

public class EventSourceOptions
{
	public string EventSourceName { get; set; }
	public int EventCounterIntervalSec { get; set; } = 1;
	public EventLevel EventLevel { get; set; } = EventLevel.LogAlways;
	public EventKeywords EventKeywords { get; set; } = EventKeywords.All;
	public List<string>? AllowedCounters { get; set; }
	public IDictionary<string, Guid>? EventSourceAdapterAllowedCounters { get; set; }
	public IDictionary<string, Func<string, Guid>>? EventSourceAdapterAllowedCounterGetters { get; set; }
	public Func<string, Guid>? EventSourceAdapterIdEventCounterGetter { get; set; }
	public bool AutoEnable { get; set; } = true;

	public EventSourceOptions(string eventSourceName)
	{
		Throw.IfArgumentNullOrWhiteSpace(eventSourceName);

		EventSourceName = eventSourceName;
	}
}
