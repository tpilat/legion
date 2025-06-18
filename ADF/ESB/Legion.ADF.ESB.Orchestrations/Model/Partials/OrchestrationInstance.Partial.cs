using Legion.ADF.ESB.Orchestrations;

namespace Legion.ADF.ESB.Orchestrations.Model;

public sealed partial class OrchestrationInstance : Orchestrations.OrchestrationsBaseEntity, Legion.Model.IEntity
{
	internal static IResult<OrchestrationInstance> CreateOrchestrationInstance(
		IScopeContext scopeContext,
		IESBOrchestrationInstance esbOrchestrationInstance)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<OrchestrationInstance>();

		if (result.IsArgumentNull(scopeContext, esbOrchestrationInstance))
			return result.Build();

		var orchestration = new OrchestrationInstance
		{
			IdOrchestrationInstance = esbOrchestrationInstance.IdOrchestrationInstance,
			IdOrchestration = esbOrchestrationInstance.IdOrchestration,
			IdOrchestrationStatus = esbOrchestrationInstance.IdOrchestrationStatus,
			CreatedUtc = esbOrchestrationInstance.CreatedUtc
		};

		var validationResult = DefaultDBValidator.Validate(orchestration);

		if (result.MergeHasError(scopeContext, validationResult, true))
			return result.Build();

		return result.WithData(orchestration).Build();
	}

	public static bool AreEquals(OrchestrationInstance orchestrationInstance1, OrchestrationInstance orchestrationInstance2)
		=> orchestrationInstance1?.Orchestration != null
			&& orchestrationInstance2?.Orchestration != null
			&& orchestrationInstance1.Orchestration.Class == orchestrationInstance2.Orchestration.Class
			&& orchestrationInstance1.IdOrchestrationInstance == orchestrationInstance2.IdOrchestrationInstance;
}
