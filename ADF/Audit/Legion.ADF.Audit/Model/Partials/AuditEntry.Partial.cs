namespace Legion.ADF.Audit.Model;

public sealed partial class AuditEntry : Audit.AuditBaseEntity, Legion.Model.IEntity
{
	internal static IResult<AuditEntry> Create(
		IScopeContext scopeContext,
		Guid auditCorrelationId,
		Guid idAuditOperation,
		string tableName,
		string? primaryKey,
		string? oldValues,
		string? newValues,
		string? affectedColumns,
		string? traceFrame)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<AuditEntry>();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, tableName))
			return result.Build();

		var auditEntry = new AuditEntry
		{
			__IsNewObject = true,
			IdAuditEntry = Guid.NewGuid(),
			CreatedUtc = GlobalContext.Instance.UtcNow,
			IdAuditOperation = idAuditOperation,
			TableName = tableName,
			IdUser = scopeContext.IdUser,
			PrimaryKey = primaryKey,
			OldValues = oldValues,
			NewValues = newValues,
			AffectedColumns = affectedColumns,
			AuditCorrelationId = auditCorrelationId,
			TraceFrame = traceFrame,
			CorrelationId = scopeContext.CorrelationId,
		};

		var validationResult =
			DefaultDBValidator
				.Validate(auditEntry);

		if (result.MergeHasError(scopeContext.CreateNew(), validationResult, true))
			return result.Build();

		return result.WithData(auditEntry).Build();
	}

	internal static IResult<AuditEntry> Create(
		IScopeContext scopeContext,
		Legion.Model.Audit.IAuditEntry iauditEntry)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<AuditEntry>();

		if (result.IsArgumentNull(scopeContext, iauditEntry))
			return result.Build();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, iauditEntry.TableName))
			return result.Build();

		var auditEntry = new AuditEntry
		{
			__IsNewObject = true,
			IdAuditEntry = Guid.NewGuid(),
			CreatedUtc = GlobalContext.Instance.UtcNow,
			IdAuditOperation = iauditEntry.IdAuditOperation,
			TableName = iauditEntry.TableName,
			IdUser = iauditEntry.IdUser,
			PrimaryKey = iauditEntry.PrimaryKey,
			OldValues = iauditEntry.OldValues,
			NewValues = iauditEntry.NewValues,
			AffectedColumns = iauditEntry.AffectedColumns,
			AuditCorrelationId = iauditEntry.AuditCorrelationId,
			TraceFrame = iauditEntry.TraceFrame,
			CorrelationId = iauditEntry.CorrelationId,
		};

		var validationResult =
			DefaultDBValidator
				.Validate(auditEntry);

		if (result.MergeHasError(scopeContext.CreateNew(), validationResult, true))
			return result.Build();

		return result.WithData(auditEntry).Build();
	}

	internal static IResult<List<AuditEntry>> CreateRange(
		IScopeContext scopeContext,
		IEnumerable<Legion.Model.Audit.IAuditEntry> iauditEntries)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<List<AuditEntry>>();

		if (result.IsArgumentNullOrEmpty(scopeContext, iauditEntries))
			return result.Build();

		var auditEntries = new List<AuditEntry>();

		foreach (var iauditEntry in iauditEntries)
		{
			var createResult = Create(scopeContext, iauditEntry);
			if (result.MergeHasError(createResult))
				return result.Build();

			auditEntries.Add(createResult.Data!);
		}

		return result.WithData(auditEntries).Build();
	}
}
