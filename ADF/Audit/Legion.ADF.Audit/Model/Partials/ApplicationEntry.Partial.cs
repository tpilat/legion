namespace Legion.ADF.Audit.Model;

public sealed partial class ApplicationEntry : Audit.AuditBaseEntity, Legion.Model.IEntity
{
	internal static IResult<ApplicationEntry> Create(
		IScopeContext scopeContext,
		Guid idApplicationEntryToken,
		Guid idAuditOperation,
		string? aggregateIdentifier,
		string? uri)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<ApplicationEntry>();

		var applicationEntry = new ApplicationEntry
		{
			__IsNewObject = true,
			IdApplicationEntry = GlobalContext.Instance.NewGuid(),
			IdApplicationEntryToken = idApplicationEntryToken,
			IdAuditOperation = idAuditOperation,
			RuntimeUniqueKey = scopeContext.RuntimeUniqueKey,
			CreatedUtc = GlobalContext.Instance.UtcNow,
			CorrelationId = scopeContext.CorrelationId,
			ExternalCorrelationId = scopeContext.ExternalCorrelationId,
			AggregateIdentifier = aggregateIdentifier,
			Uri = uri,
			IdUser = scopeContext.IdUser,
			TenantIdentifier = scopeContext.TenantIdentifier
		};

		var validationResult =
			DefaultDBValidator
				.Validate(applicationEntry);

		if (result.MergeHasError(scopeContext.CreateNew(), validationResult, true))
			return result.Build();

		return result.WithData(applicationEntry).Build();
	}
}
