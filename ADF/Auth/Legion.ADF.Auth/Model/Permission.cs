using Legion.Validation;

namespace Legion.ADF.Auth.Model;

public sealed partial class Permission : Auth.AuthBaseEntity, Legion.Model.IEntity
{
	private List<Auth.Model.RolePermission> _rolePermissions;
	private List<Auth.Model.UserPermission> _userPermissions;

	public static IValidator<Permission> DefaultDBValidator { get; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdPermission { get; private set; }

	/// <summary>
	/// Database DataType: varchar(256) NOT NULL
	/// </summary>
	public string Code { get; private set; }

	/// <summary>
	/// Database DataType: varchar(1024) NOT NULL
	/// </summary>
	public string Name { get; private set; }

	/// <summary>
	/// Database DataType: text NULL
	/// </summary>
	public string? Description { get; private set; }

	/// <summary>
	/// Database DataType: varchar(1024) NULL
	/// </summary>
	public string? ClaimValue { get; private set; }

	/// <summary>
	/// Database DataType: boolean NOT NULL
	/// </summary>
	public bool IsSystemPermission { get; private set; }


	/// <summary>
	/// N:_1 Auth.Model.RolePermission.IdPermission | FK_RolePermission_IdPermission
	/// </summary>
	public IReadOnlyList<Auth.Model.RolePermission> RolePermissions => _rolePermissions;

	/// <summary>
	/// N:_1 Auth.Model.UserPermission.IdPermission | FK_UserPermission_IdPermission
	/// </summary>
	public IReadOnlyList<Auth.Model.UserPermission> UserPermissions => _userPermissions;

	private Permission()
	{
		_rolePermissions = [];
		_userPermissions = [];
	}

	static Permission()
	{
		DefaultDBValidator = SetDBValidatorRules(new ValidatorBuilder<Permission>()).Build();
	}

	public Dictionary<string, object?> ToDictionary()
		=> new()
		{
			{ nameof(IdPermission), IdPermission },
			{ nameof(Code), Code },
			{ nameof(Name), Name },
			{ nameof(Description), Description },
			{ nameof(ClaimValue), ClaimValue },
			{ nameof(IsSystemPermission), IsSystemPermission },
		};

	public void TrimStringValuesToFitDatabaseMaxLengths(string? postfix = null)
	{
		Code = Legion.Text.StringHelper.TrimToFitMaxLength(Code, 256, postfix);
		Name = Legion.Text.StringHelper.TrimToFitMaxLength(Name, 1024, postfix);
		ClaimValue = Legion.Text.StringHelper.TrimToFitMaxLength(ClaimValue, 1024, postfix);
	}

	public override string? GetPrimaryKeyValue()
	{
		return IdPermission.ToString();
	}

	public override string? ToString()
	{
		return IdPermission.ToString();
	}

	public static ValidatorBuilder<Permission> SetDBValidatorRules(ValidatorBuilder<Permission> builder)
		=> builder
			.ForProperty(x => x.IdPermission, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.Code, v => v.NotDefaultOrEmpty().MaxLength(256))
			.ForProperty(x => x.Name, v => v.NotDefaultOrEmpty().MaxLength(1024))
			.ForProperty(x => x.ClaimValue, v => v.MaxLength(1024))
		;
}
