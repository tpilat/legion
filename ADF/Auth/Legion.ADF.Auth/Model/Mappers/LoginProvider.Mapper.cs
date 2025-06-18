using Legion;
using Legion.Model.Mappers;

namespace Legion.ADF.Auth.Model;

public sealed partial class LoginProvider : Auth.AuthBaseEntity, Legion.Model.IEntity
{
	public static Auth.Model.LoginProvider? Map(
		Auth.Model.LoginProvider source,
		Auth.Model.LoginProvider? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Auth.Model.LoginProvider>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		Throw.IfArgumentNull(source);

		return source.MapTo(target, referenceModifier, conditions, instanceFactory, cache);
	}

	public Auth.Model.LoginProvider? Clone(
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Auth.Model.LoginProvider>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
		=> MapTo(target: null, referenceModifier, conditions, instanceFactory, cache);

	public Auth.Model.LoginProvider? MapTo(
		Auth.Model.LoginProvider? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Auth.Model.LoginProvider>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		cache ??= [];
		target ??= instanceFactory?.CreateInstance<Legion.ADF.Auth.Model.LoginProvider>(
				useActivatorIfNoFactoryFound: false,
				throwIfFactoryReturnsNull: true)
			?? new Legion.ADF.Auth.Model.LoginProvider();

		if (cache.TryGetValue(this, out var cached))
			return (Auth.Model.LoginProvider)cached;
			
		MappingConditions<Auth.Model.LoginProvider>? conds = null;
		if (conditions != null)
		{
			conds = new MappingConditions<Auth.Model.LoginProvider>();
			conditions.Invoke(conds);

			if (conds.CanMap(this, nameof(IdLoginProvider)))
				target.IdLoginProvider = IdLoginProvider;
			if (conds.CanMap(this, nameof(Code)))
				target.Code = Code;
			if (conds.CanMap(this, nameof(Name)))
				target.Name = Name;
			if (conds.CanMap(this, nameof(DisabledUtc)))
				target.DisabledUtc = DisabledUtc;
		}
		else
		{
			target.IdLoginProvider = IdLoginProvider;
			target.Code = Code;
			target.Name = Name;
			target.DisabledUtc = DisabledUtc;
		}

		cache.Add(this, target);

		if (referenceModifier == ReferenceModifier.MapAllReferences)
		{
			target._externalLogins = MapperHelper.MapToList(ExternalLogins, target._externalLogins, ExternalLogin.Map, referenceModifier, conds?.GetConditions(x => x.ExternalLogins), instanceFactory, cache)!;
			target._userTokens = MapperHelper.MapToList(UserTokens, target._userTokens, UserToken.Map, referenceModifier, conds?.GetConditions(x => x.UserTokens), instanceFactory, cache)!;
		}
		else if (referenceModifier == ReferenceModifier.SetNull)
		{
			target._externalLogins = [];
			target._userTokens = [];
		}

		return target;
	}
}
