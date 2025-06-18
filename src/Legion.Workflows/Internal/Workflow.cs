using Legion.Exceptions;
using Legion.Serializer;
using Legion.Validation;
using Legion.Validation.Results;
using Legion.Workflows.Nodes;

namespace Legion.Workflows.Internal;

internal class Workflow : IWorkflow
{
	private readonly Dictionary<string, IWorkflowNode> _nodesDict;

	internal List<IWorkflowNode> Nodes { get; set; }

	public string IdWorkflow { get; set; }
	public string Name { get; set; }
	public IWorkflowNode StartNode => Nodes?.FirstOrDefault()!;

	IReadOnlyList<IWorkflowNode> IWorkflow.Nodes => Nodes;

	internal Workflow()
	{
		_nodesDict = [];
		Nodes = [];
	}

	internal Workflow(string idWorkflow, string name)
		: this()
	{
		Throw.IfArgumentNullOrWhiteSpace(idWorkflow);
		Throw.IfArgumentNullOrWhiteSpace(name);

		IdWorkflow = idWorkflow;
		Name = name;
	}

	internal bool AddNode(IWorkflowNode node)
	{
		Throw.IfArgumentNull(node);
		var added = _nodesDict.TryAdd(node.IdNode, node);
		
		if (added)
			Nodes.Add(node);

		return added;
	}

	public IWorkflowNode? GetWorkflowNode(string idNode)
	{
		Throw.IfArgumentNullOrWhiteSpace(idNode);

		return _nodesDict.TryGetValue(idNode, out var node) ? node : null;
	}

	public bool TryGetWorkflowNode(string idNode, out IWorkflowNode? workflowNode)
	{
		Throw.IfArgumentNullOrWhiteSpace(idNode);

		return _nodesDict.TryGetValue(idNode, out workflowNode);
	}

	private readonly static Lazy<IValidator<Workflow>> _workflowValidator = new(() =>
		new ValidatorBuilder<Workflow>()
			.ForProperty(x => x.IdWorkflow, v => v.NotDefaultOrWhiteSpace())
			.ForProperty(x => x.Name, v => v.NotDefaultOrWhiteSpace())
			.ForProperty(x => x.Nodes, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.StartNode, v => v.NotNull())
			.WithPropertyError(
				x => x._nodesDict,
				(t, parent) =>
				{
					if (t?._nodesDict == null || t._nodesDict.Count == 0)
						return ValidationResultFactory.Failure<Dictionary<string, IWorkflowNode>>(
							new ErrorCode("WF_VAL_Nodes", $"{nameof(Nodes)} == null"),
							nameof(Nodes));

					foreach (var node in t._nodesDict)
					{
						var res = node.Value.Validate();
						if (res.HasError)
							return res;
					}

					return ValidationResultFactory.Success();
				})
			//.ForEach(x => x.Nodes, WorkflowNodeBase.WorkflowNodeBaseValidatorRules)
			.Build());

	public static IValidator<Workflow> WorkflowValidator => _workflowValidator.Value;

	public IValidationResult Validate()
		=> WorkflowValidator
			.Validate(this);

	public IValidationResult Init()
	{
		_nodesDict.Clear();

		if (Nodes == null || Nodes.Count == 0)
			throw new InvalidOperationException($"Workflow '{IdWorkflow}' has no nodes defined.");

		foreach (var node in Nodes)
		{
			if (!_nodesDict.TryAdd(node.IdNode, node))
				throw new InvalidOperationException($"Node with ID '{node.IdNode}' already exists.");
		}

		return Validate();
	}

	internal static Workflow? LoadFromJson(string json)
	{
		var workflow = JsonSerializerHelper.Deserialize<Workflow>(json);

		if (workflow == null)
			return null;

		var validationResult = workflow.Init();
		if (validationResult.HasError)
			throw validationResult.ToException(ScopeContext.Create($"{nameof(Workflow)}: {nameof(workflow.IdWorkflow)} = {workflow.IdWorkflow}"))!;

		return workflow;
	}

	internal static Workflow? LoadFromXml(string xml, System.Text.Encoding? encoding)
	{
		var workflow = XmlSerializerHelper.DeserializeFromString<Workflow>(xml, encoding);

		if (workflow == null)
			return null;

		var validationResult = workflow.Init();
		if (validationResult.HasError)
			throw validationResult.ToException(ScopeContext.Create($"{nameof(Workflow)}: {nameof(workflow.IdWorkflow)} = {workflow.IdWorkflow}"))!;

		return workflow;
	}
}
