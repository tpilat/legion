using Legion;
using Legion.Model.Mappers;

namespace Legion.ADF.ServiceBus.Model;

public sealed partial class HostActivity : ServiceBus.ServiceBusBaseEntity, Legion.Model.Concurrence.IConcurrent, Legion.Model.IEntity
{
	public static ServiceBus.Model.HostActivity? Map(
		ServiceBus.Model.HostActivity source,
		ServiceBus.Model.HostActivity? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<ServiceBus.Model.HostActivity>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		Throw.IfArgumentNull(source);

		return source.MapTo(target, referenceModifier, conditions, instanceFactory, cache);
	}

	public ServiceBus.Model.HostActivity? Clone(
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<ServiceBus.Model.HostActivity>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
		=> MapTo(target: null, referenceModifier, conditions, instanceFactory, cache);

	public ServiceBus.Model.HostActivity? MapTo(
		ServiceBus.Model.HostActivity? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<ServiceBus.Model.HostActivity>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		cache ??= [];
		target ??= instanceFactory?.CreateInstance<Legion.ADF.ServiceBus.Model.HostActivity>(
				useActivatorIfNoFactoryFound: false,
				throwIfFactoryReturnsNull: true)
			?? new Legion.ADF.ServiceBus.Model.HostActivity();

		if (cache.TryGetValue(this, out var cached))
			return (ServiceBus.Model.HostActivity)cached;
			
		MappingConditions<ServiceBus.Model.HostActivity>? conds = null;
		if (conditions != null)
		{
			conds = new MappingConditions<ServiceBus.Model.HostActivity>();
			conditions.Invoke(conds);

			if (conds.CanMap(this, nameof(IdHostActivity)))
				target.IdHostActivity = IdHostActivity;
			if (conds.CanMap(this, nameof(IdHost)))
				target.IdHost = IdHost;
			if (conds.CanMap(this, nameof(StartedUtc)))
				target.StartedUtc = StartedUtc;
			if (conds.CanMap(this, nameof(LastActivityUtc)))
				target.LastActivityUtc = LastActivityUtc;
			if (conds.CanMap(this, nameof(StoppedUtc)))
				target.StoppedUtc = StoppedUtc;
			if (conds.CanMap(this, nameof(IsDistributedManagerAvailable)))
				target.IsDistributedManagerAvailable = IsDistributedManagerAvailable;
			if (conds.CanMap(this, nameof(RowVersion)))
				target.RowVersion = RowVersion;
		}
		else
		{
			target.IdHostActivity = IdHostActivity;
			target.IdHost = IdHost;
			target.StartedUtc = StartedUtc;
			target.LastActivityUtc = LastActivityUtc;
			target.StoppedUtc = StoppedUtc;
			target.IsDistributedManagerAvailable = IsDistributedManagerAvailable;
			target.RowVersion = RowVersion;
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
