using Legion;
using Legion.Model.Mappers;

namespace Legion.ADF.Cache.Model;

public sealed partial class DistributedLock : Cache.CacheBaseEntity, Legion.Model.IEntity
{
	public static Cache.Model.DistributedLock? Map(
		Cache.Model.DistributedLock source,
		Cache.Model.DistributedLock? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Cache.Model.DistributedLock>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		Throw.IfArgumentNull(source);

		return source.MapTo(target, referenceModifier, conditions, instanceFactory, cache);
	}

	public Cache.Model.DistributedLock? Clone(
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Cache.Model.DistributedLock>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
		=> MapTo(target: null, referenceModifier, conditions, instanceFactory, cache);

	public Cache.Model.DistributedLock? MapTo(
		Cache.Model.DistributedLock? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Cache.Model.DistributedLock>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		cache ??= [];
		target ??= instanceFactory?.CreateInstance<Legion.ADF.Cache.Model.DistributedLock>(
				useActivatorIfNoFactoryFound: false,
				throwIfFactoryReturnsNull: true)
			?? new Legion.ADF.Cache.Model.DistributedLock();

		if (cache.TryGetValue(this, out var cached))
			return (Cache.Model.DistributedLock)cached;
			
		MappingConditions<Cache.Model.DistributedLock>? conds = null;
		if (conditions != null)
		{
			conds = new MappingConditions<Cache.Model.DistributedLock>();
			conditions.Invoke(conds);

			if (conds.CanMap(this, nameof(KeyHash)))
				target.KeyHash = KeyHash;
			if (conds.CanMap(this, nameof(LockKey)))
				target.LockKey = LockKey;
			if (conds.CanMap(this, nameof(LockId)))
				target.LockId = LockId;
			if (conds.CanMap(this, nameof(Metadata)))
				target.Metadata = Metadata;
			if (conds.CanMap(this, nameof(CreatedUtc)))
				target.CreatedUtc = CreatedUtc;
			if (conds.CanMap(this, nameof(ExpiresUtc)))
				target.ExpiresUtc = ExpiresUtc;
		}
		else
		{
			target.KeyHash = KeyHash;
			target.LockKey = LockKey;
			target.LockId = LockId;
			target.Metadata = Metadata;
			target.CreatedUtc = CreatedUtc;
			target.ExpiresUtc = ExpiresUtc;
		}

		cache.Add(this, target);

		return target;
	}
}
