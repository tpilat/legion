using Legion.Validation;

namespace Legion.ADF.Auth.Model;

public sealed partial class LoginProvider : Auth.AuthBaseEntity, Legion.Model.IEntity
{
	private List<Auth.Model.ExternalLogin> _externalLogins;
	private List<Auth.Model.UserToken> _userTokens;

	public static IValidator<LoginProvider> DefaultDBValidator { get; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdLoginProvider { get; private set; }

	/// <summary>
	/// Database DataType: varchar(128) NOT NULL
	/// </summary>
	public string Code { get; private set; }

	/// <summary>
	/// Database DataType: varchar(128) NOT NULL
	/// </summary>
	public string Name { get; private set; }

	/// <summary>
	/// Database DataType: timestamp with time zone NULL
	/// </summary>
	public DateTime? DisabledUtc { get; private set; }


	/// <summary>
	/// N:_1 Auth.Model.ExternalLogin.IdLoginProvider | FK_ExternalLogin_IdLoginProvider
	/// </summary>
	public IReadOnlyList<Auth.Model.ExternalLogin> ExternalLogins => _externalLogins;

	/// <summary>
	/// N:_1 Auth.Model.UserToken.IdLoginProvider | FK_UserToken_IdLoginProvider
	/// </summary>
	public IReadOnlyList<Auth.Model.UserToken> UserTokens => _userTokens;

	private LoginProvider()
	{
		_externalLogins = [];
		_userTokens = [];
	}

	static LoginProvider()
	{
		DefaultDBValidator = SetDBValidatorRules(new ValidatorBuilder<LoginProvider>()).Build();
	}

	public Dictionary<string, object?> ToDictionary()
		=> new()
		{
			{ nameof(IdLoginProvider), IdLoginProvider },
			{ nameof(Code), Code },
			{ nameof(Name), Name },
			{ nameof(DisabledUtc), DisabledUtc },
		};

	public void TrimStringValuesToFitDatabaseMaxLengths(string? postfix = null)
	{
		Code = Legion.Text.StringHelper.TrimToFitMaxLength(Code, 128, postfix);
		Name = Legion.Text.StringHelper.TrimToFitMaxLength(Name, 128, postfix);
	}

	public override string? GetPrimaryKeyValue()
	{
		return IdLoginProvider.ToString();
	}

	public override string? ToString()
	{
		return IdLoginProvider.ToString();
	}

	public static ValidatorBuilder<LoginProvider> SetDBValidatorRules(ValidatorBuilder<LoginProvider> builder)
		=> builder
			.ForProperty(x => x.IdLoginProvider, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.Code, v => v.NotDefaultOrEmpty().MaxLength(128))
			.ForProperty(x => x.Name, v => v.NotDefaultOrEmpty().MaxLength(128))
		;
}
