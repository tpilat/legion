using Legion.Validation;

namespace Legion.ADF.Auth.Model;

public sealed partial class ExternalLogin : Auth.AuthBaseEntity, Legion.Model.IEntity
{
	public static IValidator<ExternalLogin> DefaultDBValidator { get; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdExternalLogin { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL | Auth.Model.LoginProvider.LoginProvider | FK_ExternalLogin_IdLoginProvider
	/// </summary>
	public Guid IdLoginProvider { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL | Auth.Model.User.User | FK_ExternalLogin_IdUser
	/// </summary>
	public Guid IdUser { get; private set; }

	/// <summary>
	/// Database DataType: text NOT NULL
	/// </summary>
	public string ExternalUserIdentifier { get; private set; }

	/// <summary>
	/// Database DataType: jsonb NULL
	/// </summary>
	public string? Data { get; private set; }

	/// <summary>
	/// Database DataType: timestamp with time zone NOT NULL
	/// </summary>
	public DateTime CreatedUtc { get; private set; }

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
	/// _1:N Guid IdLoginProvider | FK_ExternalLogin_IdLoginProvider
	/// </summary>
	public Auth.Model.LoginProvider LoginProvider { get; private set; }

	/// <summary>
	/// _1:N Guid IdUser | FK_ExternalLogin_IdUser
	/// </summary>
	public Auth.Model.User User { get; private set; }

	private ExternalLogin()
	{
	}

	static ExternalLogin()
	{
		DefaultDBValidator = SetDBValidatorRules(new ValidatorBuilder<ExternalLogin>()).Build();
	}

	public Dictionary<string, object?> ToDictionary()
		=> new()
		{
			{ nameof(IdExternalLogin), IdExternalLogin },
			{ nameof(IdLoginProvider), IdLoginProvider },
			{ nameof(IdUser), IdUser },
			{ nameof(ExternalUserIdentifier), ExternalUserIdentifier },
			{ nameof(Data), Data },
			{ nameof(CreatedUtc), CreatedUtc },
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
		return IdExternalLogin.ToString();
	}

	public override string? ToString()
	{
		return IdExternalLogin.ToString();
	}

	public static ValidatorBuilder<ExternalLogin> SetDBValidatorRules(ValidatorBuilder<ExternalLogin> builder)
		=> builder
			.ForProperty(x => x.IdExternalLogin, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.IdLoginProvider, v => v.NotDefaultOrEmpty(), (x, parent) => x.LoginProvider == null)
			.ForProperty(x => x.IdUser, v => v.NotDefaultOrEmpty(), (x, parent) => x.User == null)
			.ForProperty(x => x.ExternalUserIdentifier, v => v.NotDefaultOrEmpty())
			//.ForProperty(x => x.CreatedUtc, v => v.NotDefaultOrEmpty())
			//.ForProperty(x => x.ValidToUtc, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.RemoteIP, v => v.MaxLength(64))
		;
}
