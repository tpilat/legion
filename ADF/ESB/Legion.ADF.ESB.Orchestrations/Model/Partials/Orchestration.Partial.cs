using Legion.ADF.ESB.Orchestrations;
using Legion.Extensions;

namespace Legion.ADF.ESB.Orchestrations.Model;

public sealed partial class Orchestration : Orchestrations.OrchestrationsBaseEntity, Legion.Model.IEntity
{
	internal static IResult<Orchestration> CreateOrchestration(
		IScopeContext scopeContext,
		IESBOrchestration esbOrchestration)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<Orchestration>();

		if (result.IsArgumentNull(scopeContext, esbOrchestration))
			return result.Build();

		if (result.IsArgumentNullOrEmpty(scopeContext, esbOrchestration.Steps))
			return result.Build();

		var orchestration = new Orchestration
		{
			IdOrchestration = esbOrchestration.IdOrchestration,
			Code = esbOrchestration.Code,
			Name = esbOrchestration.Name,
			Description = esbOrchestration.Description,
			IsSingleton = esbOrchestration.IsSingleton,
			Class = esbOrchestration.GetType().ToFriendlyFullName(),
			Properties = esbOrchestration.Properties,
			TimeoutForMessageProcessingInSeconds = esbOrchestration.TimeoutForMessageProcessingInSeconds,
			MaxMessageProcessingRetryCount = esbOrchestration.MaxMessageProcessingRetryCount,
			Version = esbOrchestration.Version,
			ValidTo = esbOrchestration.ValidTo
		};

		foreach (var esbOrchestrationStep in esbOrchestration.Steps)
		{
			var orchestrationStepResult = OrchestrationStep.CreateOrchestrationStep(scopeContext, esbOrchestrationStep);

			if (result.MergeHasError(orchestrationStepResult))
				return result.Build();

			orchestration._orchestrationSteps.Add(orchestrationStepResult.Data!);
		}

		var validationResult = DefaultDBValidator.Validate(orchestration);

		if (result.MergeHasError(scopeContext, validationResult, true))
			return result.Build();

		return result.WithData(orchestration).Build();
	}

	public static bool AreEquals(Orchestration orchestration1, Orchestration orchestration2)
		=> orchestration1 != null
			&& orchestration2 != null
			&& orchestration1.Class == orchestration2.Class
			&& orchestration1.IdOrchestration == orchestration2.IdOrchestration;
}
