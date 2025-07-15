namespace Legion.ADF.Auth.Model;

public sealed partial class UserToken : Auth.AuthBaseEntity, Legion.Model.IEntity
{
	internal static IResult<UserToken> CreateUserToken(
		IScopeContext scopeContext,
		Guid idLoginProvider,
		Guid idUser,
		string name,
		string? value,
		string? data,
		string? remoteIP,
		DateTime? validToUtc = null)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<UserToken>();

		var userToken = new UserToken
		{
			__IsNewObject = true,
			IdUserToken = GlobalContext.Instance.NewGuid(),
			IdLoginProvider = idLoginProvider,
			IdUser = idUser,
			Name = name,
			Value = value,
			Data = data,
			CreatedUtc = GlobalContext.Instance.UtcNow,
			ModifiedUtc = null,
			ValidToUtc = validToUtc ?? DateTime.MaxValue,
			LastAccessUtc = null,
			RemoteIP = remoteIP
		};

		var validationResult =
			DefaultDBValidator
				.Validate(userToken);

		if (result.MergeHasError(scopeContext.CreateNew(), validationResult, true))
			return result.Build();

		return result.WithData(userToken).Build();
	}

	internal IResult SetValue(
		IScopeContext scopeContext,
		string? value,
		DateTime? validToUtc = null)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, value))
			return result.Build();

		Value = value;

		if (!__IsNewObject)
		{
			ModifiedUtc = GlobalContext.Instance.UtcNow;
		}

		if (validToUtc.HasValue)
			ValidToUtc = validToUtc.Value;

		var validationResult =
			DefaultDBValidator
				.Validate(this);

		if (result.MergeHasError(scopeContext.CreateNew(), validationResult, true))
			return result.Build();

		return result.Build();
	}
}
