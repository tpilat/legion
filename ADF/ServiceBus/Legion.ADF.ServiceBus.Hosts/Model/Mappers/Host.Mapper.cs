using Legion;
using Legion.Model.Mappers;

namespace Legion.ADF.ServiceBus.Hosts.Model;

public sealed partial class Host : Hosts.HostsBaseEntity, Legion.Model.IEntity
{
	public static Hosts.Model.Host? Map(
		Hosts.Model.Host source,
		Hosts.Model.Host? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Hosts.Model.Host>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		Throw.IfArgumentNull(source);

		return source.MapTo(target, referenceModifier, conditions, instanceFactory, cache);
	}

	public Hosts.Model.Host? Clone(
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Hosts.Model.Host>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
		=> MapTo(target: null, referenceModifier, conditions, instanceFactory, cache);

	public Hosts.Model.Host? MapTo(
		Hosts.Model.Host? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Hosts.Model.Host>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		cache ??= [];
		target ??= instanceFactory?.CreateInstance<Legion.ADF.ServiceBus.Hosts.Model.Host>(
				useActivatorIfNoFactoryFound: false,
				throwIfFactoryReturnsNull: true)
			?? new Legion.ADF.ServiceBus.Hosts.Model.Host();

		if (cache.TryGetValue(this, out var cached))
			return (Hosts.Model.Host)cached;
			
		MappingConditions<Hosts.Model.Host>? conds = null;
		if (conditions != null)
		{
			conds = new MappingConditions<Hosts.Model.Host>();
			conditions.Invoke(conds);

			if (conds.CanMap(this, nameof(IdHost)))
				target.IdHost = IdHost;
			if (conds.CanMap(this, nameof(Name)))
				target.Name = Name;
			if (conds.CanMap(this, nameof(Description)))
				target.Description = Description;
			if (conds.CanMap(this, nameof(CreatedUtc)))
				target.CreatedUtc = CreatedUtc;
			if (conds.CanMap(this, nameof(IsEnabled)))
				target.IsEnabled = IsEnabled;
			if (conds.CanMap(this, nameof(StartedUtc)))
				target.StartedUtc = StartedUtc;
			if (conds.CanMap(this, nameof(LastActivityUtc)))
				target.LastActivityUtc = LastActivityUtc;
			if (conds.CanMap(this, nameof(StoppedUtc)))
				target.StoppedUtc = StoppedUtc;
			if (conds.CanMap(this, nameof(Configuration)))
				target.Configuration = Configuration;
		}
		else
		{
			target.IdHost = IdHost;
			target.Name = Name;
			target.Description = Description;
			target.CreatedUtc = CreatedUtc;
			target.IsEnabled = IsEnabled;
			target.StartedUtc = StartedUtc;
			target.LastActivityUtc = LastActivityUtc;
			target.StoppedUtc = StoppedUtc;
			target.Configuration = Configuration;
		}

		cache.Add(this, target);

		if (referenceModifier == ReferenceModifier.MapAllReferences)
		{
			target._hostLogs = MapperHelper.MapToList(HostLogs, target._hostLogs, Legion.ADF.ServiceBus.Hosts.Model.HostLog.Map, referenceModifier, conds?.GetConditions(x => x.HostLogs), instanceFactory, cache)!;
		}
		else if (referenceModifier == ReferenceModifier.SetNull)
		{
			target._hostLogs = [];
		}

		return target;
	}
}
