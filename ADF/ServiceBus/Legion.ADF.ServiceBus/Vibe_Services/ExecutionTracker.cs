namespace Legion.ADF.ServiceBus.Orchestrations.Vibe_Services;

public static class ExecutionTracker
{
	public static void Log(OrchestrationState state, string nodeId, NodeStatus status, string? message = null)
	{
		state.ExecutionLog.Add(new NodeExecutionStatus
		{
			NodeId = nodeId,
			Status = status,
			Message = message
		});
	}
}
