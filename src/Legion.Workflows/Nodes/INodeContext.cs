namespace Legion.Workflows.Nodes;

public interface INodeContext
{
	string IdNode { get; }

	Guid IdNodeInstance { get; }

	string IdWorkflow { get; }

	Guid IdWorkflowInstance { get; }

	NodeStatus NodeStatus { get; }

	object? Result { get; }

	string? NextNodeId { get; }

	string? Message { get; }

	string? WaitForEvent { get; }

	Dictionary<string, object?> GetAllData();

	void SetData(Dictionary<string, object?> data);

	void SetVariable(string name, object? value, bool force = true);

	T GetVariable<T>(string name, T defaultValue = default);

	bool TryGetVariable<T>(string name, out T value);

	T AddOrGetVariable<T>(string name, T defaultValue);

	bool Start(bool force, bool resetData);

	bool Restart();

	NodeResult SetResult(NodeResult nodeResult, object? result, string? idNextNode, string? message, string? waitForEvent, bool force);

	INodeLogger GetLogger();
}
