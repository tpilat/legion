using Legion.Validation;

namespace Legion.Workflows.Nodes.Internal;

internal class SetVariableNode : WorkflowNodeBase
{
	public string Key { get; set; }
	public string? Value { get; set; }

	public SetVariableNode()
		: base()
	{
	}

	public SetVariableNode(string id)
		: base(id)
	{
	}

	public override async Task<NodeResult> ExecuteAsync(INodeContext nodeContext, WorkflowExecutor exec)
	{
		exec.WorkflowContext.SetVariable(Key, Value, force: true);

		return nodeContext.SetResult(NodeResult.Success, result: null, idNextNode: null, message: null, waitForEvent: null, force: true);
	}

	private readonly static Lazy<IValidator<IWorkflowNode>> _setNodeValidator = new(() =>
	{
		var baseBuilder = new ValidatorBuilder<IWorkflowNode>();
		WorkflowNodeBaseValidatorRules(baseBuilder);

		baseBuilder.ForChildImplementation<SetVariableNode>(builder => SetNodeValidatorRules(builder));
		return baseBuilder.Build();
	});

	public static IValidator<IWorkflowNode> SetNodeValidator => _setNodeValidator.Value;

	public static void SetNodeValidatorRules(ValidatorBuilder<SetVariableNode> builder)
	{
		builder?
			.ForProperty(x => x.Key, v => v.NotDefaultOrWhiteSpace());
	}

	public override IValidationResult Validate()
		=> SetNodeValidator
			.Validate(this);
}
