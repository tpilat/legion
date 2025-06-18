namespace Legion.ADF.ESB.Orchestrations;

public interface IESBOrchestrationInstance : IESBOrchestration
{
	Guid IdOrchestrationInstance { get; }

	Guid IdOrchestrationStatus { get; }

	DateTime CreatedUtc { get; }
}
