namespace Legion.Workflows.Nodes.Internal;

internal class NodeContext : INodeContext
{
	private INodeLogger? _logger;

	public string IdNode { get; set; }
	public Guid IdNodeInstance { get; set; }
	public string IdWorkflow { get; set; }
	public Guid IdWorkflowInstance { get; set; }
	public NodeStatus NodeStatus { get; set; }
	public object? Result { get; set; }
	public string? NextNodeId { get; set; }
	public string? Message { get; set; }
	public string? WaitForEvent { get; set; }
	public Dictionary<string, object?> Data { get; set; }

	public NodeContext()
	{
		Data = [];
	}

	public NodeContext(string idNode, Guid idNodeInstance, string idWorkflow, Guid idWorkflowInstance)
		: this()
	{
		Throw.IfArgumentNullOrWhiteSpace(idNode);

		IdNode = idNode;
		IdNodeInstance = idNodeInstance;
		IdWorkflow = idWorkflow;
		IdWorkflowInstance = idWorkflowInstance;
		NodeStatus = NodeStatus.NotStarted;
	}

	public bool Start(bool force, bool resetData)
	{
		if (force || NodeStatus == NodeStatus.NotStarted)
		{
			NodeStatus = NodeStatus.InProgress;
			return true;
		}

		if (resetData)
		{
			Data = [];
			Result = null;
			NextNodeId = null;
			Message = null;
			WaitForEvent = null;
		}

		return false;
	}

	public bool Restart()
		=> Start(true, true);

	public NodeResult SetResult(NodeResult nodeResult, object? result, string? idNextNode, string? message, string? waitForEvent, bool force)
	{
		if (nodeResult == NodeResult.Success)
		{
			NodeStatus = NodeStatus.Completed;
		}
		else if (nodeResult == NodeResult.Failure)
		{
			NodeStatus = NodeStatus.Failed;
		}
		else if (nodeResult == NodeResult.Waiting)
		{
			if (string.IsNullOrWhiteSpace(waitForEvent))
				Throw.InvalidOperationException("WaitForEvent cannot be null or empty when nodeResult is Waiting.");

			NodeStatus = NodeStatus.WaitingForEvent;
		}
		else
		{
			NodeStatus = NodeStatus.InProgress; //fallback
		}

		if (force)
		{
			NextNodeId = idNextNode;
			Result = result;
			Message = message;
			WaitForEvent = waitForEvent;
		}
		else
		{
			if (string.IsNullOrWhiteSpace(NextNodeId))
				NextNodeId = idNextNode;

			if (Result == null)
				Result = result;

			if (string.IsNullOrWhiteSpace(Message))
				Message = message;

			if (string.IsNullOrWhiteSpace(WaitForEvent))
				WaitForEvent = waitForEvent;
		}

		return nodeResult;
	}

	public Dictionary<string, object?> GetAllData()
		=> Data.ToDictionary(x => x.Key, x => x.Value);

	public void SetData(Dictionary<string, object?> data)
	{
		Data = data ?? [];
	}

	public void SetVariable(string name, object? value, bool force = true)
	{
		Throw.IfArgumentNullOrWhiteSpace(name);

		if (force)
		{
			Data[name] = value;
		}
		else
		{
			Data.TryAdd(name, value);
		}
	}

	public T GetVariable<T>(string name, T defaultValue = default)
	{
		Throw.IfArgumentNullOrWhiteSpace(name);

		if (Data.TryGetValue(name, out var value) && value is T typedValue)
			return typedValue;

		return defaultValue;
	}

	public bool TryGetVariable<T>(string name, out T value)
	{
		Throw.IfArgumentNullOrWhiteSpace(name);

		if (Data.TryGetValue(name, out var val) && val is T typedValue)
		{
			value = typedValue;
			return true;
		}

		value = default!;
		return false;
	}

	public T AddOrGetVariable<T>(string name, T defaultValue)
	{
		Throw.IfArgumentNullOrWhiteSpace(name);

		if (Data.TryGetValue(name, out var value) && value is T typedValue)
		{
			return typedValue;
		}
		else
		{
			Data[name] = defaultValue;
			return defaultValue;
		}
	}

	private readonly object _lock = new();
	public INodeLogger GetLogger()
	{
		if (_logger == null)
		{
			lock (_lock)
			{
				_logger = new ConsoleNodeLogger();
				_logger.SetWorkflowAndNode(IdWorkflow, IdWorkflowInstance, IdNode, IdNodeInstance);
			}
		}

		return _logger;
	}
}
