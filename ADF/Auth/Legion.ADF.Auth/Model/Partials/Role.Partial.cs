namespace Legion.ADF.Auth.Model;

public partial class Role : Auth.AuthBaseEntity, Legion.Model.Audit.ISelfAuditableEntity, Legion.Model.Concurrence.IConcurrent, Legion.Model.Audit.IAuditableEntity, Legion.Model.IEntity
{
	internal static IResult<Role> CreateRole(
		IScopeContext scopeContext,
		string roleName,
		string normalizedRoleName)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<Role>();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, roleName))
			return result.Build();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, normalizedRoleName))
			return result.Build();

		var role = new Role
		{
			__IsNewObject = true,
			IdRole = Guid.NewGuid(),
			Name = roleName,
			NormalizedName = normalizedRoleName,
			ADGroupDistinguishedName = null,
			Data = null,
			Description = null,
			HasConstantPermissions = false,
			HasConstantUsers = false,
			IsSystemRole = false,
			AuditCreatedUtc = GlobalContext.Instance.UtcNow,
			AuditModifiedUtc = null,
			IdAuditCreatedBy = scopeContext.IdUser,
			IdAuditModifiedBy = null,
			ConcurrencyToken = Guid.NewGuid(),
			DeletedUtc = DateTime.MinValue
		};

		var validationResult =
			DefaultDBValidator
				.Validate(role);

		if (result.MergeHasError(scopeContext.CreateNew(), validationResult, true))
			return result.Build();

		return result.WithData(role).Build();
	}

	internal IResult SetName(
		IScopeContext scopeContext,
		string normalizedName)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, normalizedName))
			return result.Build();

		Name = normalizedName;

		if (!__IsNewObject)
		{
			AuditModifiedUtc = GlobalContext.Instance.UtcNow;
			IdAuditModifiedBy = scopeContext.IdUser;
			ConcurrencyToken = Guid.NewGuid();
		}

		var validationResult =
			DefaultDBValidator
				.Validate(this);

		if (result.MergeHasError(scopeContext.CreateNew(), validationResult, true))
			return result.Build();

		return result.Build();
	}

	internal IResult SetNormalizedName(
		IScopeContext scopeContext,
		string normalizedName)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, normalizedName))
			return result.Build();

		NormalizedName = normalizedName;

		if (!__IsNewObject)
		{
			AuditModifiedUtc = GlobalContext.Instance.UtcNow;
			IdAuditModifiedBy = scopeContext.IdUser;
			ConcurrencyToken = Guid.NewGuid();
		}

		var validationResult =
			DefaultDBValidator
				.Validate(this);

		if (result.MergeHasError(scopeContext.CreateNew(), validationResult, true))
			return result.Build();

		return result.Build();
	}

	internal IResult SetSoftDelete(IScopeContext scopeContext)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder();

		DeletedUtc = GlobalContext.Instance.UtcNow;

		if (!__IsNewObject)
		{
			AuditModifiedUtc = DeletedUtc;
			IdAuditModifiedBy = scopeContext.IdUser;
			ConcurrencyToken = Guid.NewGuid();
		}

		var validationResult =
			DefaultDBValidator
				.Validate(this);

		if (result.MergeHasError(scopeContext.CreateNew(), validationResult, true))
			return result.Build();

		return result.Build();
	}
}
