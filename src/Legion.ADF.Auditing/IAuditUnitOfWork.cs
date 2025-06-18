using Legion.Database;
using System.Runtime.CompilerServices;

namespace Legion.ADF.Auditing;

public partial interface IAuditUnitOfWork : Legion.Model.Repositories.IUnitOfWork
{

	Legion.ADF.Auditing.Audit.Repositories.IApplicationEntryRepository ApplicationEntryRepository { get; }

	Legion.ADF.Auditing.Audit.Repositories.IApplicationEntryTokenRepository ApplicationEntryTokenRepository { get; }

	Legion.ADF.Auditing.Audit.Repositories.IAuditEntryRepository AuditEntryRepository { get; }

	Legion.ADF.Auditing.Audit.Repositories.IAuditTypeRepository AuditTypeRepository { get; }
}
