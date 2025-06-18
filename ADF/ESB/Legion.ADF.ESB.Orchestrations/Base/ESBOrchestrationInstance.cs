using Microsoft.Extensions.Options;

namespace Legion.ADF.ESB.Orchestrations;

public abstract class ESBOrchestrationInstance : ESBOrchestration, IESBOrchestrationInstance
{
	public Guid IdOrchestrationInstance { get; protected set; }

	public Guid IdOrchestrationStatus { get; protected set; }

	public DateTime CreatedUtc { get; protected set; }



	public ESBOrchestrationInstance()
		: base()
	{
		IdOrchestrationStatus = Legion.ADF.ESB.Orchestrations.Model.OrchestrationStatus.Offline;
	}

	//public abstract ESBOrchestrationInstance<TOptions> Default();

	public new IResult<Legion.ADF.ESB.Orchestrations.Model.OrchestrationInstance> ToPersistentModel(IScopeContext scopeContext)
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

public abstract class ESBOrchestrationInstance<TOptions> : ESBOrchestration<TOptions>, IESBOrchestrationInstance
	where TOptions : class
{
	public Guid IdOrchestrationInstance { get; protected set; }

	public Guid IdOrchestrationStatus { get; protected set; }

	public DateTime CreatedUtc { get; protected set; }



	public ESBOrchestrationInstance(IOptions<TOptions> options)
		: base(options)
	{
		IdOrchestrationStatus = Legion.ADF.ESB.Orchestrations.Model.OrchestrationStatus.Offline;
	}

	//public abstract ESBOrchestrationInstance<TOptions> Default();

	public new IResult<Legion.ADF.ESB.Orchestrations.Model.OrchestrationInstance> ToPersistentModel(IScopeContext scopeContext)
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
