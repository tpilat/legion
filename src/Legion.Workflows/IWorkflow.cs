using Legion.Validation;
using Legion.Workflows.Nodes;

namespace Legion.Workflows;

public interface IWorkflow
{
	string IdWorkflow { get; }
	string Name { get; }
	IReadOnlyList<IWorkflowNode> Nodes { get; }
	IWorkflowNode StartNode { get; }
	
	IValidationResult Init();
	
	IWorkflowNode? GetWorkflowNode(string idNode);

	bool TryGetWorkflowNode(string idNode, out IWorkflowNode? workflowNode);

	IValidationResult Validate();
}
