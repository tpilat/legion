namespace Legion.Workflows.Internal;

internal class WorkflowContext : IWorkflowContext
{
	private IWorkflowLogger? _logger;

	public string IdWorkflow { get; set; }
	public Guid IdWorkflowInstance { get; set; }
	public Dictionary<string, object?> Data { get; set; }

	public WorkflowContext()
	{
		Data = [];
	}

	public WorkflowContext(string idWorkflow, Guid idWorkflowInstance)
		: this()
	{
		Throw.IfArgumentNullOrWhiteSpace(idWorkflow);

		IdWorkflow = idWorkflow;
		IdWorkflowInstance = idWorkflowInstance;
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
	public IWorkflowLogger GetLogger()
	{
		if (_logger == null)
		{
			lock (_lock)
			{
				_logger = new ConsoleWorkflowLogger();
				_logger.SetWorkflow(IdWorkflow, IdWorkflowInstance);
			}
		}

		return _logger;
	}
}
