namespace Legion.ADF.Auditing.Audit;

public sealed partial class AuditEntry : Auditing.EntityBase, Legion.Model.Audit.IAuditEntry, Legion.Model.IEntity
{
	public static IResult<AuditEntry> Create(IScopeContext scopeContext, Legion.Model.Audit.IAuditEntry otherAuditEntry)
	{
		var result = new ResultBuilder<AuditEntry>();

		//TODO: uncomment
		//if (scopeContext == null)
		//	return result.WithArgumentNullException(ScopeContext.Create(nameof(AuditEntry)), $"{nameof(scopeContext)} == null");

		//if (otherAuditEntry == null)
		//	return result.WithArgumentNullException(scopeContext, $"{nameof(otherAuditEntry)} == null");

		result.WithData(new AuditEntry
		{
			CreatedUtc = otherAuditEntry.CreatedUtc,
			IdUser = otherAuditEntry.IdUser,
			IdAuditType = otherAuditEntry.IdAuditType,
			TableName = otherAuditEntry.TableName,
			PrimaryKey = otherAuditEntry.PrimaryKey,
			OldValues = otherAuditEntry.OldValues,
			NewValues = otherAuditEntry.NewValues,
			AffectedColumns = otherAuditEntry.AffectedColumns,
			AuditCorrelationId = otherAuditEntry.AuditCorrelationId,
			CommandQueryName = otherAuditEntry.CommandQueryName,
			IdCommandQuery = otherAuditEntry.IdCommandQuery,
			CorrelationId = otherAuditEntry.CorrelationId,
		});

		return result.Build();
	}
}
