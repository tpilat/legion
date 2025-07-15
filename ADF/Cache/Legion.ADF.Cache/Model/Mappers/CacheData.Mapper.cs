using Legion;
using Legion.Model.Mappers;

namespace Legion.ADF.Cache.Model;

public sealed partial class CacheData : Cache.CacheBaseEntity, Legion.Model.Concurrence.IConcurrent, Legion.Model.IEntity
{
	public static Cache.Model.CacheData? Map(
		Cache.Model.CacheData source,
		Cache.Model.CacheData? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Cache.Model.CacheData>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		Throw.IfArgumentNull(source);

		return source.MapTo(target, referenceModifier, conditions, instanceFactory, cache);
	}

	public Cache.Model.CacheData? Clone(
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Cache.Model.CacheData>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
		=> MapTo(target: null, referenceModifier, conditions, instanceFactory, cache);

	public Cache.Model.CacheData? MapTo(
		Cache.Model.CacheData? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Cache.Model.CacheData>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		cache ??= [];
		target ??= instanceFactory?.CreateInstance<Legion.ADF.Cache.Model.CacheData>(
				useActivatorIfNoFactoryFound: false,
				throwIfFactoryReturnsNull: true)
			?? new Legion.ADF.Cache.Model.CacheData();

		if (cache.TryGetValue(this, out var cached))
			return (Cache.Model.CacheData)cached;
			
		MappingConditions<Cache.Model.CacheData>? conds = null;
		if (conditions != null)
		{
			conds = new MappingConditions<Cache.Model.CacheData>();
			conditions.Invoke(conds);

			if (conds.CanMap(this, nameof(KeyHash)))
				target.KeyHash = KeyHash;
			if (conds.CanMap(this, nameof(ValueHash)))
				target.ValueHash = ValueHash;
			if (conds.CanMap(this, nameof(Key)))
				target.Key = Key;
			if (conds.CanMap(this, nameof(Value)))
				target.Value = Value;
			if (conds.CanMap(this, nameof(KeyPrefix450)))
				target.KeyPrefix450 = KeyPrefix450;
			if (conds.CanMap(this, nameof(ExpiresUtc)))
				target.ExpiresUtc = ExpiresUtc;
			if (conds.CanMap(this, nameof(SlidingTime)))
				target.SlidingTime = SlidingTime;
			if (conds.CanMap(this, nameof(CreatedUtc)))
				target.CreatedUtc = CreatedUtc;
			if (conds.CanMap(this, nameof(LastAccessedUtc)))
				target.LastAccessedUtc = LastAccessedUtc;
			if (conds.CanMap(this, nameof(RowVersion)))
				target.RowVersion = RowVersion;
		}
		else
		{
			target.KeyHash = KeyHash;
			target.ValueHash = ValueHash;
			target.Key = Key;
			target.Value = Value;
			target.KeyPrefix450 = KeyPrefix450;
			target.ExpiresUtc = ExpiresUtc;
			target.SlidingTime = SlidingTime;
			target.CreatedUtc = CreatedUtc;
			target.LastAccessedUtc = LastAccessedUtc;
			target.RowVersion = RowVersion;
		}

		cache.Add(this, target);

		return target;
	}
}
