using Legion;
using Legion.Model.Mappers;

namespace Legion.ADF.Logs.Model;

public sealed partial class UnstructuredLog : Logs.LogsBaseEntity, Legion.Model.IEntity
{
	public static Logs.Model.UnstructuredLog? Map(
		Logs.Model.UnstructuredLog source,
		Logs.Model.UnstructuredLog? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Logs.Model.UnstructuredLog>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		Throw.IfArgumentNull(source);

		return source.MapTo(target, referenceModifier, conditions, instanceFactory, cache);
	}

	public Logs.Model.UnstructuredLog? Clone(
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Logs.Model.UnstructuredLog>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
		=> MapTo(target: null, referenceModifier, conditions, instanceFactory, cache);

	public Logs.Model.UnstructuredLog? MapTo(
		Logs.Model.UnstructuredLog? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Logs.Model.UnstructuredLog>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		cache ??= [];
		target ??= instanceFactory?.CreateInstance<Legion.ADF.Logs.Model.UnstructuredLog>(
				useActivatorIfNoFactoryFound: false,
				throwIfFactoryReturnsNull: true)
			?? new Legion.ADF.Logs.Model.UnstructuredLog();

		if (cache.TryGetValue(this, out var cached))
			return (Logs.Model.UnstructuredLog)cached;
			
		MappingConditions<Logs.Model.UnstructuredLog>? conds = null;
		if (conditions != null)
		{
			conds = new MappingConditions<Logs.Model.UnstructuredLog>();
			conditions.Invoke(conds);

			if (conds.CanMap(this, nameof(IdUnstructuredLog)))
				target.IdUnstructuredLog = IdUnstructuredLog;
			if (conds.CanMap(this, nameof(CreatedUtc)))
				target.CreatedUtc = CreatedUtc;
			if (conds.CanMap(this, nameof(IdLogLevel)))
				target.IdLogLevel = IdLogLevel;
			if (conds.CanMap(this, nameof(Message)))
				target.Message = Message;
			if (conds.CanMap(this, nameof(StackTrace)))
				target.StackTrace = StackTrace;
			if (conds.CanMap(this, nameof(SourceContext)))
				target.SourceContext = SourceContext;
			if (conds.CanMap(this, nameof(RuntimeUniqueKey)))
				target.RuntimeUniqueKey = RuntimeUniqueKey;
			if (conds.CanMap(this, nameof(EventName)))
				target.EventName = EventName;
			if (conds.CanMap(this, nameof(EventId)))
				target.EventId = EventId;
		}
		else
		{
			target.IdUnstructuredLog = IdUnstructuredLog;
			target.CreatedUtc = CreatedUtc;
			target.IdLogLevel = IdLogLevel;
			target.Message = Message;
			target.StackTrace = StackTrace;
			target.SourceContext = SourceContext;
			target.RuntimeUniqueKey = RuntimeUniqueKey;
			target.EventName = EventName;
			target.EventId = EventId;
		}

		cache.Add(this, target);

		return target;
	}
}
