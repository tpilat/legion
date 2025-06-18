using Legion.Workflows.Nodes;
using System.Collections.Concurrent;

namespace Legion.Workflows.Internal;

internal class InMemoryPersistenceProvider : IWorkflowPersistenceProvider
{
	private readonly ConcurrentDictionary<Guid, IWorkflowContext> _workflowContexts;
	private readonly ConcurrentDictionary<Guid, INodeContext> _nodeContexts;

	internal InMemoryPersistenceProvider()
	{
		_workflowContexts = [];
		_nodeContexts = [];
	}

	public bool WorkflowStateExists(Guid idWorkflowInstance)
	{
		return _workflowContexts.ContainsKey(idWorkflowInstance);
	}

	public Task<List<(string, Guid)>> GetCurrentNodesAsync(Guid idWorkflowInstance, string idWorkflow)
	{
		return null;
	}

	public Task<IWorkflowContext?> LoadWorkflowStateAsync(Guid idWorkflowInstance, string idWorkflow)
	{
		if (_workflowContexts.TryGetValue(idWorkflowInstance, out var context))
		{
			if (context.IdWorkflow != idWorkflow)
				Throw.InvalidOperationException($"Workflow ID mismatch: expected {idWorkflow}, but found {context.IdWorkflow} for instance {idWorkflowInstance}.");

			return Task.FromResult<IWorkflowContext?>(context);
		}

		return Task.FromResult<IWorkflowContext?>(null);
	}

	public Task SaveWorkflowStateAsync(IWorkflowContext context, bool withData)
	{
		Throw.IfArgumentNull(context);

		var newContext = context;

		if (!withData)
		{
			if (_workflowContexts.TryGetValue(context.IdWorkflowInstance, out var existingContext))
			{
				newContext.SetData(existingContext.GetAllData());
			}
		}

		_workflowContexts.AddOrUpdate(context.IdWorkflowInstance, newContext, (k, v) => newContext);
		return Task.CompletedTask;
	}

	public bool NodeStateExists(Guid idNodeInstance)
	{
		return _nodeContexts.ContainsKey(idNodeInstance);
	}

	public Task<INodeContext?> LoadNodeStateAsync(Guid idNodeInstance, string idNode, Guid idWorkflowInstance, string idWorkflow)
	{
		if (_nodeContexts.TryGetValue(idNodeInstance, out var context))
		{
			if (context.IdNode != idNode)
				Throw.InvalidOperationException($"Node ID mismatch: expected {idNode}, but found {context.IdNode} for instance {idNodeInstance}.");

			if (context.IdWorkflow != idWorkflow)
				Throw.InvalidOperationException($"Workflow ID mismatch: expected {idWorkflow}, but found {context.IdWorkflow} for instance {idWorkflowInstance}.");

			return Task.FromResult<INodeContext?>(context);
		}

		return Task.FromResult<INodeContext?>(null);
	}

	public Task SaveNodeStateAsync(INodeContext context, bool withData)
	{
		var newContext = context;

		if (!withData)
		{
			if (_nodeContexts.TryGetValue(context.IdNodeInstance, out var existingContext))
			{
				newContext.SetData(existingContext.GetAllData());
			}
		}

		_nodeContexts.AddOrUpdate(context.IdWorkflowInstance, newContext, (k, v) => newContext);
		return Task.CompletedTask;
	}

	public Task SaveStateAsync(INodeContext nodeContext, IWorkflowContext workflowContext, bool withData)
	{
		var newNodeContext = nodeContext;

		if (!withData)
		{
			if (_nodeContexts.TryGetValue(newNodeContext.IdNodeInstance, out var existingContext))
			{
				newNodeContext.SetData(existingContext.GetAllData());
			}
		}

		_nodeContexts.AddOrUpdate(nodeContext.IdWorkflowInstance, newNodeContext, (k, v) => newNodeContext);

		var newWorkflowContext = workflowContext;

		if (!withData)
		{
			if (_workflowContexts.TryGetValue(newWorkflowContext.IdWorkflowInstance, out var existingContext))
			{
				newWorkflowContext.SetData(existingContext.GetAllData());
			}
		}

		_workflowContexts.AddOrUpdate(workflowContext.IdWorkflowInstance, newWorkflowContext, (k, v) => newWorkflowContext);
		return Task.CompletedTask;
	}
}

