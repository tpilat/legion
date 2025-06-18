using Legion.Database;
using System.Runtime.CompilerServices;

namespace Legion.ADF.Audit;

public partial interface IAuditUnitOfWork : Legion.Model.Repositories.IUnitOfWork, IDisposable, IAsyncDisposable
{

	Legion.ADF.Audit.Model.Repositories.IApplicationEntryRepository ApplicationEntryRepository { get; }

	Legion.ADF.Audit.Model.Repositories.IApplicationEntryRequestRepository ApplicationEntryRequestRepository { get; }

	Legion.ADF.Audit.Model.Repositories.IApplicationEntryResponseRepository ApplicationEntryResponseRepository { get; }

	Legion.ADF.Audit.Model.Repositories.IApplicationEntryTokenRepository ApplicationEntryTokenRepository { get; }

	Legion.ADF.Audit.Model.Repositories.IAuditEntryRepository AuditEntryRepository { get; }

	Legion.ADF.Audit.Model.Repositories.IAuditOperationRepository AuditOperationRepository { get; }
}
