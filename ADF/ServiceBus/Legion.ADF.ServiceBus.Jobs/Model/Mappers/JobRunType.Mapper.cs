using Legion;
using Legion.Model.Mappers;

namespace Legion.ADF.ServiceBus.Jobs.Model;

public sealed partial class JobRunType : Jobs.JobsBaseEntity, Legion.Model.IEntity
{
	public static Jobs.Model.JobRunType? Map(
		Jobs.Model.JobRunType source,
		Jobs.Model.JobRunType? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Jobs.Model.JobRunType>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		Throw.IfArgumentNull(source);

		return source.MapTo(target, referenceModifier, conditions, instanceFactory, cache);
	}

	public Jobs.Model.JobRunType? Clone(
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Jobs.Model.JobRunType>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
		=> MapTo(target: null, referenceModifier, conditions, instanceFactory, cache);

	public Jobs.Model.JobRunType? MapTo(
		Jobs.Model.JobRunType? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Jobs.Model.JobRunType>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		cache ??= [];
		target ??= Legion.ADF.ServiceBus.Jobs.Model.JobRunType.DictionaryMap.Value[IdJobRunType];

		if (cache.TryGetValue(this, out var cached))
			return (Jobs.Model.JobRunType)cached;
			
		MappingConditions<Jobs.Model.JobRunType>? conds = null;
		if (conditions != null)
		{
			conds = new MappingConditions<Jobs.Model.JobRunType>();
			conditions.Invoke(conds);
		}

		cache.Add(this, target);

		if (referenceModifier == ReferenceModifier.MapAllReferences)
		{
			target._jobs = MapperHelper.MapToList(Jobs, target._jobs, Legion.ADF.ServiceBus.Jobs.Model.Job.Map, referenceModifier, conds?.GetConditions(x => x.Jobs), instanceFactory, cache)!;
		}
		else if (referenceModifier == ReferenceModifier.SetNull)
		{
			target._jobs = [];
		}

		return target;
	}
}
