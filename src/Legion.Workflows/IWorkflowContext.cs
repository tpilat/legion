namespace Legion.Workflows;

public interface IWorkflowContext
{
	string IdWorkflow { get; }

	Guid IdWorkflowInstance { get; }

	Dictionary<string, object?> GetAllData();

	void SetData(Dictionary<string, object?> data);

	void SetVariable(string name, object? value, bool force = true);

	T GetVariable<T>(string name, T defaultValue = default);

	bool TryGetVariable<T>(string name, out T value);

	T AddOrGetVariable<T>(string name, T defaultValue);

	IWorkflowLogger GetLogger();
}
