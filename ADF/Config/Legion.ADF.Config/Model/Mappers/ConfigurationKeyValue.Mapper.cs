using Legion;
using Legion.Model.Mappers;

namespace Legion.ADF.Config.Model;

public sealed partial class ConfigurationKeyValue : Config.ConfigBaseEntity, Legion.Model.Audit.ISelfAuditableEntity, Legion.Model.Concurrence.IConcurrent, Legion.Model.Audit.IAuditableEntity, Legion.Model.IEntity
{
	public static Config.Model.ConfigurationKeyValue? Map(
		Config.Model.ConfigurationKeyValue source,
		Config.Model.ConfigurationKeyValue? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Config.Model.ConfigurationKeyValue>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		Throw.IfArgumentNull(source);

		return source.MapTo(target, referenceModifier, conditions, instanceFactory, cache);
	}

	public Config.Model.ConfigurationKeyValue? Clone(
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Config.Model.ConfigurationKeyValue>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
		=> MapTo(target: null, referenceModifier, conditions, instanceFactory, cache);

	public Config.Model.ConfigurationKeyValue? MapTo(
		Config.Model.ConfigurationKeyValue? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Config.Model.ConfigurationKeyValue>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		cache ??= [];
		target ??= instanceFactory?.CreateInstance<Legion.ADF.Config.Model.ConfigurationKeyValue>(
				useActivatorIfNoFactoryFound: false,
				throwIfFactoryReturnsNull: true)
			?? new Legion.ADF.Config.Model.ConfigurationKeyValue();

		if (cache.TryGetValue(this, out var cached))
			return (Config.Model.ConfigurationKeyValue)cached;
			
		MappingConditions<Config.Model.ConfigurationKeyValue>? conds = null;
		if (conditions != null)
		{
			conds = new MappingConditions<Config.Model.ConfigurationKeyValue>();
			conditions.Invoke(conds);

			if (conds.CanMap(this, nameof(IdConfigurationKeyValue)))
				target.IdConfigurationKeyValue = IdConfigurationKeyValue;
			if (conds.CanMap(this, nameof(Key)))
				target.Key = Key;
			if (conds.CanMap(this, nameof(Value)))
				target.Value = Value;
			if (conds.CanMap(this, nameof(AuditCreatedUtc)))
				target.AuditCreatedUtc = AuditCreatedUtc;
			if (conds.CanMap(this, nameof(AuditModifiedUtc)))
				target.AuditModifiedUtc = AuditModifiedUtc;
			if (conds.CanMap(this, nameof(IdAuditCreatedBy)))
				target.IdAuditCreatedBy = IdAuditCreatedBy;
			if (conds.CanMap(this, nameof(IdAuditModifiedBy)))
				target.IdAuditModifiedBy = IdAuditModifiedBy;
			if (conds.CanMap(this, nameof(ConcurrencyToken)))
				target.ConcurrencyToken = ConcurrencyToken;
		}
		else
		{
			target.IdConfigurationKeyValue = IdConfigurationKeyValue;
			target.Key = Key;
			target.Value = Value;
			target.AuditCreatedUtc = AuditCreatedUtc;
			target.AuditModifiedUtc = AuditModifiedUtc;
			target.IdAuditCreatedBy = IdAuditCreatedBy;
			target.IdAuditModifiedBy = IdAuditModifiedBy;
			target.ConcurrencyToken = ConcurrencyToken;
		}

		cache.Add(this, target);

		return target;
	}
}
