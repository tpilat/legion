namespace Legion.ADF.ESB.Orchestrations;

public interface IESBOrchestrationStepInstance : IESBOrchestrationStep
{
	Guid IdOrchestrationStepInstance { get; }

	Guid IdStepStatus { get; }

	DateTime LastProcessedUtc { get; }

	DateTime NextProcessingUtc { get; }

	int RetryCount { get; }

	DateTime? SucceededUtc { get; }

	DateTime? SuspendedUtc { get; }
}
