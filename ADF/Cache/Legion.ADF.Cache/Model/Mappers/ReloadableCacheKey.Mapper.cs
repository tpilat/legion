using Legion;
using Legion.Model.Mappers;

namespace Legion.ADF.Cache.Model;

public sealed partial class ReloadableCacheKey : Cache.CacheBaseEntity, Legion.Model.IEntity
{
	public static Cache.Model.ReloadableCacheKey? Map(
		Cache.Model.ReloadableCacheKey source,
		Cache.Model.ReloadableCacheKey? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Cache.Model.ReloadableCacheKey>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		Throw.IfArgumentNull(source);

		return source.MapTo(target, referenceModifier, conditions, instanceFactory, cache);
	}

	public Cache.Model.ReloadableCacheKey? Clone(
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Cache.Model.ReloadableCacheKey>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
		=> MapTo(target: null, referenceModifier, conditions, instanceFactory, cache);

	public Cache.Model.ReloadableCacheKey? MapTo(
		Cache.Model.ReloadableCacheKey? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Cache.Model.ReloadableCacheKey>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		cache ??= [];
		target ??= instanceFactory?.CreateInstance<Legion.ADF.Cache.Model.ReloadableCacheKey>(
				useActivatorIfNoFactoryFound: false,
				throwIfFactoryReturnsNull: true)
			?? new Legion.ADF.Cache.Model.ReloadableCacheKey();

		if (cache.TryGetValue(this, out var cached))
			return (Cache.Model.ReloadableCacheKey)cached;
			
		MappingConditions<Cache.Model.ReloadableCacheKey>? conds = null;
		if (conditions != null)
		{
			conds = new MappingConditions<Cache.Model.ReloadableCacheKey>();
			conditions.Invoke(conds);

			if (conds.CanMap(this, nameof(IdReloadableCacheKey)))
				target.IdReloadableCacheKey = IdReloadableCacheKey;
			if (conds.CanMap(this, nameof(Key)))
				target.Key = Key;
			if (conds.CanMap(this, nameof(Tags)))
				target.Tags = Tags?.ToList();
			if (conds.CanMap(this, nameof(CreatedUtc)))
				target.CreatedUtc = CreatedUtc;
			if (conds.CanMap(this, nameof(ReloadAtUtc)))
				target.ReloadAtUtc = ReloadAtUtc;
		}
		else
		{
			target.IdReloadableCacheKey = IdReloadableCacheKey;
			target.Key = Key;
			target.Tags = Tags?.ToList();
			target.CreatedUtc = CreatedUtc;
			target.ReloadAtUtc = ReloadAtUtc;
		}

		cache.Add(this, target);

		return target;
	}
}
