using Legion;
using Legion.Model.Mappers;

namespace Legion.ADF.Logs.Model;

public sealed partial class EventCounter : Logs.LogsBaseEntity, Legion.Model.IEntity
{
	public static Logs.Model.EventCounter? Map(
		Logs.Model.EventCounter source,
		Logs.Model.EventCounter? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Logs.Model.EventCounter>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		Throw.IfArgumentNull(source);

		return source.MapTo(target, referenceModifier, conditions, instanceFactory, cache);
	}

	public Logs.Model.EventCounter? Clone(
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Logs.Model.EventCounter>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
		=> MapTo(target: null, referenceModifier, conditions, instanceFactory, cache);

	public Logs.Model.EventCounter? MapTo(
		Logs.Model.EventCounter? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Logs.Model.EventCounter>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		cache ??= [];
		target ??= instanceFactory?.CreateInstance<Legion.ADF.Logs.Model.EventCounter>(
				useActivatorIfNoFactoryFound: false,
				throwIfFactoryReturnsNull: true)
			?? new Legion.ADF.Logs.Model.EventCounter();

		if (cache.TryGetValue(this, out var cached))
			return (Logs.Model.EventCounter)cached;
			
		MappingConditions<Logs.Model.EventCounter>? conds = null;
		if (conditions != null)
		{
			conds = new MappingConditions<Logs.Model.EventCounter>();
			conditions.Invoke(conds);

			if (conds.CanMap(this, nameof(IdEventCounter)))
				target.IdEventCounter = IdEventCounter;
			if (conds.CanMap(this, nameof(IdEventCounterCategory)))
				target.IdEventCounterCategory = IdEventCounterCategory;
			if (conds.CanMap(this, nameof(Code)))
				target.Code = Code;
			if (conds.CanMap(this, nameof(Name)))
				target.Name = Name;
			if (conds.CanMap(this, nameof(DisplayName)))
				target.DisplayName = DisplayName;
			if (conds.CanMap(this, nameof(CounterType)))
				target.CounterType = CounterType;
			if (conds.CanMap(this, nameof(DisplayRateTimeScale)))
				target.DisplayRateTimeScale = DisplayRateTimeScale;
			if (conds.CanMap(this, nameof(Metadata)))
				target.Metadata = Metadata;
			if (conds.CanMap(this, nameof(DisplayUnits)))
				target.DisplayUnits = DisplayUnits;
		}
		else
		{
			target.IdEventCounter = IdEventCounter;
			target.IdEventCounterCategory = IdEventCounterCategory;
			target.Code = Code;
			target.Name = Name;
			target.DisplayName = DisplayName;
			target.CounterType = CounterType;
			target.DisplayRateTimeScale = DisplayRateTimeScale;
			target.Metadata = Metadata;
			target.DisplayUnits = DisplayUnits;
		}

		cache.Add(this, target);

		if (referenceModifier == ReferenceModifier.MapAllReferences)
		{
			target.EventCounterCategory = EventCounterCategory?.MapTo(target.EventCounterCategory, referenceModifier, conds?.GetConditions(x => x.EventCounterCategory), instanceFactory, cache)!;
			target._eventCounterDatas = MapperHelper.MapToList(EventCounterDatas, target._eventCounterDatas, EventCounterData.Map, referenceModifier, conds?.GetConditions(x => x.EventCounterDatas), instanceFactory, cache)!;
		}
		else if (referenceModifier == ReferenceModifier.SetNull)
		{
			target.EventCounterCategory = null!;
			target._eventCounterDatas = [];
		}

		return target;
	}
}
