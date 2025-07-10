using Legion;
using Legion.Model.Mappers;

namespace Legion.ADF.ServiceBus.Model;

public sealed partial class OrchestrationStepProcessing : ServiceBus.ServiceBusBaseEntity, Legion.Model.IEntity
{
	public static ServiceBus.Model.OrchestrationStepProcessing? Map(
		ServiceBus.Model.OrchestrationStepProcessing source,
		ServiceBus.Model.OrchestrationStepProcessing? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<ServiceBus.Model.OrchestrationStepProcessing>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		Throw.IfArgumentNull(source);

		return source.MapTo(target, referenceModifier, conditions, instanceFactory, cache);
	}

	public ServiceBus.Model.OrchestrationStepProcessing? Clone(
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<ServiceBus.Model.OrchestrationStepProcessing>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
		=> MapTo(target: null, referenceModifier, conditions, instanceFactory, cache);

	public ServiceBus.Model.OrchestrationStepProcessing? MapTo(
		ServiceBus.Model.OrchestrationStepProcessing? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<ServiceBus.Model.OrchestrationStepProcessing>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		cache ??= [];
		target ??= instanceFactory?.CreateInstance<Legion.ADF.ServiceBus.Model.OrchestrationStepProcessing>(
				useActivatorIfNoFactoryFound: false,
				throwIfFactoryReturnsNull: true)
			?? new Legion.ADF.ServiceBus.Model.OrchestrationStepProcessing();

		if (cache.TryGetValue(this, out var cached))
			return (ServiceBus.Model.OrchestrationStepProcessing)cached;
			
		MappingConditions<ServiceBus.Model.OrchestrationStepProcessing>? conds = null;
		if (conditions != null)
		{
			conds = new MappingConditions<ServiceBus.Model.OrchestrationStepProcessing>();
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
			target._orchestrationStepProcessingDirections = MapperHelper.MapToList(OrchestrationStepProcessingDirections, target._orchestrationStepProcessingDirections, Legion.ADF.ServiceBus.Model.OrchestrationStepProcessingDirection.Map, referenceModifier, conds?.GetConditions(x => x.OrchestrationStepProcessingDirections), instanceFactory, cache)!;
			target._orchestrationStepProcessingLogs = MapperHelper.MapToList(OrchestrationStepProcessingLogs, target._orchestrationStepProcessingLogs, Legion.ADF.ServiceBus.Model.OrchestrationStepProcessingLog.Map, referenceModifier, conds?.GetConditions(x => x.OrchestrationStepProcessingLogs), instanceFactory, cache)!;
			target._orchestrationStepProcessingMessages = MapperHelper.MapToList(OrchestrationStepProcessingMessages, target._orchestrationStepProcessingMessages, Legion.ADF.ServiceBus.Model.OrchestrationStepProcessingMessage.Map, referenceModifier, conds?.GetConditions(x => x.OrchestrationStepProcessingMessages), instanceFactory, cache)!;
			target._toStepOrchestrationStepProcessingDirections = MapperHelper.MapToList(ToStepOrchestrationStepProcessingDirections, target._toStepOrchestrationStepProcessingDirections, Legion.ADF.ServiceBus.Model.OrchestrationStepProcessingDirection.Map, referenceModifier, conds?.GetConditions(x => x.ToStepOrchestrationStepProcessingDirections), instanceFactory, cache)!;
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
