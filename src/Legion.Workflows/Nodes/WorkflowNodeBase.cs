using Legion.Validation;

namespace Legion.Workflows.Nodes;

public abstract class WorkflowNodeBase : IWorkflowNode
{
	public string IdNode { get; set; }

	protected WorkflowNodeBase()
	{
	}

	public WorkflowNodeBase(string idNode)
	{
		Throw.IfArgumentNullOrWhiteSpace(idNode);

		IdNode = idNode;
	}

	public abstract Task<NodeResult> ExecuteAsync(INodeContext nodeContext, WorkflowExecutor exec);

	private readonly static Lazy<IValidator<IWorkflowNode>> _workflowNodeBaseValidator = new(() =>
	{
		var builder = new ValidatorBuilder<IWorkflowNode>();
		WorkflowNodeBaseValidatorRules(builder);
		return builder.Build();
	});

	public static IValidator<IWorkflowNode> WorkflowNodeBaseValidator => _workflowNodeBaseValidator.Value;

	public static void WorkflowNodeBaseValidatorRules(ValidatorBuilder<IWorkflowNode> builder)
	{
		builder?
			.ForProperty(x => x.IdNode, v => v.NotDefaultOrWhiteSpace());
	}

	public  virtual IValidationResult Validate()
		=> WorkflowNodeBaseValidator
			.Validate(this);
}
