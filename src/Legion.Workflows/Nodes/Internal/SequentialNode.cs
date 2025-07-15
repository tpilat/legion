using Legion.Validation;

namespace Legion.Workflows.Nodes.Internal;

internal class SequentialNode : WorkflowNodeBase
{
	public const string CurrentChildIndex = nameof(CurrentChildIndex);

	public List<string> ChildNodeIds { get; set; }

	public SequentialNode()
		: base()
	{
		ChildNodeIds = [];
	}

	public SequentialNode(string id)
		: base(id)
	{
		ChildNodeIds = [];
	}

	public override async Task<NodeResult> ExecuteAsync(INodeContext nodeContext, WorkflowExecutor exec)
	{
		if (ChildNodeIds.Count == 0)
			return nodeContext.SetResult(NodeResult.Success, result: null, idNextNode: null, $"{nameof(ChildNodeIds)}.Count == 0", waitForEvent: null, force: true);

		var currentChildIndex = 0;
		currentChildIndex = nodeContext.AddOrGetVariable(CurrentChildIndex, currentChildIndex);

		while (currentChildIndex < ChildNodeIds.Count)
		{
			string childId = ChildNodeIds[currentChildIndex];
			if (!exec.Workflow.TryGetWorkflowNode(childId, out var childNode))
			{
				var error = $"Child node '{childId}' not found";
				nodeContext.GetLogger().LogError(error);
				return nodeContext.SetResult(NodeResult.Failure, result: null, idNextNode: null, error, waitForEvent: null, force: true);
			}

			var result = await exec.ExecuteNodeAsync(
				childId,
				GlobalContext.Instance.NewGuid(),
				restart: false,
				onExecuted: async res =>
				{
					if (res == NodeResult.Success)
					{
						currentChildIndex++;
						nodeContext.SetVariable(CurrentChildIndex, currentChildIndex);
					}

					return res;
				});
		}

		return nodeContext.SetResult(NodeResult.Success, result: null, idNextNode: null, message: null, waitForEvent: null, force: true);
	}

	private readonly static Lazy<IValidator<IWorkflowNode>> _sequentialNodeValidator = new(() =>
	{
		var baseBuilder = new ValidatorBuilder<IWorkflowNode>();
		WorkflowNodeBaseValidatorRules(baseBuilder);

		baseBuilder.ForChildImplementation<SequentialNode>(builder => SequentialNodeValidatorRules(builder));
		return baseBuilder.Build();
	});

	public static IValidator<IWorkflowNode> SequentialNodeValidator => _sequentialNodeValidator.Value;

	public static void SequentialNodeValidatorRules(ValidatorBuilder<SequentialNode> builder)
	{
		builder?
			.ForProperty(x => x.ChildNodeIds, v => v.NotDefaultOrEmpty());
	}

	public override IValidationResult Validate()
		=> SequentialNodeValidator
			.Validate(this);
}
