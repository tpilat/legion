using Legion.Workflows.Nodes;

namespace Legion.Workflows;

public interface IWorkflowPersistenceProvider
{
	Task<IWorkflowContext?> LoadWorkflowStateAsync(Guid idWorkflowInstance, string idWorkflow);
	Task SaveWorkflowStateAsync(IWorkflowContext context, bool withData);
	bool WorkflowStateExists(Guid idWorkflowInstance);
	Task<List<(string, Guid)>> GetCurrentNodesAsync(Guid idWorkflowInstance, string idWorkflow);
	Task<INodeContext?> LoadNodeStateAsync(Guid idNodeInstance, string idNode, Guid idWorkflowInstance, string idWorkflow);
	Task SaveNodeStateAsync(INodeContext context, bool withData);
	Task SaveStateAsync(INodeContext nodeContext, IWorkflowContext workflowContext, bool withData);
	bool NodeStateExists(Guid idNodeInstance);
}
