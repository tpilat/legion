using Legion;
using Legion.Model.Mappers;

namespace Legion.ADF.Auth.Model;

public sealed partial class UserToken : Auth.AuthBaseEntity, Legion.Model.IEntity
{
	public static Auth.Model.UserToken? Map(
		Auth.Model.UserToken source,
		Auth.Model.UserToken? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Auth.Model.UserToken>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		Throw.IfArgumentNull(source);

		return source.MapTo(target, referenceModifier, conditions, instanceFactory, cache);
	}

	public Auth.Model.UserToken? Clone(
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Auth.Model.UserToken>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
		=> MapTo(target: null, referenceModifier, conditions, instanceFactory, cache);

	public Auth.Model.UserToken? MapTo(
		Auth.Model.UserToken? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Auth.Model.UserToken>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		cache ??= [];
		target ??= instanceFactory?.CreateInstance<Legion.ADF.Auth.Model.UserToken>(
				useActivatorIfNoFactoryFound: false,
				throwIfFactoryReturnsNull: true)
			?? new Legion.ADF.Auth.Model.UserToken();

		if (cache.TryGetValue(this, out var cached))
			return (Auth.Model.UserToken)cached;
			
		MappingConditions<Auth.Model.UserToken>? conds = null;
		if (conditions != null)
		{
			conds = new MappingConditions<Auth.Model.UserToken>();
			conditions.Invoke(conds);

			if (conds.CanMap(this, nameof(IdUserToken)))
				target.IdUserToken = IdUserToken;
			if (conds.CanMap(this, nameof(IdLoginProvider)))
				target.IdLoginProvider = IdLoginProvider;
			if (conds.CanMap(this, nameof(IdUser)))
				target.IdUser = IdUser;
			if (conds.CanMap(this, nameof(Name)))
				target.Name = Name;
			if (conds.CanMap(this, nameof(Value)))
				target.Value = Value;
			if (conds.CanMap(this, nameof(Data)))
				target.Data = Data;
			if (conds.CanMap(this, nameof(CreatedUtc)))
				target.CreatedUtc = CreatedUtc;
			if (conds.CanMap(this, nameof(ModifiedUtc)))
				target.ModifiedUtc = ModifiedUtc;
			if (conds.CanMap(this, nameof(ValidToUtc)))
				target.ValidToUtc = ValidToUtc;
			if (conds.CanMap(this, nameof(LastAccessUtc)))
				target.LastAccessUtc = LastAccessUtc;
			if (conds.CanMap(this, nameof(RemoteIP)))
				target.RemoteIP = RemoteIP;
		}
		else
		{
			target.IdUserToken = IdUserToken;
			target.IdLoginProvider = IdLoginProvider;
			target.IdUser = IdUser;
			target.Name = Name;
			target.Value = Value;
			target.Data = Data;
			target.CreatedUtc = CreatedUtc;
			target.ModifiedUtc = ModifiedUtc;
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
