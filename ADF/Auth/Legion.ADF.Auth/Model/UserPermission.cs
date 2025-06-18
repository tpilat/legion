using Legion.Validation;

namespace Legion.ADF.Auth.Model;

public sealed partial class UserPermission : Auth.AuthBaseEntity, Legion.Model.Audit.ISelfAuditableEntity, Legion.Model.Concurrence.IConcurrent, Legion.Model.Audit.IAuditableEntity, Legion.Model.IEntity
{
	public static IValidator<UserPermission> DefaultDBValidator { get; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdUserPermission { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL | Auth.Model.User.User | FK_UserPermission_IdUser
	/// </summary>
	public Guid IdUser { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL | Auth.Model.Permission.Permission | FK_UserPermission_IdPermission
	/// </summary>
	public Guid IdPermission { get; private set; }

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
	/// _1:N Guid IdPermission | FK_UserPermission_IdPermission
	/// </summary>
	public Auth.Model.Permission Permission { get; private set; }

	/// <summary>
	/// _1:N Guid IdUser | FK_UserPermission_IdUser
	/// </summary>
	public Auth.Model.User User { get; private set; }

	private UserPermission()
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
		ConcurrencyToken = Guid.NewGuid();
	}

	static UserPermission()
	{
		DefaultDBValidator = SetDBValidatorRules(new ValidatorBuilder<UserPermission>()).Build();
	}

	public Dictionary<string, object?> ToDictionary()
		=> new()
		{
			{ nameof(IdUserPermission), IdUserPermission },
			{ nameof(IdUser), IdUser },
			{ nameof(IdPermission), IdPermission },
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
		return IdUserPermission.ToString();
	}

	public override string? ToString()
	{
		return IdUserPermission.ToString();
	}

	public static ValidatorBuilder<UserPermission> SetDBValidatorRules(ValidatorBuilder<UserPermission> builder)
		=> builder
			.ForProperty(x => x.IdUserPermission, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.IdUser, v => v.NotDefaultOrEmpty(), (x, parent) => x.User == null)
			.ForProperty(x => x.IdPermission, v => v.NotDefaultOrEmpty(), (x, parent) => x.Permission == null)
			//.ForProperty(x => x.TenantIdentifier, v => v.NotDefaultOrEmpty())
			//.ForProperty(x => x.AuditCreatedUtc, v => v.NotDefaultOrEmpty())
			//.ForProperty(x => x.ConcurrencyToken, v => v.NotDefaultOrEmpty())
			//.ForProperty(x => x.DeletedUtc, v => v.NotDefaultOrEmpty())
		;
}
