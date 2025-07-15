namespace Legion.ADF.Auth.Model;

public sealed partial class ExternalLogin : Auth.AuthBaseEntity, Legion.Model.IEntity
{
	internal static IResult<ExternalLogin> CreateExternalLogin(
		IScopeContext scopeContext,
		Guid idLoginProvider,
		Guid idUser,
		string externalUserIdentifier,
		string? data,
		string? remoteIP,
		DateTime? validToUtc = null)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<ExternalLogin>();

		var externalLogin = new ExternalLogin
		{
			__IsNewObject = true,
			IdExternalLogin = GlobalContext.Instance.NewGuid(),
			IdLoginProvider = idLoginProvider,
			IdUser = idUser,
			ExternalUserIdentifier = externalUserIdentifier,
			Data = data,
			CreatedUtc = GlobalContext.Instance.UtcNow,
			ValidToUtc = validToUtc ?? DateTime.MaxValue,
			LastAccessUtc = null,
			RemoteIP = remoteIP
		};

		var validationResult =
			DefaultDBValidator
				.Validate(externalLogin);

		if (result.MergeHasError(scopeContext.CreateNew(), validationResult, true))
			return result.Build();

		return result.WithData(externalLogin).Build();
	}
}
