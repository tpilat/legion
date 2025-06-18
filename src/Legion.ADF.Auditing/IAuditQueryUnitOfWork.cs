namespace Legion.ADF.Auditing;

public partial interface IAuditQueryUnitOfWork : Legion.Model.Repositories.IQueryUnitOfWork
{
	Legion.ADF.Auditing.Audit.Repositories.IVwApplicationEntryRepository VwApplicationEntryRepository { get; }

	Legion.ADF.Auditing.Audit.Repositories.IVwApplicationEntryTokenRepository VwApplicationEntryTokenRepository { get; }

	Legion.ADF.Auditing.Audit.Repositories.IVwAuditEntryRepository VwAuditEntryRepository { get; }
}
