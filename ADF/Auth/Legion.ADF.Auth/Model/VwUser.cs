namespace Legion.ADF.Auth.Model;

public sealed partial class VwUser : Auth.AuthBaseQueryEntity, Legion.Model.IQueryEntity
{
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
	/// Database DataType: boolean NULL
	/// </summary>
	public bool? EmailConfirmed { get; private set; }

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
	/// Database DataType: boolean NULL
	/// </summary>
	public bool? PhoneNumberConfirmed { get; private set; }

	/// <summary>
	/// Database DataType: boolean NULL
	/// </summary>
	public bool? MultiFactorEnabled { get; private set; }

	/// <summary>
	/// Database DataType: timestamp with time zone NULL
	/// </summary>
	public DateTime? LockoutEndUtc { get; private set; }

	/// <summary>
	/// Database DataType: boolean NULL
	/// </summary>
	public bool? LockoutEnabled { get; private set; }

	/// <summary>
	/// Database DataType: integer NULL
	/// </summary>
	public int? AccessFailedCount { get; private set; }

	/// <summary>
	/// Database DataType: boolean NULL
	/// </summary>
	public bool? IsSystemUser { get; private set; }

	/// <summary>
	/// Database DataType: text NULL
	/// </summary>
	public string? ConfirmationUrlSlug { get; private set; }

	/// <summary>
	/// Database DataType: timestamp with time zone NULL
	/// </summary>
	public DateTime? ConfirmationUrlValidToUtc { get; private set; }

	/// <summary>
	/// Database DataType: timestamp with time zone NULL
	/// </summary>
	public DateTime? AuditCreatedUtc { get; private set; }

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
	/// Database DataType: uuid NULL
	/// </summary>
	public Guid? ConcurrencyToken { get; private set; }

	/// <summary>
	/// Database DataType: timestamp with time zone NULL
	/// </summary>
	public DateTime? DeletedUtc { get; private set; }


	private VwUser()
	{
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

	public override string? ToString()
	{
		return IdUser.ToString();
	}
}
