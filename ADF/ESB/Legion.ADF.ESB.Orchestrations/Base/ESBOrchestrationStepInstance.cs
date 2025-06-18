using Microsoft.Extensions.Options;

namespace Legion.ADF.ESB.Orchestrations;

public abstract class ESBOrchestrationStepInstance : ESBOrchestrationStep, IESBOrchestrationStepInstance
{
	public Guid IdOrchestrationStepInstance { get; protected set; }

	public Guid IdStepStatus { get; protected set; }

	public DateTime LastProcessedUtc { get; protected set; }

	public DateTime NextProcessingUtc { get; protected set; }

	public int RetryCount { get; protected set; }

	public DateTime? SucceededUtc { get; protected set; }

	public DateTime? SuspendedUtc { get; protected set; }



	public ESBOrchestrationStepInstance()
		: base()
	{
		IdStepStatus = Legion.ADF.ESB.Orchestrations.Model.OrchestrationStepStatus.Idle;
	}

	//public abstract ESBOrchestrationStepInstance<TOptions> Default();

	public IResult<Legion.ADF.ESB.Orchestrations.Model.OrchestrationStepInstance> ToPersistentModel(IScopeContext scopeContext)
	{
		scopeContext = scopeContext.CreateNew();

		//return IsSingleton

		//	? Legion.ADF.ESB.Orchestrations.Model.OrchestrationInstance.CreateSingletonOrchestration(
		//		scopeContext,
		//		Code,
		//		Name,
		//		Description,
		//		this.GetType(),
		//		Properties,
		//		TimeoutForMessageProcessingInSeconds,
		//		MaxMessageProcessingRetryCount,
		//		Version,
		//		ValidTo)

		//	: Legion.ADF.ESB.Orchestrations.Model.OrchestrationInstance.CreateMultiInstantiableOrchestration(
		//		scopeContext,
		//		Code,
		//		Name,
		//		Description,
		//		this.GetType(),
		//		Properties,
		//		TimeoutForMessageProcessingInSeconds,
		//		MaxMessageProcessingRetryCount,
		//		Version,
		//		ValidTo);

		return null;
	}
}

public abstract class ESBOrchestrationStepInstance<TOptions> : ESBOrchestrationStep<TOptions>, IESBOrchestrationStepInstance
	where TOptions : class
{
	public Guid IdOrchestrationStepInstance { get; protected set; }

	public Guid IdStepStatus { get; protected set; }

	public DateTime LastProcessedUtc { get; protected set; }

	public DateTime NextProcessingUtc { get; protected set; }

	public int RetryCount { get; protected set; }

	public DateTime? SucceededUtc { get; protected set; }

	public DateTime? SuspendedUtc { get; protected set; }



	public ESBOrchestrationStepInstance(IOptions<TOptions> options)
		: base(options)
	{
		IdStepStatus = Legion.ADF.ESB.Orchestrations.Model.OrchestrationStepStatus.Idle;
	}

	//public abstract ESBOrchestrationStepInstance<TOptions> Default();

	public IResult<Legion.ADF.ESB.Orchestrations.Model.OrchestrationStepInstance> ToPersistentModel(IScopeContext scopeContext)
	{
		scopeContext = scopeContext.CreateNew();

		//return IsSingleton

		//	? Legion.ADF.ESB.Orchestrations.Model.OrchestrationInstance.CreateSingletonOrchestration(
		//		scopeContext,
		//		Code,
		//		Name,
		//		Description,
		//		this.GetType(),
		//		Properties,
		//		TimeoutForMessageProcessingInSeconds,
		//		MaxMessageProcessingRetryCount,
		//		Version,
		//		ValidTo)

		//	: Legion.ADF.ESB.Orchestrations.Model.OrchestrationInstance.CreateMultiInstantiableOrchestration(
		//		scopeContext,
		//		Code,
		//		Name,
		//		Description,
		//		this.GetType(),
		//		Properties,
		//		TimeoutForMessageProcessingInSeconds,
		//		MaxMessageProcessingRetryCount,
		//		Version,
		//		ValidTo);

		return null;
	}
}
