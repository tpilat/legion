using Legion;
using Legion.Model.Mappers;

namespace Legion.ADF.ServiceBus.Jobs.Model;

public sealed partial class JobMessageType : Jobs.JobsBaseEntity, Legion.Model.IEntity
{
	public static Jobs.Model.JobMessageType? Map(
		Jobs.Model.JobMessageType source,
		Jobs.Model.JobMessageType? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Jobs.Model.JobMessageType>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		Throw.IfArgumentNull(source);

		return source.MapTo(target, referenceModifier, conditions, instanceFactory, cache);
	}

	public Jobs.Model.JobMessageType? Clone(
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Jobs.Model.JobMessageType>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
		=> MapTo(target: null, referenceModifier, conditions, instanceFactory, cache);

	public Jobs.Model.JobMessageType? MapTo(
		Jobs.Model.JobMessageType? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Jobs.Model.JobMessageType>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		cache ??= [];
		target ??= Legion.ADF.ServiceBus.Jobs.Model.JobMessageType.DictionaryMap.Value[IdJobMessageType];

		if (cache.TryGetValue(this, out var cached))
			return (Jobs.Model.JobMessageType)cached;
			
		MappingConditions<Jobs.Model.JobMessageType>? conds = null;
		if (conditions != null)
		{
			conds = new MappingConditions<Jobs.Model.JobMessageType>();
			conditions.Invoke(conds);
		}

		cache.Add(this, target);

		if (referenceModifier == ReferenceModifier.MapAllReferences)
		{
			target._jobMessages = MapperHelper.MapToList(JobMessages, target._jobMessages, Legion.ADF.ServiceBus.Jobs.Model.JobMessage.Map, referenceModifier, conds?.GetConditions(x => x.JobMessages), instanceFactory, cache)!;
		}
		else if (referenceModifier == ReferenceModifier.SetNull)
		{
			target._jobMessages = [];
		}

		return target;
	}
}
