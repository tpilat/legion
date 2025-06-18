namespace Legion.ADF.Auth.Model;

public sealed partial class UserPermission : Auth.AuthBaseEntity, Legion.Model.Audit.ISelfAuditableEntity, Legion.Model.Concurrence.IConcurrent, Legion.Model.Audit.IAuditableEntity, Legion.Model.IEntity
{
	internal static IResult<UserPermission> CreateUserPermission(
		IScopeContext scopeContext,
		Guid idUser,
		Guid idPermission)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<UserPermission>();

		var userPermission = new UserPermission
		{
			__IsNewObject = true,
			IdUserPermission = Guid.NewGuid(),
			IdUser = idUser,
			IdPermission = idPermission,
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
				.Validate(userPermission);

		if (result.MergeHasError(scopeContext.CreateNew(), validationResult, true))
			return result.Build();

		return result.WithData(userPermission).Build();
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
