using Legion.Validation;

namespace Legion.ADF.Auth.Model;

public partial class Role : Auth.AuthBaseEntity, Legion.Model.Audit.ISelfAuditableEntity, Legion.Model.Concurrence.IConcurrent, Legion.Model.Audit.IAuditableEntity, Legion.Model.IEntity
{
	private List<Auth.Model.RolePermission> _rolePermissions;
	private List<Auth.Model.UserRole> _userRoles;

	public static IValidator<Role> DefaultDBValidator { get; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdRole { get; private set; }

	/// <summary>
	/// Database DataType: varchar(256) NOT NULL
	/// </summary>
	public string Name { get; private set; }

	/// <summary>
	/// Database DataType: varchar(256) NOT NULL
	/// </summary>
	public string NormalizedName { get; private set; }

	/// <summary>
	/// Database DataType: text NULL
	/// </summary>
	public string? ADGroupDistinguishedName { get; private set; }

	/// <summary>
	/// Database DataType: jsonb NULL
	/// </summary>
	public string? Data { get; private set; }

	/// <summary>
	/// Database DataType: text NULL
	/// </summary>
	public string? Description { get; private set; }

	/// <summary>
	/// Database DataType: boolean NOT NULL
	/// </summary>
	public bool HasConstantPermissions { get; private set; }

	/// <summary>
	/// Database DataType: boolean NOT NULL
	/// </summary>
	public bool HasConstantUsers { get; private set; }

	/// <summary>
	/// Database DataType: boolean NOT NULL
	/// </summary>
	public bool IsSystemRole { get; private set; }

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
	/// N:_1 Auth.Model.RolePermission.IdRole | FK_RolePermission_IdRole
	/// </summary>
	public IReadOnlyList<Auth.Model.RolePermission> RolePermissions => _rolePermissions;

	/// <summary>
	/// N:_1 Auth.Model.UserRole.IdRole | FK_UserRole_IdRole
	/// </summary>
	public IReadOnlyList<Auth.Model.UserRole> UserRoles => _userRoles;

	private Role()
	{
		_rolePermissions = [];
		_userRoles = [];
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

	static Role()
	{
		DefaultDBValidator = SetDBValidatorRules(new ValidatorBuilder<Role>()).Build();
	}

	public Dictionary<string, object?> ToDictionary()
		=> new()
		{
			{ nameof(IdRole), IdRole },
			{ nameof(Name), Name },
			{ nameof(NormalizedName), NormalizedName },
			{ nameof(ADGroupDistinguishedName), ADGroupDistinguishedName },
			{ nameof(Data), Data },
			{ nameof(Description), Description },
			{ nameof(HasConstantPermissions), HasConstantPermissions },
			{ nameof(HasConstantUsers), HasConstantUsers },
			{ nameof(IsSystemRole), IsSystemRole },
			{ nameof(AuditCreatedUtc), AuditCreatedUtc },
			{ nameof(AuditModifiedUtc), AuditModifiedUtc },
			{ nameof(IdAuditCreatedBy), IdAuditCreatedBy },
			{ nameof(IdAuditModifiedBy), IdAuditModifiedBy },
			{ nameof(ConcurrencyToken), ConcurrencyToken },
			{ nameof(DeletedUtc), DeletedUtc },
		};

	public void TrimStringValuesToFitDatabaseMaxLengths(string? postfix = null)
	{
		Name = Legion.Text.StringHelper.TrimToFitMaxLength(Name, 256, postfix);
		NormalizedName = Legion.Text.StringHelper.TrimToFitMaxLength(NormalizedName, 256, postfix);
	}

	public override string? GetPrimaryKeyValue()
	{
		return IdRole.ToString();
	}

	public override string? ToString()
	{
		return IdRole.ToString();
	}

	public static ValidatorBuilder<Role> SetDBValidatorRules(ValidatorBuilder<Role> builder)
		=> builder
			.ForProperty(x => x.IdRole, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.Name, v => v.NotDefaultOrEmpty().MaxLength(256))
			.ForProperty(x => x.NormalizedName, v => v.NotDefaultOrEmpty().MaxLength(256))
			//.ForProperty(x => x.AuditCreatedUtc, v => v.NotDefaultOrEmpty())
			//.ForProperty(x => x.ConcurrencyToken, v => v.NotDefaultOrEmpty())
			//.ForProperty(x => x.DeletedUtc, v => v.NotDefaultOrEmpty())
		;
}
