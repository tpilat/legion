using Legion.Workflows.Nodes;

namespace Legion.Workflows;

public class WorkflowScriptingGlobals
{
	public IWorkflowContext WFCtx { get; }
	public INodeContext NodeCtx { get; }

	public WorkflowScriptingGlobals(IWorkflowContext workflowContext, INodeContext nodeContext)
	{
		Throw.IfArgumentNull(workflowContext);
		Throw.IfArgumentNull(nodeContext);

		WFCtx = workflowContext;
		NodeCtx = nodeContext;
	}
}
