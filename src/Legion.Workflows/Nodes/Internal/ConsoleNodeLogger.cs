
namespace Legion.Workflows.Nodes.Internal;

internal class ConsoleNodeLogger : INodeLogger
{
	private string _idWorkflow;
	private Guid _idWorkflowInstance;
	private string _idNode;
	private Guid _idNodeInstance;

	private bool _set;
	public void SetWorkflowAndNode(string idWorkflow, Guid idWorkflowInstance, string idNode, Guid idNodeInstance)
	{
		Throw.IfArgumentNullOrWhiteSpace(idWorkflow);
		Throw.IfArgumentNullOrWhiteSpace(idNode);

		if (_set)
			Throw.InvalidOperationException("Workflow and Node are already set.");

		_idWorkflow = idWorkflow;
		_idWorkflowInstance = idWorkflowInstance;
		_idNode = idNode;
		_idNodeInstance = idNodeInstance;
	}

	public void LogCritical(string message, Exception? exception = null)
	{
		Console.WriteLine($"FATAL WF:{_idWorkflow} [{_idWorkflowInstance}] NODE:{_idNode} [{_idNodeInstance}]: {message} | {exception}");
	}

	public void LogError(string message, Exception? exception = null)
	{
		Console.WriteLine($"ERROR WF:{_idWorkflow} [{_idWorkflowInstance}] NODE:{_idNode} [{_idNodeInstance}]: {message} | {exception}");
	}

	public void LogWarning(string message)
	{
		Console.WriteLine($"WARN WF:{_idWorkflow} [{_idWorkflowInstance}] NODE:{_idNode} [{_idNodeInstance}]: {message}");
	}

	public void LogInformation(string message)
	{
		Console.WriteLine($"INFO WF:{_idWorkflow} [{_idWorkflowInstance}] NODE:{_idNode} [{_idNodeInstance}]: {message}");
	}

	public void LogDebug(string message)
	{
		Console.WriteLine($"DEBUG WF:{_idWorkflow} [{_idWorkflowInstance}] NODE:{_idNode} [{_idNodeInstance}]: {message}");
	}

	public void LogTrace(string message)
	{
		Console.WriteLine($"TRACE WF:{_idWorkflow} [{_idWorkflowInstance}] NODE:{_idNode} [{_idNodeInstance}]: {message}");
	}
}
