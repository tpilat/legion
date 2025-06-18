using Legion;
using Legion.Model.Mappers;

namespace Legion.ADF.Auth.Model;

public sealed partial class ExternalLogin : Auth.AuthBaseEntity, Legion.Model.IEntity
{
	public static Auth.Model.ExternalLogin? Map(
		Auth.Model.ExternalLogin source,
		Auth.Model.ExternalLogin? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Auth.Model.ExternalLogin>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		Throw.IfArgumentNull(source);

		return source.MapTo(target, referenceModifier, conditions, instanceFactory, cache);
	}

	public Auth.Model.ExternalLogin? Clone(
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Auth.Model.ExternalLogin>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
		=> MapTo(target: null, referenceModifier, conditions, instanceFactory, cache);

	public Auth.Model.ExternalLogin? MapTo(
		Auth.Model.ExternalLogin? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Auth.Model.ExternalLogin>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		cache ??= [];
		target ??= instanceFactory?.CreateInstance<Legion.ADF.Auth.Model.ExternalLogin>(
				useActivatorIfNoFactoryFound: false,
				throwIfFactoryReturnsNull: true)
			?? new Legion.ADF.Auth.Model.ExternalLogin();

		if (cache.TryGetValue(this, out var cached))
			return (Auth.Model.ExternalLogin)cached;
			
		MappingConditions<Auth.Model.ExternalLogin>? conds = null;
		if (conditions != null)
		{
			conds = new MappingConditions<Auth.Model.ExternalLogin>();
			conditions.Invoke(conds);

			if (conds.CanMap(this, nameof(IdExternalLogin)))
				target.IdExternalLogin = IdExternalLogin;
			if (conds.CanMap(this, nameof(IdLoginProvider)))
				target.IdLoginProvider = IdLoginProvider;
			if (conds.CanMap(this, nameof(IdUser)))
				target.IdUser = IdUser;
			if (conds.CanMap(this, nameof(ExternalUserIdentifier)))
				target.ExternalUserIdentifier = ExternalUserIdentifier;
			if (conds.CanMap(this, nameof(Data)))
				target.Data = Data;
			if (conds.CanMap(this, nameof(CreatedUtc)))
				target.CreatedUtc = CreatedUtc;
			if (conds.CanMap(this, nameof(ValidToUtc)))
				target.ValidToUtc = ValidToUtc;
			if (conds.CanMap(this, nameof(LastAccessUtc)))
				target.LastAccessUtc = LastAccessUtc;
			if (conds.CanMap(this, nameof(RemoteIP)))
				target.RemoteIP = RemoteIP;
		}
		else
		{
			target.IdExternalLogin = IdExternalLogin;
			target.IdLoginProvider = IdLoginProvider;
			target.IdUser = IdUser;
			target.ExternalUserIdentifier = ExternalUserIdentifier;
			target.Data = Data;
			target.CreatedUtc = CreatedUtc;
			target.ValidToUtc = ValidToUtc;
			target.LastAccessUtc = LastAccessUtc;
			target.RemoteIP = RemoteIP;
		}

		cache.Add(this, target);

		if (referenceModifier == ReferenceModifier.MapAllReferences)
		{
			target.LoginProvider = LoginProvider?.MapTo(target.LoginProvider, referenceModifier, conds?.GetConditions(x => x.LoginProvider), instanceFactory, cache)!;
			target.User = User?.MapTo(target.User, referenceModifier, conds?.GetConditions(x => x.User), instanceFactory, cache)!;
		}
		else if (referenceModifier == ReferenceModifier.SetNull)
		{
			target.LoginProvider = null!;
			target.User = null!;
		}

		return target;
	}
}
