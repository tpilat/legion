using Legion;
using Legion.Model.Mappers;

namespace Legion.ADF.Logs.Model;

public sealed partial class LogLevel : Logs.LogsBaseEntity, Legion.Model.IEntity
{
	public static Logs.Model.LogLevel? Map(
		Logs.Model.LogLevel source,
		Logs.Model.LogLevel? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Logs.Model.LogLevel>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		Throw.IfArgumentNull(source);

		return source.MapTo(target, referenceModifier, conditions, instanceFactory, cache);
	}

	public Logs.Model.LogLevel? Clone(
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Logs.Model.LogLevel>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
		=> MapTo(target: null, referenceModifier, conditions, instanceFactory, cache);

	public Logs.Model.LogLevel? MapTo(
		Logs.Model.LogLevel? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Logs.Model.LogLevel>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		cache ??= [];
		target ??= Legion.ADF.Logs.Model.LogLevel.DictionaryMap.Value[IdLogLevel];

		if (cache.TryGetValue(this, out var cached))
			return (Logs.Model.LogLevel)cached;
			
		MappingConditions<Logs.Model.LogLevel>? conds = null;
		if (conditions != null)
		{
			conds = new MappingConditions<Logs.Model.LogLevel>();
			conditions.Invoke(conds);
		}

		cache.Add(this, target);

		return target;
	}
}
