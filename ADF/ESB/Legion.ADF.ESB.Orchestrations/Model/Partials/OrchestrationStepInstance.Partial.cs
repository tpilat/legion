using Legion.ADF.ESB.Orchestrations;

namespace Legion.ADF.ESB.Orchestrations.Model;

public sealed partial class OrchestrationStepInstance : Orchestrations.OrchestrationsBaseEntity, Legion.Model.IEntity
{
	internal static IResult<OrchestrationStepInstance> CreateOrchestrationStepInstance(
		IScopeContext scopeContext,
		IESBOrchestrationStepInstance esbOrchestrationStepInstance)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<OrchestrationStepInstance>();

		if (result.IsArgumentNull(scopeContext, esbOrchestrationStepInstance))
			return result.Build();

		var orchestrationStepInstance = new OrchestrationStepInstance
		{
			IdOrchestrationStepInstance = esbOrchestrationStepInstance.IdOrchestrationStepInstance,
			//IdOrchestration = idOrchestration,
			IdOrchestrationStep = esbOrchestrationStepInstance.IdOrchestrationStep,
			IdStepStatus = esbOrchestrationStepInstance.IdStepStatus,
			LastProcessedUtc = esbOrchestrationStepInstance.LastProcessedUtc,
			NextProcessingUtc = esbOrchestrationStepInstance.NextProcessingUtc,
			RetryCount = esbOrchestrationStepInstance.RetryCount,
			SucceededUtc = esbOrchestrationStepInstance.SucceededUtc,
			SuspendedUtc = esbOrchestrationStepInstance.SuspendedUtc
		};

		var validationResult = DefaultDBValidator.Validate(orchestrationStepInstance);

		if (result.MergeHasError(scopeContext, validationResult, true))
			return result.Build();

		return result.WithData(orchestrationStepInstance).Build();
	}

	public static bool AreEquals(OrchestrationStepInstance orchestrationStepInstance1, OrchestrationStepInstance orchestrationStepInstance2)
		=> orchestrationStepInstance1?.OrchestrationStep != null
			&& orchestrationStepInstance2?.OrchestrationStep != null
			&& orchestrationStepInstance1.OrchestrationStep.Class == orchestrationStepInstance2.OrchestrationStep.Class
			&& orchestrationStepInstance1.IdOrchestrationStepInstance == orchestrationStepInstance2.IdOrchestrationStepInstance;
}
