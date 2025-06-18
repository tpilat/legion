using Legion;
using Legion.Model.Mappers;

namespace Legion.ADF.Audit.Model;

public sealed partial class ApplicationEntryToken : Audit.AuditBaseEntity, Legion.Model.IEntity
{
	public static Audit.Model.ApplicationEntryToken? Map(
		Audit.Model.ApplicationEntryToken source,
		Audit.Model.ApplicationEntryToken? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Audit.Model.ApplicationEntryToken>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		Throw.IfArgumentNull(source);

		return source.MapTo(target, referenceModifier, conditions, instanceFactory, cache);
	}

	public Audit.Model.ApplicationEntryToken? Clone(
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Audit.Model.ApplicationEntryToken>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
		=> MapTo(target: null, referenceModifier, conditions, instanceFactory, cache);

	public Audit.Model.ApplicationEntryToken? MapTo(
		Audit.Model.ApplicationEntryToken? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Audit.Model.ApplicationEntryToken>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		cache ??= [];
		target ??= instanceFactory?.CreateInstance<Legion.ADF.Audit.Model.ApplicationEntryToken>(
				useActivatorIfNoFactoryFound: false,
				throwIfFactoryReturnsNull: true)
			?? new Legion.ADF.Audit.Model.ApplicationEntryToken();

		if (cache.TryGetValue(this, out var cached))
			return (Audit.Model.ApplicationEntryToken)cached;
			
		MappingConditions<Audit.Model.ApplicationEntryToken>? conds = null;
		if (conditions != null)
		{
			conds = new MappingConditions<Audit.Model.ApplicationEntryToken>();
			conditions.Invoke(conds);

			if (conds.CanMap(this, nameof(IdApplicationEntryToken)))
				target.IdApplicationEntryToken = IdApplicationEntryToken;
			if (conds.CanMap(this, nameof(Token)))
				target.Token = Token;
			if (conds.CanMap(this, nameof(SourceFilePath)))
				target.SourceFilePath = SourceFilePath;
			if (conds.CanMap(this, nameof(MethodInfo)))
				target.MethodInfo = MethodInfo;
			if (conds.CanMap(this, nameof(AggregateName)))
				target.AggregateName = AggregateName;
			if (conds.CanMap(this, nameof(Description)))
				target.Description = Description;
		}
		else
		{
			target.IdApplicationEntryToken = IdApplicationEntryToken;
			target.Token = Token;
			target.SourceFilePath = SourceFilePath;
			target.MethodInfo = MethodInfo;
			target.AggregateName = AggregateName;
			target.Description = Description;
		}

		cache.Add(this, target);

		if (referenceModifier == ReferenceModifier.MapAllReferences)
		{
			target._applicationEntries = MapperHelper.MapToList(ApplicationEntries, target._applicationEntries, ApplicationEntry.Map, referenceModifier, conds?.GetConditions(x => x.ApplicationEntries), instanceFactory, cache)!;
		}
		else if (referenceModifier == ReferenceModifier.SetNull)
		{
			target._applicationEntries = [];
		}

		return target;
	}
}
