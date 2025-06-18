namespace Legion.Workflows;

public interface IWorkflowLogger
{
	void SetWorkflow(string idWorkflow, Guid idWorkflowInstance);

	void LogCritical(string message, Exception? exception = null);
	void LogError(string message, Exception? exception = null);
	void LogWarning(string message);
	void LogInformation(string message);
	void LogDebug(string message);
	void LogTrace(string message);
}
