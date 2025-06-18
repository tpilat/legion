using Legion.ADF.ESB.Orchestrations;
using Legion.Extensions;

namespace Legion.ADF.ESB.Orchestrations.Model;

public sealed partial class OrchestrationStep : Orchestrations.OrchestrationsBaseEntity, Legion.Model.IEntity
{
	internal static IResult<OrchestrationStep> CreateOrchestrationStep(
		IScopeContext scopeContext,
		IESBOrchestrationStep esbOrchestrationStep)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<OrchestrationStep>();

		if (result.IsArgumentNull(scopeContext, esbOrchestrationStep))
			return result.Build();

		var orchestrationStep = new OrchestrationStep
		{
			IdOrchestrationStep = esbOrchestrationStep.IdOrchestrationStep,
			//IdOrchestration = idOrchestration,
			IsMainEntry = esbOrchestrationStep.IsMainEntry,
			Code = esbOrchestrationStep.Code,
			Name = esbOrchestrationStep.Name,
			Description = esbOrchestrationStep.Description,
			Class = esbOrchestrationStep.GetType().ToFriendlyFullName(),
			Properties = esbOrchestrationStep.Properties,
			Order = esbOrchestrationStep.Order
		};

		var validationResult = DefaultDBValidator.Validate(orchestrationStep);

		if (result.MergeHasError(scopeContext, validationResult, true))
			return result.Build();

		return result.WithData(orchestrationStep).Build();
	}

	public static bool AreEquals(OrchestrationStep orchestrationStep1, OrchestrationStep orchestrationStep2)
		=> orchestrationStep1 != null
			&& orchestrationStep2 != null
			&& orchestrationStep1.Class == orchestrationStep2.Class
			&& orchestrationStep1.IdOrchestration == orchestrationStep2.IdOrchestration;
}
