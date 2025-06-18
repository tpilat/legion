using Legion.Validation;

namespace Legion.ADF.Auth.Model;

public sealed partial class UserToken : Auth.AuthBaseEntity, Legion.Model.IEntity
{
	public static IValidator<UserToken> DefaultDBValidator { get; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdUserToken { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL | Auth.Model.LoginProvider.LoginProvider | FK_UserToken_IdLoginProvider
	/// </summary>
	public Guid IdLoginProvider { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL | Auth.Model.User.User | FK_UserToken_IdUser
	/// </summary>
	public Guid IdUser { get; private set; }

	/// <summary>
	/// Database DataType: text NOT NULL
	/// </summary>
	public string Name { get; private set; }

	/// <summary>
	/// Database DataType: text NOT NULL
	/// </summary>
	public string Value { get; private set; }

	/// <summary>
	/// Database DataType: jsonb NULL
	/// </summary>
	public string? Data { get; private set; }

	/// <summary>
	/// Database DataType: timestamp with time zone NOT NULL
	/// </summary>
	public DateTime CreatedUtc { get; private set; }

	/// <summary>
	/// Database DataType: timestamp with time zone NULL
	/// </summary>
	public DateTime? ModifiedUtc { get; private set; }

	/// <summary>
	/// Database DataType: timestamp with time zone NOT NULL
	/// </summary>
	public DateTime ValidToUtc { get; private set; }

	/// <summary>
	/// Database DataType: timestamp with time zone NULL
	/// </summary>
	public DateTime? LastAccessUtc { get; private set; }

	/// <summary>
	/// Database DataType: varchar(64) NULL
	/// </summary>
	public string? RemoteIP { get; private set; }


	/// <summary>
	/// _1:N Guid IdLoginProvider | FK_UserToken_IdLoginProvider
	/// </summary>
	public Auth.Model.LoginProvider LoginProvider { get; private set; }

	/// <summary>
	/// _1:N Guid IdUser | FK_UserToken_IdUser
	/// </summary>
	public Auth.Model.User User { get; private set; }

	private UserToken()
	{
	}

	static UserToken()
	{
		DefaultDBValidator = SetDBValidatorRules(new ValidatorBuilder<UserToken>()).Build();
	}

	public Dictionary<string, object?> ToDictionary()
		=> new()
		{
			{ nameof(IdUserToken), IdUserToken },
			{ nameof(IdLoginProvider), IdLoginProvider },
			{ nameof(IdUser), IdUser },
			{ nameof(Name), Name },
			{ nameof(Value), Value },
			{ nameof(Data), Data },
			{ nameof(CreatedUtc), CreatedUtc },
			{ nameof(ModifiedUtc), ModifiedUtc },
			{ nameof(ValidToUtc), ValidToUtc },
			{ nameof(LastAccessUtc), LastAccessUtc },
			{ nameof(RemoteIP), RemoteIP },
		};

	public void TrimStringValuesToFitDatabaseMaxLengths(string? postfix = null)
	{
		RemoteIP = Legion.Text.StringHelper.TrimToFitMaxLength(RemoteIP, 64, postfix);
	}

	public override string? GetPrimaryKeyValue()
	{
		return IdUserToken.ToString();
	}

	public override string? ToString()
	{
		return IdUserToken.ToString();
	}

	public static ValidatorBuilder<UserToken> SetDBValidatorRules(ValidatorBuilder<UserToken> builder)
		=> builder
			.ForProperty(x => x.IdUserToken, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.IdLoginProvider, v => v.NotDefaultOrEmpty(), (x, parent) => x.LoginProvider == null)
			.ForProperty(x => x.IdUser, v => v.NotDefaultOrEmpty(), (x, parent) => x.User == null)
			.ForProperty(x => x.Name, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.Value, v => v.NotDefaultOrEmpty())
			//.ForProperty(x => x.CreatedUtc, v => v.NotDefaultOrEmpty())
			//.ForProperty(x => x.ValidToUtc, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.RemoteIP, v => v.MaxLength(64))
		;
}
