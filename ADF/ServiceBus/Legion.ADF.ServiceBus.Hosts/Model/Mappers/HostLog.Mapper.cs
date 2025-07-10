using Legion;
using Legion.Model.Mappers;

namespace Legion.ADF.ServiceBus.Hosts.Model;

public sealed partial class HostLog : Hosts.HostsBaseEntity, Legion.Model.IEntity
{
	public static Hosts.Model.HostLog? Map(
		Hosts.Model.HostLog source,
		Hosts.Model.HostLog? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Hosts.Model.HostLog>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		Throw.IfArgumentNull(source);

		return source.MapTo(target, referenceModifier, conditions, instanceFactory, cache);
	}

	public Hosts.Model.HostLog? Clone(
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Hosts.Model.HostLog>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
		=> MapTo(target: null, referenceModifier, conditions, instanceFactory, cache);

	public Hosts.Model.HostLog? MapTo(
		Hosts.Model.HostLog? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Hosts.Model.HostLog>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		cache ??= [];
		target ??= instanceFactory?.CreateInstance<Legion.ADF.ServiceBus.Hosts.Model.HostLog>(
				useActivatorIfNoFactoryFound: false,
				throwIfFactoryReturnsNull: true)
			?? new Legion.ADF.ServiceBus.Hosts.Model.HostLog();

		if (cache.TryGetValue(this, out var cached))
			return (Hosts.Model.HostLog)cached;
			
		MappingConditions<Hosts.Model.HostLog>? conds = null;
		if (conditions != null)
		{
			conds = new MappingConditions<Hosts.Model.HostLog>();
			conditions.Invoke(conds);

			if (conds.CanMap(this, nameof(IdHostLog)))
				target.IdHostLog = IdHostLog;
			if (conds.CanMap(this, nameof(IdHost)))
				target.IdHost = IdHost;
			if (conds.CanMap(this, nameof(IdLogLevel)))
				target.IdLogLevel = IdLogLevel;
			if (conds.CanMap(this, nameof(CreatedUtc)))
				target.CreatedUtc = CreatedUtc;
			if (conds.CanMap(this, nameof(IsRunning)))
				target.IsRunning = IsRunning;
			if (conds.CanMap(this, nameof(TraceCorrelationId)))
				target.TraceCorrelationId = TraceCorrelationId;
			if (conds.CanMap(this, nameof(IdLogMessage)))
				target.IdLogMessage = IdLogMessage;
			if (conds.CanMap(this, nameof(Code)))
				target.Code = Code;
			if (conds.CanMap(this, nameof(Detail)))
				target.Detail = Detail;
		}
		else
		{
			target.IdHostLog = IdHostLog;
			target.IdHost = IdHost;
			target.IdLogLevel = IdLogLevel;
			target.CreatedUtc = CreatedUtc;
			target.IsRunning = IsRunning;
			target.TraceCorrelationId = TraceCorrelationId;
			target.IdLogMessage = IdLogMessage;
			target.Code = Code;
			target.Detail = Detail;
		}

		cache.Add(this, target);

		if (referenceModifier == ReferenceModifier.MapAllReferences)
		{
			target.Host = Host?.MapTo(target.Host, referenceModifier, conds?.GetConditions(x => x.Host), instanceFactory, cache)!;
		}
		else if (referenceModifier == ReferenceModifier.SetNull)
		{
			target.Host = null!;
		}

		return target;
	}
}
