using Legion.Validation;

namespace Legion.Workflows.Nodes;

public interface IWorkflowNode
{
	string IdNode { get; set; }

	Task<NodeResult> ExecuteAsync(INodeContext nodeContext, WorkflowExecutor exec);
	
	IValidationResult Validate();
}
