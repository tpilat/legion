using Legion;
using Legion.Model.Mappers;

namespace Legion.ADF.Logs.Model;

public sealed partial class RemoteSystem : Logs.LogsBaseEntity, Legion.Model.IEntity
{
	public static Logs.Model.RemoteSystem? Map(
		Logs.Model.RemoteSystem source,
		Logs.Model.RemoteSystem? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Logs.Model.RemoteSystem>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		Throw.IfArgumentNull(source);

		return source.MapTo(target, referenceModifier, conditions, instanceFactory, cache);
	}

	public Logs.Model.RemoteSystem? Clone(
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Logs.Model.RemoteSystem>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
		=> MapTo(target: null, referenceModifier, conditions, instanceFactory, cache);

	public Logs.Model.RemoteSystem? MapTo(
		Logs.Model.RemoteSystem? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Logs.Model.RemoteSystem>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		cache ??= [];
		target ??= instanceFactory?.CreateInstance<Legion.ADF.Logs.Model.RemoteSystem>(
				useActivatorIfNoFactoryFound: false,
				throwIfFactoryReturnsNull: true)
			?? new Legion.ADF.Logs.Model.RemoteSystem();

		if (cache.TryGetValue(this, out var cached))
			return (Logs.Model.RemoteSystem)cached;
			
		MappingConditions<Logs.Model.RemoteSystem>? conds = null;
		if (conditions != null)
		{
			conds = new MappingConditions<Logs.Model.RemoteSystem>();
			conditions.Invoke(conds);

			if (conds.CanMap(this, nameof(IdRemoteSystem)))
				target.IdRemoteSystem = IdRemoteSystem;
			if (conds.CanMap(this, nameof(Code)))
				target.Code = Code;
			if (conds.CanMap(this, nameof(Name)))
				target.Name = Name;
		}
		else
		{
			target.IdRemoteSystem = IdRemoteSystem;
			target.Code = Code;
			target.Name = Name;
		}

		cache.Add(this, target);

		if (referenceModifier == ReferenceModifier.MapAllReferences)
		{
			target._localRequests = MapperHelper.MapToList(LocalRequests, target._localRequests, LocalRequest.Map, referenceModifier, conds?.GetConditions(x => x.LocalRequests), instanceFactory, cache)!;
			target._remoteRequests = MapperHelper.MapToList(RemoteRequests, target._remoteRequests, RemoteRequest.Map, referenceModifier, conds?.GetConditions(x => x.RemoteRequests), instanceFactory, cache)!;
		}
		else if (referenceModifier == ReferenceModifier.SetNull)
		{
			target._localRequests = [];
			target._remoteRequests = [];
		}

		return target;
	}
}
