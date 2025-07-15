using Legion.Validation;

namespace Legion.ADF.Auth.Model;

public sealed partial class UserRole : Auth.AuthBaseEntity, Legion.Model.Audit.ISelfAuditableEntity, Legion.Model.Concurrence.IConcurrent, Legion.Model.Audit.IAuditableEntity, Legion.Model.IEntity
{
	public static IValidator<UserRole> DefaultDBValidator { get; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdUserRole { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL | Auth.Model.User.User | FK_UserRole_IdUser
	/// </summary>
	public Guid IdUser { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL | Auth.Model.Role.Role | FK_UserRole_IdRole
	/// </summary>
	public Guid IdRole { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid TenantIdentifier { get; private set; }

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
	/// _1:N Guid IdRole | FK_UserRole_IdRole
	/// </summary>
	public Auth.Model.Role Role { get; private set; }

	/// <summary>
	/// _1:N Guid IdUser | FK_UserRole_IdUser
	/// </summary>
	public Auth.Model.User User { get; private set; }

	private UserRole()
	{
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
		ConcurrencyToken = GlobalContext.Instance.NewGuid();
	}

	static UserRole()
	{
		DefaultDBValidator = SetDBValidatorRules(new ValidatorBuilder<UserRole>()).Build();
	}

	public Dictionary<string, object?> ToDictionary()
		=> new()
		{
			{ nameof(IdUserRole), IdUserRole },
			{ nameof(IdUser), IdUser },
			{ nameof(IdRole), IdRole },
			{ nameof(TenantIdentifier), TenantIdentifier },
			{ nameof(AuditCreatedUtc), AuditCreatedUtc },
			{ nameof(AuditModifiedUtc), AuditModifiedUtc },
			{ nameof(IdAuditCreatedBy), IdAuditCreatedBy },
			{ nameof(IdAuditModifiedBy), IdAuditModifiedBy },
			{ nameof(ConcurrencyToken), ConcurrencyToken },
			{ nameof(DeletedUtc), DeletedUtc },
		};

	public void TrimStringValuesToFitDatabaseMaxLengths(string? postfix = null)
	{
	}

	public override string? GetPrimaryKeyValue()
	{
		return IdUserRole.ToString();
	}

	public override string? ToString()
	{
		return IdUserRole.ToString();
	}

	public static ValidatorBuilder<UserRole> SetDBValidatorRules(ValidatorBuilder<UserRole> builder)
		=> builder
			.ForProperty(x => x.IdUserRole, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.IdUser, v => v.NotDefaultOrEmpty(), (x, parent) => x.User == null)
			.ForProperty(x => x.IdRole, v => v.NotDefaultOrEmpty(), (x, parent) => x.Role == null)
			//.ForProperty(x => x.TenantIdentifier, v => v.NotDefaultOrEmpty())
			//.ForProperty(x => x.AuditCreatedUtc, v => v.NotDefaultOrEmpty())
			//.ForProperty(x => x.ConcurrencyToken, v => v.NotDefaultOrEmpty())
			//.ForProperty(x => x.DeletedUtc, v => v.NotDefaultOrEmpty())
		;
}
