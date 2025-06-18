using Legion;
using Legion.Model.Mappers;

namespace Legion.ADF.ServiceBus.Jobs.Model;

public sealed partial class JobLog : Jobs.JobsBaseEntity, Legion.Model.IEntity
{
	public static Jobs.Model.JobLog? Map(
		Jobs.Model.JobLog source,
		Jobs.Model.JobLog? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Jobs.Model.JobLog>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		Throw.IfArgumentNull(source);

		return source.MapTo(target, referenceModifier, conditions, instanceFactory, cache);
	}

	public Jobs.Model.JobLog? Clone(
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Jobs.Model.JobLog>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
		=> MapTo(target: null, referenceModifier, conditions, instanceFactory, cache);

	public Jobs.Model.JobLog? MapTo(
		Jobs.Model.JobLog? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Jobs.Model.JobLog>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		cache ??= [];
		target ??= instanceFactory?.CreateInstance<Legion.ADF.ServiceBus.Jobs.Model.JobLog>(
				useActivatorIfNoFactoryFound: false,
				throwIfFactoryReturnsNull: true)
			?? new Legion.ADF.ServiceBus.Jobs.Model.JobLog();

		if (cache.TryGetValue(this, out var cached))
			return (Jobs.Model.JobLog)cached;
			
		MappingConditions<Jobs.Model.JobLog>? conds = null;
		if (conditions != null)
		{
			conds = new MappingConditions<Jobs.Model.JobLog>();
			conditions.Invoke(conds);

			if (conds.CanMap(this, nameof(IdJobLog)))
				target.IdJobLog = IdJobLog;
			if (conds.CanMap(this, nameof(IdJob)))
				target.IdJob = IdJob;
			if (conds.CanMap(this, nameof(IdLogLevel)))
				target.IdLogLevel = IdLogLevel;
			if (conds.CanMap(this, nameof(CreatedUtc)))
				target.CreatedUtc = CreatedUtc;
			if (conds.CanMap(this, nameof(IdJobStatus)))
				target.IdJobStatus = IdJobStatus;
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
			target.IdJobLog = IdJobLog;
			target.IdJob = IdJob;
			target.IdLogLevel = IdLogLevel;
			target.CreatedUtc = CreatedUtc;
			target.IdJobStatus = IdJobStatus;
			target.TraceCorrelationId = TraceCorrelationId;
			target.IdLogMessage = IdLogMessage;
			target.Code = Code;
			target.Detail = Detail;
			target.IdMessageProcessingLog = IdMessageProcessingLog;
		}

		cache.Add(this, target);

		if (referenceModifier == ReferenceModifier.MapAllReferences)
		{
			target.Job = Job?.MapTo(target.Job, referenceModifier, conds?.GetConditions(x => x.Job), instanceFactory, cache)!;
			target.JobStatus = JobStatus?.MapTo(target.JobStatus, referenceModifier, conds?.GetConditions(x => x.JobStatus), instanceFactory, cache)!;
		}
		else if (referenceModifier == ReferenceModifier.SetNull)
		{
			target.Job = null!;
			target.JobStatus = null!;
		}

		return target;
	}
}
