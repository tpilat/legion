using Legion.Model.Audit;

namespace Legion.ADF.Audit.Services;

public partial class AuditStore : IAuditEntryStore, IDisposable, IAsyncDisposable
{
	public async Task<IResult<Model.AuditEntry>> WriteAuditEntryAsync(
		IScopeContext scopeContext,
		Guid auditCorrelationId,
		Guid idAuditOperation,
		string tableName,
		string? primaryKey,
		string? oldValues,
		string? newValues,
		string? affectedColumns,
		string? commandQueryName,
		bool checkPermissions,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<Model.AuditEntry>();

		if (result.IsCancellationRequested(cancellationToken, scopeContext))
			return result.Build();

		if (result.IsDisposed(_disposed, scopeContext))
			return result.Build();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, tableName))
			return result.Build();

		var createResult = Model.AuditEntry.Create(
			scopeContext,
			auditCorrelationId,
			idAuditOperation,
			tableName,
			primaryKey,
			oldValues,
			newValues,
			affectedColumns,
			commandQueryName);

		if (result.MergeHasError(createResult))
			return result.Build();

		var auditEntry = createResult.Data!;

		if (checkPermissions)
		{
			var operationName = nameof(AuditPermissions.AuditEntry.WriteAuditEntry);
			if (AccessControlManager?.IsAuthorizedFor(scopeContext, operationName, auditEntry) == false)
				return result.WithUnauthorizedException(scopeContext, null, operationName);
		}

		UoW.AuditEntryRepository.Add(scopeContext, auditEntry);

		var saveResult = await SaveInternalAsync(scopeContext, force: false, cancellationToken);
		result.MergeHasError(saveResult);
		return result.WithData(auditEntry).Build();
	}
}
