using Legion.Workflows.Internal;
using Legion.Workflows.Nodes;
using Legion.Workflows.Nodes.Internal;

namespace Legion.Workflows;

public class WorkflowEngine
{
	private readonly IWorkflowPersistenceProvider _persistenceProvider;

	public WorkflowEngine(IWorkflowPersistenceProvider? persistenceProvider = null)
	{
		_persistenceProvider = persistenceProvider ?? new InMemoryPersistenceProvider();
	}

	public async Task RunWorkflowAsync(IWorkflow workflow, Guid idWorkflowInstance)
	{
		Throw.IfArgumentNull(workflow);

		workflow.Init();

		IWorkflowContext workflowContext;

		// Načítanie uloženého stavu, ak existuje
		if (_persistenceProvider.WorkflowStateExists(idWorkflowInstance))
		{
			var ctx = await _persistenceProvider.LoadWorkflowStateAsync(idWorkflowInstance, workflow.IdWorkflow);
			if (ctx == null)
				Throw.InvalidOperationException($"Workflow state with ID {idWorkflowInstance} not found.");

			workflowContext = ctx;
		}
		else
		{
			workflowContext = new WorkflowContext(workflow.IdWorkflow, idWorkflowInstance);
			await SaveWorkflowStateAsync(workflowContext, true);
		}

		var exec = new WorkflowExecutor(this, workflow, workflowContext);
		var currentNodeContexts = await _persistenceProvider.GetCurrentNodesAsync(idWorkflowInstance, workflow.IdWorkflow);

		if (0 < currentNodeContexts?.Count)
		{
			//TODO run in parallel
			foreach (var currentNodeContext in currentNodeContexts)
			{
				await exec.ExecuteNodeAsync(currentNodeContext.Item1, currentNodeContext.Item2, restart: false);
			}
		}
		else
		{
			await exec.ExecuteNodeAsync(workflow.StartNode.IdNode, GlobalContext.Instance.NewGuid(), restart: false);
		}
	}

	public async Task SaveWorkflowStateAsync(IWorkflowContext workflowContext, bool withData)
	{
		Throw.IfArgumentNull(workflowContext);

		await _persistenceProvider.SaveWorkflowStateAsync(workflowContext, withData);
	}

	public async Task<INodeContext?> LoadNodeStateAsync(Guid idNodeInstance, string idNode, Guid idWorkflowInstance, string idWorkflow)
	{
		Throw.IfArgumentNullOrWhiteSpace(idNode);
		Throw.IfArgumentNullOrWhiteSpace(idWorkflow);

		return await _persistenceProvider.LoadNodeStateAsync(idNodeInstance, idNode, idWorkflowInstance, idWorkflow);
	}

	public async Task<INodeContext> AddOrGetNodeStateAsync(Guid idNodeInstance, string idNode, Guid idWorkflowInstance, string idWorkflow)
	{
		Throw.IfArgumentNullOrWhiteSpace(idNode);
		Throw.IfArgumentNullOrWhiteSpace(idWorkflow);

		var nodeContext = await _persistenceProvider.LoadNodeStateAsync(idNodeInstance, idNode, idWorkflowInstance, idWorkflow);
		if (nodeContext == null)
		{
			nodeContext = new NodeContext(idNode, idNodeInstance, idWorkflow, idWorkflowInstance);
			await _persistenceProvider.SaveNodeStateAsync(nodeContext, false);
		}

		return nodeContext;
	}

	public async Task SaveNodeStateAsync(INodeContext nodeContext, bool withData)
	{
		Throw.IfArgumentNull(nodeContext);

		await _persistenceProvider.SaveNodeStateAsync(nodeContext, withData);
	}

	public async Task SaveStateAsync(INodeContext nodeContext, IWorkflowContext workflowContext, bool withData)
	{
		Throw.IfArgumentNull(nodeContext);
		Throw.IfArgumentNull(workflowContext);

		await _persistenceProvider.SaveStateAsync(nodeContext, workflowContext, withData);
	}
}

