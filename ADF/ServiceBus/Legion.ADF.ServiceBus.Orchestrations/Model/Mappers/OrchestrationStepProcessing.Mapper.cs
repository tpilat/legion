using Legion;
using Legion.Model.Mappers;

namespace Legion.ADF.ServiceBus.Orchestrations.Model;

public sealed partial class OrchestrationStepProcessing : Orchestrations.OrchestrationsBaseEntity, Legion.Model.IEntity
{
	public static Orchestrations.Model.OrchestrationStepProcessing? Map(
		Orchestrations.Model.OrchestrationStepProcessing source,
		Orchestrations.Model.OrchestrationStepProcessing? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Orchestrations.Model.OrchestrationStepProcessing>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		Throw.IfArgumentNull(source);

		return source.MapTo(target, referenceModifier, conditions, instanceFactory, cache);
	}

	public Orchestrations.Model.OrchestrationStepProcessing? Clone(
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Orchestrations.Model.OrchestrationStepProcessing>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
		=> MapTo(target: null, referenceModifier, conditions, instanceFactory, cache);

	public Orchestrations.Model.OrchestrationStepProcessing? MapTo(
		Orchestrations.Model.OrchestrationStepProcessing? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Orchestrations.Model.OrchestrationStepProcessing>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		cache ??= [];
		target ??= instanceFactory?.CreateInstance<Legion.ADF.ServiceBus.Orchestrations.Model.OrchestrationStepProcessing>(
				useActivatorIfNoFactoryFound: false,
				throwIfFactoryReturnsNull: true)
			?? new Legion.ADF.ServiceBus.Orchestrations.Model.OrchestrationStepProcessing();

		if (cache.TryGetValue(this, out var cached))
			return (Orchestrations.Model.OrchestrationStepProcessing)cached;
			
		MappingConditions<Orchestrations.Model.OrchestrationStepProcessing>? conds = null;
		if (conditions != null)
		{
			conds = new MappingConditions<Orchestrations.Model.OrchestrationStepProcessing>();
			conditions.Invoke(conds);

			if (conds.CanMap(this, nameof(IdOrchestrationStepProcessing)))
				target.IdOrchestrationStepProcessing = IdOrchestrationStepProcessing;
			if (conds.CanMap(this, nameof(IdOrchestrationStep)))
				target.IdOrchestrationStep = IdOrchestrationStep;
			if (conds.CanMap(this, nameof(IdOrchestrationInstance)))
				target.IdOrchestrationInstance = IdOrchestrationInstance;
			if (conds.CanMap(this, nameof(IdOrchestrationStepProcessingStatus)))
				target.IdOrchestrationStepProcessingStatus = IdOrchestrationStepProcessingStatus;
			if (conds.CanMap(this, nameof(CreatedUtc)))
				target.CreatedUtc = CreatedUtc;
			if (conds.CanMap(this, nameof(ProcessedUtc)))
				target.ProcessedUtc = ProcessedUtc;
			if (conds.CanMap(this, nameof(SuspendedUtc)))
				target.SuspendedUtc = SuspendedUtc;
			if (conds.CanMap(this, nameof(LastProcessingUtc)))
				target.LastProcessingUtc = LastProcessingUtc;
			if (conds.CanMap(this, nameof(NextProcessingUtc)))
				target.NextProcessingUtc = NextProcessingUtc;
			if (conds.CanMap(this, nameof(RetryCount)))
				target.RetryCount = RetryCount;
		}
		else
		{
			target.IdOrchestrationStepProcessing = IdOrchestrationStepProcessing;
			target.IdOrchestrationStep = IdOrchestrationStep;
			target.IdOrchestrationInstance = IdOrchestrationInstance;
			target.IdOrchestrationStepProcessingStatus = IdOrchestrationStepProcessingStatus;
			target.CreatedUtc = CreatedUtc;
			target.ProcessedUtc = ProcessedUtc;
			target.SuspendedUtc = SuspendedUtc;
			target.LastProcessingUtc = LastProcessingUtc;
			target.NextProcessingUtc = NextProcessingUtc;
			target.RetryCount = RetryCount;
		}

		cache.Add(this, target);

		if (referenceModifier == ReferenceModifier.MapAllReferences)
		{
			target.OrchestrationInstance = OrchestrationInstance?.MapTo(target.OrchestrationInstance, referenceModifier, conds?.GetConditions(x => x.OrchestrationInstance), instanceFactory, cache)!;
			target.OrchestrationStep = OrchestrationStep?.MapTo(target.OrchestrationStep, referenceModifier, conds?.GetConditions(x => x.OrchestrationStep), instanceFactory, cache)!;
			target.OrchestrationStepProcessingStatus = OrchestrationStepProcessingStatus?.MapTo(target.OrchestrationStepProcessingStatus, referenceModifier, conds?.GetConditions(x => x.OrchestrationStepProcessingStatus), instanceFactory, cache)!;
			target._orchestrationStepProcessingDirections = MapperHelper.MapToList(OrchestrationStepProcessingDirections, target._orchestrationStepProcessingDirections, Legion.ADF.ServiceBus.Orchestrations.Model.OrchestrationStepProcessingDirection.Map, referenceModifier, conds?.GetConditions(x => x.OrchestrationStepProcessingDirections), instanceFactory, cache)!;
			target._orchestrationStepProcessingLogs = MapperHelper.MapToList(OrchestrationStepProcessingLogs, target._orchestrationStepProcessingLogs, Legion.ADF.ServiceBus.Orchestrations.Model.OrchestrationStepProcessingLog.Map, referenceModifier, conds?.GetConditions(x => x.OrchestrationStepProcessingLogs), instanceFactory, cache)!;
			target._orchestrationStepProcessingMessages = MapperHelper.MapToList(OrchestrationStepProcessingMessages, target._orchestrationStepProcessingMessages, Legion.ADF.ServiceBus.Orchestrations.Model.OrchestrationStepProcessingMessage.Map, referenceModifier, conds?.GetConditions(x => x.OrchestrationStepProcessingMessages), instanceFactory, cache)!;
			target._toStepOrchestrationStepProcessingDirections = MapperHelper.MapToList(ToStepOrchestrationStepProcessingDirections, target._toStepOrchestrationStepProcessingDirections, Legion.ADF.ServiceBus.Orchestrations.Model.OrchestrationStepProcessingDirection.Map, referenceModifier, conds?.GetConditions(x => x.ToStepOrchestrationStepProcessingDirections), instanceFactory, cache)!;
		}
		else if (referenceModifier == ReferenceModifier.SetNull)
		{
			target.OrchestrationInstance = null!;
			target.OrchestrationStep = null!;
			target.OrchestrationStepProcessingStatus = null!;
			target._orchestrationStepProcessingDirections = [];
			target._orchestrationStepProcessingLogs = [];
			target._orchestrationStepProcessingMessages = [];
			target._toStepOrchestrationStepProcessingDirections = [];
		}

		return target;
	}
}
