using Legion;
using Legion.Model.Mappers;

namespace Legion.ADF.Logs.Model;

public sealed partial class EventCounterData : Logs.LogsBaseEntity, Legion.Model.IEntity
{
	public static Logs.Model.EventCounterData? Map(
		Logs.Model.EventCounterData source,
		Logs.Model.EventCounterData? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Logs.Model.EventCounterData>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		Throw.IfArgumentNull(source);

		return source.MapTo(target, referenceModifier, conditions, instanceFactory, cache);
	}

	public Logs.Model.EventCounterData? Clone(
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Logs.Model.EventCounterData>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
		=> MapTo(target: null, referenceModifier, conditions, instanceFactory, cache);

	public Logs.Model.EventCounterData? MapTo(
		Logs.Model.EventCounterData? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Logs.Model.EventCounterData>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		cache ??= [];
		target ??= instanceFactory?.CreateInstance<Legion.ADF.Logs.Model.EventCounterData>(
				useActivatorIfNoFactoryFound: false,
				throwIfFactoryReturnsNull: true)
			?? new Legion.ADF.Logs.Model.EventCounterData();

		if (cache.TryGetValue(this, out var cached))
			return (Logs.Model.EventCounterData)cached;
			
		MappingConditions<Logs.Model.EventCounterData>? conds = null;
		if (conditions != null)
		{
			conds = new MappingConditions<Logs.Model.EventCounterData>();
			conditions.Invoke(conds);

			if (conds.CanMap(this, nameof(IdEventCounterData)))
				target.IdEventCounterData = IdEventCounterData;
			if (conds.CanMap(this, nameof(IdEventCounter)))
				target.IdEventCounter = IdEventCounter;
			if (conds.CanMap(this, nameof(CreatedUtc)))
				target.CreatedUtc = CreatedUtc;
			if (conds.CanMap(this, nameof(RuntimeUniqueKey)))
				target.RuntimeUniqueKey = RuntimeUniqueKey;
			if (conds.CanMap(this, nameof(Increment)))
				target.Increment = Increment;
			if (conds.CanMap(this, nameof(Mean)))
				target.Mean = Mean;
			if (conds.CanMap(this, nameof(Count)))
				target.Count = Count;
			if (conds.CanMap(this, nameof(Min)))
				target.Min = Min;
			if (conds.CanMap(this, nameof(Max)))
				target.Max = Max;
		}
		else
		{
			target.IdEventCounterData = IdEventCounterData;
			target.IdEventCounter = IdEventCounter;
			target.CreatedUtc = CreatedUtc;
			target.RuntimeUniqueKey = RuntimeUniqueKey;
			target.Increment = Increment;
			target.Mean = Mean;
			target.Count = Count;
			target.Min = Min;
			target.Max = Max;
		}

		cache.Add(this, target);

		if (referenceModifier == ReferenceModifier.MapAllReferences)
		{
			target.EventCounter = EventCounter?.MapTo(target.EventCounter, referenceModifier, conds?.GetConditions(x => x.EventCounter), instanceFactory, cache)!;
		}
		else if (referenceModifier == ReferenceModifier.SetNull)
		{
			target.EventCounter = null!;
		}

		return target;
	}
}
