using Legion.Workflows.Nodes;

namespace Legion.Workflows;

public class WorkflowExecutor
{
	public WorkflowEngine WorkflowEngine { get; set; }
	public IWorkflow Workflow { get; set; }
	public IWorkflowContext WorkflowContext { get; set; }

	internal WorkflowExecutor(WorkflowEngine workflowEngine, IWorkflow workflow, IWorkflowContext workflowContext)
	{
		Throw.IfArgumentNull(workflowEngine);
		Throw.IfArgumentNull(workflow);
		Throw.IfArgumentNull(workflowContext);

		WorkflowEngine = workflowEngine;
		Workflow = workflow;
		WorkflowContext = workflowContext;
	}

	public async Task<NodeResult?> ExecuteNodeAsync(
		string idNode,
		Guid idNodeInstance,
		bool restart,
		Func<NodeResult, Task<NodeResult>>? onExecuted = null)
	{
		Throw.IfArgumentNullOrWhiteSpace(idNode);

		INodeContext nodeContext = null!;

		try
		{
			if (!Workflow.TryGetWorkflowNode(idNode, out var node))
			{
				WorkflowContext.GetLogger().LogError($"Node not found");
				return null;
			}

			nodeContext = await WorkflowEngine.AddOrGetNodeStateAsync(idNodeInstance, idNode, WorkflowContext.IdWorkflowInstance, Workflow.IdWorkflow);

			var started = restart
				? nodeContext.Restart()
				: nodeContext.Start(force: false, resetData: false);

			if (started)
			{
				await WorkflowEngine.SaveNodeStateAsync(nodeContext, withData: true);
			}
			else if (nodeContext.NodeStatus == NodeStatus.WaitingForEvent)
			{
				if (string.IsNullOrWhiteSpace(nodeContext.WaitForEvent))
				{
					nodeContext.GetLogger().LogError($"Node is waiting for event == NULL");
					return null;
				}

				nodeContext.GetLogger().LogInformation($"Node is waiting for event {nodeContext.WaitForEvent}");
				return NodeResult.Waiting;
			}

			nodeContext.GetLogger().LogInformation($"Node executing...");

			var result = await node!.ExecuteAsync(nodeContext, this);

			if (onExecuted != null)
				result = await onExecuted(result);

			nodeContext.GetLogger().LogInformation($"Node executed...");

			await WorkflowEngine.SaveStateAsync(nodeContext, WorkflowContext, withData: true);

			return result;
		}
		catch (Exception ex)
		{
			if (nodeContext != null)
			{
				nodeContext.GetLogger().LogError(ex.ToString());
				nodeContext.SetResult(
					NodeResult.Failure,
					result: null,
					idNextNode: null,
					ex.ToString(),
					waitForEvent: null,
					force: true);

				await WorkflowEngine.SaveNodeStateAsync(nodeContext, withData: false); //save node Data - variables
			}
			else
			{
				WorkflowContext.GetLogger().LogError($"UNHANDLED ERROR: {nameof(idNode)} = {idNode} | {nameof(idNodeInstance)} = {idNodeInstance} | {ex}");
			}
			
			return NodeResult.Failure;
		}
	}
}
