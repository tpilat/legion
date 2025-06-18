using Legion;
using Legion.Model.Mappers;

namespace Legion.ADF.Config.Model;

public sealed partial class ConfigurationClass : Config.ConfigBaseEntity, Legion.Model.IEntity
{
	public static Config.Model.ConfigurationClass? Map(
		Config.Model.ConfigurationClass source,
		Config.Model.ConfigurationClass? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Config.Model.ConfigurationClass>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		Throw.IfArgumentNull(source);

		return source.MapTo(target, referenceModifier, conditions, instanceFactory, cache);
	}

	public Config.Model.ConfigurationClass? Clone(
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Config.Model.ConfigurationClass>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
		=> MapTo(target: null, referenceModifier, conditions, instanceFactory, cache);

	public Config.Model.ConfigurationClass? MapTo(
		Config.Model.ConfigurationClass? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Config.Model.ConfigurationClass>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		cache ??= [];
		target ??= instanceFactory?.CreateInstance<Legion.ADF.Config.Model.ConfigurationClass>(
				useActivatorIfNoFactoryFound: false,
				throwIfFactoryReturnsNull: true)
			?? new Legion.ADF.Config.Model.ConfigurationClass();

		if (cache.TryGetValue(this, out var cached))
			return (Config.Model.ConfigurationClass)cached;
			
		MappingConditions<Config.Model.ConfigurationClass>? conds = null;
		if (conditions != null)
		{
			conds = new MappingConditions<Config.Model.ConfigurationClass>();
			conditions.Invoke(conds);

			if (conds.CanMap(this, nameof(IdConfigurationClass)))
				target.IdConfigurationClass = IdConfigurationClass;
			if (conds.CanMap(this, nameof(RootPath)))
				target.RootPath = RootPath;
			if (conds.CanMap(this, nameof(DisplayName)))
				target.DisplayName = DisplayName;
			if (conds.CanMap(this, nameof(Class)))
				target.Class = Class;
		}
		else
		{
			target.IdConfigurationClass = IdConfigurationClass;
			target.RootPath = RootPath;
			target.DisplayName = DisplayName;
			target.Class = Class;
		}

		cache.Add(this, target);

		return target;
	}
}
