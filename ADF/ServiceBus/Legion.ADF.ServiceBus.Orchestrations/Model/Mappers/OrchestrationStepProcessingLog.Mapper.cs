using Legion;
using Legion.Model.Mappers;

namespace Legion.ADF.ServiceBus.Orchestrations.Model;

public sealed partial class OrchestrationStepProcessingLog : Orchestrations.OrchestrationsBaseEntity, Legion.Model.IEntity
{
	public static Orchestrations.Model.OrchestrationStepProcessingLog? Map(
		Orchestrations.Model.OrchestrationStepProcessingLog source,
		Orchestrations.Model.OrchestrationStepProcessingLog? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Orchestrations.Model.OrchestrationStepProcessingLog>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		Throw.IfArgumentNull(source);

		return source.MapTo(target, referenceModifier, conditions, instanceFactory, cache);
	}

	public Orchestrations.Model.OrchestrationStepProcessingLog? Clone(
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Orchestrations.Model.OrchestrationStepProcessingLog>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
		=> MapTo(target: null, referenceModifier, conditions, instanceFactory, cache);

	public Orchestrations.Model.OrchestrationStepProcessingLog? MapTo(
		Orchestrations.Model.OrchestrationStepProcessingLog? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Orchestrations.Model.OrchestrationStepProcessingLog>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		cache ??= [];
		target ??= instanceFactory?.CreateInstance<Legion.ADF.ServiceBus.Orchestrations.Model.OrchestrationStepProcessingLog>(
				useActivatorIfNoFactoryFound: false,
				throwIfFactoryReturnsNull: true)
			?? new Legion.ADF.ServiceBus.Orchestrations.Model.OrchestrationStepProcessingLog();

		if (cache.TryGetValue(this, out var cached))
			return (Orchestrations.Model.OrchestrationStepProcessingLog)cached;
			
		MappingConditions<Orchestrations.Model.OrchestrationStepProcessingLog>? conds = null;
		if (conditions != null)
		{
			conds = new MappingConditions<Orchestrations.Model.OrchestrationStepProcessingLog>();
			conditions.Invoke(conds);

			if (conds.CanMap(this, nameof(IdOrchestrationStepProcessingLog)))
				target.IdOrchestrationStepProcessingLog = IdOrchestrationStepProcessingLog;
			if (conds.CanMap(this, nameof(IdOrchestrationStepProcessing)))
				target.IdOrchestrationStepProcessing = IdOrchestrationStepProcessing;
			if (conds.CanMap(this, nameof(IdLogLevel)))
				target.IdLogLevel = IdLogLevel;
			if (conds.CanMap(this, nameof(CreatedUtc)))
				target.CreatedUtc = CreatedUtc;
			if (conds.CanMap(this, nameof(IdOrchestrationStepProcessingStatus)))
				target.IdOrchestrationStepProcessingStatus = IdOrchestrationStepProcessingStatus;
			if (conds.CanMap(this, nameof(TraceCorrelationId)))
				target.TraceCorrelationId = TraceCorrelationId;
			if (conds.CanMap(this, nameof(IdLogMessage)))
				target.IdLogMessage = IdLogMessage;
			if (conds.CanMap(this, nameof(Code)))
				target.Code = Code;
			if (conds.CanMap(this, nameof(Detail)))
				target.Detail = Detail;
			if (conds.CanMap(this, nameof(IdMessageProcessingLog)))
				target.IdMessageProcessingLog = IdMessageProcessingLog;
		}
		else
		{
			target.IdOrchestrationStepProcessingLog = IdOrchestrationStepProcessingLog;
			target.IdOrchestrationStepProcessing = IdOrchestrationStepProcessing;
			target.IdLogLevel = IdLogLevel;
			target.CreatedUtc = CreatedUtc;
			target.IdOrchestrationStepProcessingStatus = IdOrchestrationStepProcessingStatus;
			target.TraceCorrelationId = TraceCorrelationId;
			target.IdLogMessage = IdLogMessage;
			target.Code = Code;
			target.Detail = Detail;
			target.IdMessageProcessingLog = IdMessageProcessingLog;
		}

		cache.Add(this, target);

		if (referenceModifier == ReferenceModifier.MapAllReferences)
		{
			target.OrchestrationStepProcessing = OrchestrationStepProcessing?.MapTo(target.OrchestrationStepProcessing, referenceModifier, conds?.GetConditions(x => x.OrchestrationStepProcessing), instanceFactory, cache)!;
			target.OrchestrationStepProcessingStatus = OrchestrationStepProcessingStatus?.MapTo(target.OrchestrationStepProcessingStatus, referenceModifier, conds?.GetConditions(x => x.OrchestrationStepProcessingStatus), instanceFactory, cache)!;
		}
		else if (referenceModifier == ReferenceModifier.SetNull)
		{
			target.OrchestrationStepProcessing = null!;
			target.OrchestrationStepProcessingStatus = null!;
		}

		return target;
	}
}
