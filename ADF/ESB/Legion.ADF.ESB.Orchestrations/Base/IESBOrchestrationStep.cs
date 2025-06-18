namespace Legion.ADF.ESB.Orchestrations;

public interface IESBOrchestrationStep
{
	Guid IdOrchestrationStep { get; }

	bool IsMainEntry { get; }

	string Code { get; }

	string Name { get; }

	string? Description { get; }

	string? Properties { get; }

	int Order { get; }
}
