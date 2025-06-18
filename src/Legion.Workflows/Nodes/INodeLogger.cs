namespace Legion.Workflows.Nodes;

public interface INodeLogger
{
	void SetWorkflowAndNode(string idWorkflow, Guid idWorkflowInstance, string idNode, Guid idNodeInstance);

	void LogCritical(string message, Exception? exception = null);
	void LogError(string message, Exception? exception = null);
	void LogWarning(string message);
	void LogInformation(string message);
	void LogDebug(string message);
	void LogTrace(string message);
}
