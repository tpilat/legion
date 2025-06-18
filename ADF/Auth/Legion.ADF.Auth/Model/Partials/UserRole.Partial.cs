namespace Legion.ADF.Auth.Model;

public sealed partial class UserRole : Auth.AuthBaseEntity, Legion.Model.Audit.ISelfAuditableEntity, Legion.Model.Concurrence.IConcurrent, Legion.Model.Audit.IAuditableEntity, Legion.Model.IEntity
{
	internal static IResult<UserRole> CreateUserRole(
		IScopeContext scopeContext,
		Guid idUser,
		Guid idRole)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<UserRole>();

		var userRole = new UserRole
		{
			__IsNewObject = true,
			IdUserRole = Guid.NewGuid(),
			IdUser = idUser,
			IdRole = idRole,
			TenantIdentifier = scopeContext.TenantIdentifier ?? Guid.Empty,
			AuditCreatedUtc = GlobalContext.Instance.UtcNow,
			AuditModifiedUtc = null,
			IdAuditCreatedBy = scopeContext.IdUser,
			IdAuditModifiedBy = null,
			ConcurrencyToken = Guid.NewGuid(),
			DeletedUtc = DateTime.MinValue
		};

		var validationResult =
			DefaultDBValidator
				.Validate(userRole);

		if (result.MergeHasError(scopeContext.CreateNew(), validationResult, true))
			return result.Build();

		return result.WithData(userRole).Build();
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
