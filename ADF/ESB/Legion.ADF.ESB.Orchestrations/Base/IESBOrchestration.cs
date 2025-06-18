namespace Legion.ADF.ESB.Orchestrations;

public interface IESBOrchestration
{
	Guid IdOrchestration { get; }
	
	string Code { get; }

	string Name { get; }

	string? Description { get; }

	bool IsSingleton { get; }

	string? Properties { get; }

	int TimeoutForMessageProcessingInSeconds { get; }

	int MaxMessageProcessingRetryCount { get; }

	string Version { get; }

	DateTime? ValidTo { get; }

	List<IESBOrchestrationStep> Steps { get; }

	IResult<Legion.ADF.ESB.Orchestrations.Model.Orchestration> ToPersistentModel(IScopeContext scopeContext);
}
