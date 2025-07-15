using Legion;
using Legion.Model.Mappers;

namespace Legion.ADF.ServiceBus.Model;

public sealed partial class Host : ServiceBus.ServiceBusBaseEntity, Legion.Model.Concurrence.IConcurrent, Legion.Model.IEntity
{
	public static ServiceBus.Model.Host? Map(
		ServiceBus.Model.Host source,
		ServiceBus.Model.Host? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<ServiceBus.Model.Host>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		Throw.IfArgumentNull(source);

		return source.MapTo(target, referenceModifier, conditions, instanceFactory, cache);
	}

	public ServiceBus.Model.Host? Clone(
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<ServiceBus.Model.Host>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
		=> MapTo(target: null, referenceModifier, conditions, instanceFactory, cache);

	public ServiceBus.Model.Host? MapTo(
		ServiceBus.Model.Host? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<ServiceBus.Model.Host>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		cache ??= [];
		target ??= instanceFactory?.CreateInstance<Legion.ADF.ServiceBus.Model.Host>(
				useActivatorIfNoFactoryFound: false,
				throwIfFactoryReturnsNull: true)
			?? new Legion.ADF.ServiceBus.Model.Host();

		if (cache.TryGetValue(this, out var cached))
			return (ServiceBus.Model.Host)cached;
			
		MappingConditions<ServiceBus.Model.Host>? conds = null;
		if (conditions != null)
		{
			conds = new MappingConditions<ServiceBus.Model.Host>();
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
			if (conds.CanMap(this, nameof(Configuration)))
				target.Configuration = Configuration;
			if (conds.CanMap(this, nameof(RowVersion)))
				target.RowVersion = RowVersion;
		}
		else
		{
			target.IdHost = IdHost;
			target.Name = Name;
			target.Description = Description;
			target.CreatedUtc = CreatedUtc;
			target.IsEnabled = IsEnabled;
			target.Configuration = Configuration;
			target.RowVersion = RowVersion;
		}

		cache.Add(this, target);

		if (referenceModifier == ReferenceModifier.MapAllReferences)
		{
			target.HostActivity = HostActivity?.MapTo(target.HostActivity, referenceModifier, conds?.GetConditions(x => x.HostActivity), instanceFactory, cache)!;
			target._hostLogs = MapperHelper.MapToList(HostLogs, target._hostLogs, Legion.ADF.ServiceBus.Model.HostLog.Map, referenceModifier, conds?.GetConditions(x => x.HostLogs), instanceFactory, cache)!;
		}
		else if (referenceModifier == ReferenceModifier.SetNull)
		{
			target.HostActivity = null!;
			target._hostLogs = [];
		}

		return target;
	}
}
