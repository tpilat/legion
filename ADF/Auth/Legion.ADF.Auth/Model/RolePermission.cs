using Legion.Validation;

namespace Legion.ADF.Auth.Model;

public sealed partial class RolePermission : Auth.AuthBaseEntity, Legion.Model.Audit.ISelfAuditableEntity, Legion.Model.Concurrence.IConcurrent, Legion.Model.Audit.IAuditableEntity, Legion.Model.IEntity
{
	public static IValidator<RolePermission> DefaultDBValidator { get; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdRolePermission { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL | Auth.Model.Role.Role | FK_RolePermission_IdRole
	/// </summary>
	public Guid IdRole { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL | Auth.Model.Permission.Permission | FK_RolePermission_IdPermission
	/// </summary>
	public Guid IdPermission { get; private set; }

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
	/// _1:N Guid IdPermission | FK_RolePermission_IdPermission
	/// </summary>
	public Auth.Model.Permission Permission { get; private set; }

	/// <summary>
	/// _1:N Guid IdRole | FK_RolePermission_IdRole
	/// </summary>
	public Auth.Model.Role Role { get; private set; }

	private RolePermission()
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

	static RolePermission()
	{
		DefaultDBValidator = SetDBValidatorRules(new ValidatorBuilder<RolePermission>()).Build();
	}

	public Dictionary<string, object?> ToDictionary()
		=> new()
		{
			{ nameof(IdRolePermission), IdRolePermission },
			{ nameof(IdRole), IdRole },
			{ nameof(IdPermission), IdPermission },
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
		return IdRolePermission.ToString();
	}

	public override string? ToString()
	{
		return IdRolePermission.ToString();
	}

	public static ValidatorBuilder<RolePermission> SetDBValidatorRules(ValidatorBuilder<RolePermission> builder)
		=> builder
			.ForProperty(x => x.IdRolePermission, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.IdRole, v => v.NotDefaultOrEmpty(), (x, parent) => x.Role == null)
			.ForProperty(x => x.IdPermission, v => v.NotDefaultOrEmpty(), (x, parent) => x.Permission == null)
			//.ForProperty(x => x.AuditCreatedUtc, v => v.NotDefaultOrEmpty())
			//.ForProperty(x => x.ConcurrencyToken, v => v.NotDefaultOrEmpty())
			//.ForProperty(x => x.DeletedUtc, v => v.NotDefaultOrEmpty())
		;
}
