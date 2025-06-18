namespace Legion.ADF.ServiceBus.Orchestrations.Vibe_Services;

public static class OrchestrationStore
{
	private static Dictionary<Guid, OrchestrationState> _store = new();

	public static void Save(OrchestrationState state)
		=> _store[state.CorrelationId] = state;

	public static OrchestrationState? Load(Guid correlationId)
		=> _store.TryGetValue(correlationId, out var state) ? state : null;
}
