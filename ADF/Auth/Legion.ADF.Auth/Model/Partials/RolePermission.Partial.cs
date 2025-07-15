namespace Legion.ADF.Auth.Model;

public sealed partial class RolePermission : Auth.AuthBaseEntity, Legion.Model.Audit.ISelfAuditableEntity, Legion.Model.Concurrence.IConcurrent, Legion.Model.Audit.IAuditableEntity, Legion.Model.IEntity
{
	internal static IResult<RolePermission> CreateRolePermission(
		IScopeContext scopeContext,
		Guid idRole,
		Guid idPermission)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<RolePermission>();

		var rolePermission = new RolePermission
		{
			__IsNewObject = true,
			IdRolePermission = GlobalContext.Instance.NewGuid(),
			IdRole = idRole,
			IdPermission = idPermission,
			AuditCreatedUtc = GlobalContext.Instance.UtcNow,
			AuditModifiedUtc = null,
			IdAuditCreatedBy = scopeContext.IdUser,
			IdAuditModifiedBy = null,
			ConcurrencyToken = GlobalContext.Instance.NewGuid(),
			DeletedUtc = DateTime.MinValue
		};

		var validationResult =
			DefaultDBValidator
				.Validate(rolePermission);

		if (result.MergeHasError(scopeContext.CreateNew(), validationResult, true))
			return result.Build();

		return result.WithData(rolePermission).Build();
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
			ConcurrencyToken = GlobalContext.Instance.NewGuid();
		}

		var validationResult =
			DefaultDBValidator
				.Validate(this);

		if (result.MergeHasError(scopeContext.CreateNew(), validationResult, true))
			return result.Build();

		return result.Build();
	}
}
