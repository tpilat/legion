using Legion.Validation;

namespace Legion.ADF.Auth.Model;

public partial class User : Auth.AuthBaseEntity, Legion.Model.Audit.ISelfAuditableEntity, Legion.Model.Concurrence.IConcurrent, Legion.Model.Audit.IAuditableEntity, Legion.Model.IEntity
{
	private List<Auth.Model.ExternalLogin> _externalLogins;
	private List<Auth.Model.UserPermission> _userPermissions;
	private List<Auth.Model.UserRole> _userRoles;
	private List<Auth.Model.UserToken> _userTokens;

	public static IValidator<User> DefaultDBValidator { get; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdUser { get; private set; }

	/// <summary>
	/// Database DataType: varchar(256) NULL
	/// </summary>
	public string? Login { get; private set; }

	/// <summary>
	/// Database DataType: varchar(256) NULL
	/// </summary>
	public string? NormalizedLogin { get; private set; }

	/// <summary>
	/// Database DataType: uuid NULL
	/// </summary>
	public Guid? TenantIdentifier { get; private set; }

	/// <summary>
	/// Database DataType: varchar(256) NULL
	/// </summary>
	public string? Email { get; private set; }

	/// <summary>
	/// Database DataType: varchar(256) NULL
	/// </summary>
	public string? NormalizedEmail { get; private set; }

	/// <summary>
	/// Database DataType: boolean NOT NULL
	/// </summary>
	public bool EmailConfirmed { get; private set; }

	/// <summary>
	/// Database DataType: text NULL
	/// </summary>
	public string? PasswordHash { get; private set; }

	/// <summary>
	/// Database DataType: text NULL
	/// </summary>
	public string? SecurityStamp { get; private set; }

	/// <summary>
	/// Database DataType: text NULL
	/// </summary>
	public string? ADDistinguishedName { get; private set; }

	/// <summary>
	/// Database DataType: jsonb NULL
	/// </summary>
	public string? Data { get; private set; }

	/// <summary>
	/// Database DataType: varchar(256) NULL
	/// </summary>
	public string? PhoneNumber { get; private set; }

	/// <summary>
	/// Database DataType: boolean NOT NULL
	/// </summary>
	public bool PhoneNumberConfirmed { get; private set; }

	/// <summary>
	/// Database DataType: boolean NOT NULL
	/// </summary>
	public bool MultiFactorEnabled { get; private set; }

	/// <summary>
	/// Database DataType: timestamp with time zone NULL
	/// </summary>
	public DateTime? LockoutEndUtc { get; private set; }

	/// <summary>
	/// Database DataType: boolean NOT NULL
	/// </summary>
	public bool LockoutEnabled { get; private set; }

	/// <summary>
	/// Database DataType: integer NOT NULL
	/// </summary>
	public int AccessFailedCount { get; private set; }

	/// <summary>
	/// Database DataType: boolean NOT NULL
	/// </summary>
	public bool IsSystemUser { get; private set; }

	/// <summary>
	/// Database DataType: text NULL
	/// </summary>
	public string? ConfirmationUrlSlug { get; private set; }

	/// <summary>
	/// Database DataType: timestamp with time zone NULL
	/// </summary>
	public DateTime? ConfirmationUrlValidToUtc { get; private set; }

	/// <summary>
	/// Database DataType: timestamp with time zone NOT NULL
	/// </summary>
	public DateTime AuditCreatedUtc { get; private set; }

	/// <summary>
	/// Database DataType: timestamp with time zone NULL
	/// </summary>
	public DateTime? AuditModifiedUtc { get; private set; }

	/// <summary>
	/// Database DataType: uuid NULL
	/// </summary>
	public Guid? IdAuditCreatedBy { get; private set; }

	/// <summary>
	/// Database DataType: uuid NULL
	/// </summary>
	public Guid? IdAuditModifiedBy { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid ConcurrencyToken { get; set; }

	/// <summary>
	/// Database DataType: timestamp with time zone NOT NULL
	/// </summary>
	public DateTime DeletedUtc { get; private set; }


	/// <summary>
	/// N:_1 Auth.Model.ExternalLogin.IdUser | FK_ExternalLogin_IdUser
	/// </summary>
	public IReadOnlyList<Auth.Model.ExternalLogin> ExternalLogins => _externalLogins;

	/// <summary>
	/// N:_1 Auth.Model.UserPermission.IdUser | FK_UserPermission_IdUser
	/// </summary>
	public IReadOnlyList<Auth.Model.UserPermission> UserPermissions => _userPermissions;

	/// <summary>
	/// N:_1 Auth.Model.UserRole.IdUser | FK_UserRole_IdUser
	/// </summary>
	public IReadOnlyList<Auth.Model.UserRole> UserRoles => _userRoles;

	/// <summary>
	/// N:_1 Auth.Model.UserToken.IdUser | FK_UserToken_IdUser
	/// </summary>
	public IReadOnlyList<Auth.Model.UserToken> UserTokens => _userTokens;

	private User()
	{
		_externalLogins = [];
		_userPermissions = [];
		_userRoles = [];
		_userTokens = [];
	}

	void Legion.Model.Audit.ISelfAuditableEntity.SetAuditCreated(DateTime auditCreatedUtc, Guid? idAuditCreatedBy)
	{
		AuditCreatedUtc = auditCreatedUtc;
		IdAuditCreatedBy = idAuditCreatedBy;
	}

	void Legion.Model.Audit.ISelfAuditableEntity.SetAuditModified(DateTime auditModifiedUtc, Guid? idAuditModifiedBy)
	{
		AuditModifiedUtc = auditModifiedUtc;
		IdAuditModifiedBy = idAuditModifiedBy;
	}

	[System.ComponentModel.DataAnnotations.Schema.NotMapped]
	string Legion.Model.Concurrence.IConcurrent.ConcurrencyTokenPropertyName => nameof(ConcurrencyToken);

	public void SetNewConcurrencyToken()
	{
		ConcurrencyToken = Guid.NewGuid();
	}

	static User()
	{
		DefaultDBValidator = SetDBValidatorRules(new ValidatorBuilder<User>()).Build();
	}

	public Dictionary<string, object?> ToDictionary()
		=> new()
		{
			{ nameof(IdUser), IdUser },
			{ nameof(Login), Login },
			{ nameof(NormalizedLogin), NormalizedLogin },
			{ nameof(TenantIdentifier), TenantIdentifier },
			{ nameof(Email), Email },
			{ nameof(NormalizedEmail), NormalizedEmail },
			{ nameof(EmailConfirmed), EmailConfirmed },
			{ nameof(PasswordHash), PasswordHash },
			{ nameof(SecurityStamp), SecurityStamp },
			{ nameof(ADDistinguishedName), ADDistinguishedName },
			{ nameof(Data), Data },
			{ nameof(PhoneNumber), PhoneNumber },
			{ nameof(PhoneNumberConfirmed), PhoneNumberConfirmed },
			{ nameof(MultiFactorEnabled), MultiFactorEnabled },
			{ nameof(LockoutEndUtc), LockoutEndUtc },
			{ nameof(LockoutEnabled), LockoutEnabled },
			{ nameof(AccessFailedCount), AccessFailedCount },
			{ nameof(IsSystemUser), IsSystemUser },
			{ nameof(ConfirmationUrlSlug), ConfirmationUrlSlug },
			{ nameof(ConfirmationUrlValidToUtc), ConfirmationUrlValidToUtc },
			{ nameof(AuditCreatedUtc), AuditCreatedUtc },
			{ nameof(AuditModifiedUtc), AuditModifiedUtc },
			{ nameof(IdAuditCreatedBy), IdAuditCreatedBy },
			{ nameof(IdAuditModifiedBy), IdAuditModifiedBy },
			{ nameof(ConcurrencyToken), ConcurrencyToken },
			{ nameof(DeletedUtc), DeletedUtc },
		};

	public void TrimStringValuesToFitDatabaseMaxLengths(string? postfix = null)
	{
		Login = Legion.Text.StringHelper.TrimToFitMaxLength(Login, 256, postfix);
		NormalizedLogin = Legion.Text.StringHelper.TrimToFitMaxLength(NormalizedLogin, 256, postfix);
		Email = Legion.Text.StringHelper.TrimToFitMaxLength(Email, 256, postfix);
		NormalizedEmail = Legion.Text.StringHelper.TrimToFitMaxLength(NormalizedEmail, 256, postfix);
		PhoneNumber = Legion.Text.StringHelper.TrimToFitMaxLength(PhoneNumber, 256, postfix);
	}

	public override string? GetPrimaryKeyValue()
	{
		return IdUser.ToString();
	}

	public override string? ToString()
	{
		return IdUser.ToString();
	}

	public static ValidatorBuilder<User> SetDBValidatorRules(ValidatorBuilder<User> builder)
		=> builder
			.ForProperty(x => x.IdUser, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.Login, v => v.MaxLength(256))
			.ForProperty(x => x.NormalizedLogin, v => v.MaxLength(256))
			.ForProperty(x => x.Email, v => v.MaxLength(256))
			.ForProperty(x => x.NormalizedEmail, v => v.MaxLength(256))
			.ForProperty(x => x.PhoneNumber, v => v.MaxLength(256))
			//.ForProperty(x => x.AccessFailedCount, v => v.NotDefaultOrEmpty())
			//.ForProperty(x => x.AuditCreatedUtc, v => v.NotDefaultOrEmpty())
			//.ForProperty(x => x.ConcurrencyToken, v => v.NotDefaultOrEmpty())
			//.ForProperty(x => x.DeletedUtc, v => v.NotDefaultOrEmpty())
		;
}
