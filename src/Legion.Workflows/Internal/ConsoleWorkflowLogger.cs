
namespace Legion.Workflows.Internal;

internal class ConsoleWorkflowLogger : IWorkflowLogger
{
	private string _idWorkflow;
	private Guid _idWorkflowInstance;

	private bool _set;
	public void SetWorkflow(string idWorkflow, Guid idWorkflowInstance)
	{
		Throw.IfArgumentNullOrWhiteSpace(idWorkflow);

		if (_set)
			Throw.InvalidOperationException("Workflow is already set.");

		_idWorkflow = idWorkflow;
		_idWorkflowInstance = idWorkflowInstance;
	}

	public void LogCritical(string message, Exception? exception = null)
	{
		Console.WriteLine($"FATAL WF:{_idWorkflow} [{_idWorkflowInstance}]: {message} | {exception}");
	}

	public void LogError(string message, Exception? exception = null)
	{
		Console.WriteLine($"ERROR WF:{_idWorkflow} [{_idWorkflowInstance}]: {message} | {exception}");
	}

	public void LogWarning(string message)
	{
		Console.WriteLine($"WARN WF:{_idWorkflow} [{_idWorkflowInstance}]: {message}");
	}

	public void LogInformation(string message)
	{
		Console.WriteLine($"INFO WF:{_idWorkflow} [{_idWorkflowInstance}]: {message}");
	}

	public void LogDebug(string message)
	{
		Console.WriteLine($"DEBUG WF:{_idWorkflow} [{_idWorkflowInstance}]: {message}");
	}

	public void LogTrace(string message)
	{
		Console.WriteLine($"TRACE WF:{_idWorkflow} [{_idWorkflowInstance}]: {message}");
	}
}
