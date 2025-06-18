namespace Legion.ADF.Audit;

public partial interface IAuditQueryUnitOfWork : Legion.Model.Repositories.IQueryUnitOfWork, IDisposable, IAsyncDisposable
{
	Legion.ADF.Audit.Model.Repositories.IVwApplicationEntryRepository VwApplicationEntryRepository { get; }

	Legion.ADF.Audit.Model.Repositories.IVwAuditEntryRepository VwAuditEntryRepository { get; }
}
