using Legion;
using Legion.Model.Mappers;

namespace Legion.ADF.Logs.Model;

public sealed partial class EventCounterCategory : Logs.LogsBaseEntity, Legion.Model.IEntity
{
	public static Logs.Model.EventCounterCategory? Map(
		Logs.Model.EventCounterCategory source,
		Logs.Model.EventCounterCategory? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Logs.Model.EventCounterCategory>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		Throw.IfArgumentNull(source);

		return source.MapTo(target, referenceModifier, conditions, instanceFactory, cache);
	}

	public Logs.Model.EventCounterCategory? Clone(
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Logs.Model.EventCounterCategory>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
		=> MapTo(target: null, referenceModifier, conditions, instanceFactory, cache);

	public Logs.Model.EventCounterCategory? MapTo(
		Logs.Model.EventCounterCategory? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Logs.Model.EventCounterCategory>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		cache ??= [];
		target ??= instanceFactory?.CreateInstance<Legion.ADF.Logs.Model.EventCounterCategory>(
				useActivatorIfNoFactoryFound: false,
				throwIfFactoryReturnsNull: true)
			?? new Legion.ADF.Logs.Model.EventCounterCategory();

		if (cache.TryGetValue(this, out var cached))
			return (Logs.Model.EventCounterCategory)cached;
			
		MappingConditions<Logs.Model.EventCounterCategory>? conds = null;
		if (conditions != null)
		{
			conds = new MappingConditions<Logs.Model.EventCounterCategory>();
			conditions.Invoke(conds);

			if (conds.CanMap(this, nameof(IdEventCounterCategory)))
				target.IdEventCounterCategory = IdEventCounterCategory;
			if (conds.CanMap(this, nameof(Source)))
				target.Source = Source;
			if (conds.CanMap(this, nameof(DisplayName)))
				target.DisplayName = DisplayName;
		}
		else
		{
			target.IdEventCounterCategory = IdEventCounterCategory;
			target.Source = Source;
			target.DisplayName = DisplayName;
		}

		cache.Add(this, target);

		if (referenceModifier == ReferenceModifier.MapAllReferences)
		{
			target._eventCounters = MapperHelper.MapToList(EventCounters, target._eventCounters, EventCounter.Map, referenceModifier, conds?.GetConditions(x => x.EventCounters), instanceFactory, cache)!;
		}
		else if (referenceModifier == ReferenceModifier.SetNull)
		{
			target._eventCounters = [];
		}

		return target;
	}
}
