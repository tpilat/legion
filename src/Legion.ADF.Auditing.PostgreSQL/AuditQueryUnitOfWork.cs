using Legion;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Audit;
using Legion.EntityFrameworkCore.Queries;
using Legion.Exceptions;
using Legion.Transactions;

namespace Legion.ADF.Auditing.PostgreSQL;

public partial class AuditQueryUnitOfWork : Legion.ADF.Auditing.IAuditQueryUnitOfWork, Legion.Model.Repositories.IQueryUnitOfWork
{
	public IEFConnectionProvider ConnectionProvider { get; }
	public IAuditEntriesManager? AuditEntriesManager { get; }

	public AuditQueryUnitOfWork(IEFConnectionProvider connectionProvider, IAuditEntriesManager? auditEntriesManager)
	{
		Throw.IfArgumentNull(connectionProvider);

		ConnectionProvider = connectionProvider;
		AuditEntriesManager = auditEntriesManager;
	}

	protected Legion.ADF.Auditing.PostgreSQL.IAuditQueryDbContext GetContext(IScopeContext scopeContext)
		=> ConnectionProvider.GetOrCreateDbContext<Legion.ADF.Auditing.PostgreSQL.IAuditQueryDbContext>(scopeContext, AuditEntriesManager);


	private Legion.ADF.Auditing.Audit.Repositories.IVwApplicationEntryRepository vwApplicationEntry;
	public Legion.ADF.Auditing.Audit.Repositories.IVwApplicationEntryRepository VwApplicationEntryRepository
		=> vwApplicationEntry ??= new Legion.ADF.Auditing.PostgreSQL.Audit.Repositories.VwApplicationEntryRepository(ConnectionProvider, AuditEntriesManager);


	private Legion.ADF.Auditing.Audit.Repositories.IVwApplicationEntryTokenRepository vwApplicationEntryToken;
	public Legion.ADF.Auditing.Audit.Repositories.IVwApplicationEntryTokenRepository VwApplicationEntryTokenRepository
		=> vwApplicationEntryToken ??= new Legion.ADF.Auditing.PostgreSQL.Audit.Repositories.VwApplicationEntryTokenRepository(ConnectionProvider, AuditEntriesManager);


	private Legion.ADF.Auditing.Audit.Repositories.IVwAuditEntryRepository vwAuditEntry;
	public Legion.ADF.Auditing.Audit.Repositories.IVwAuditEntryRepository VwAuditEntryRepository
		=> vwAuditEntry ??= new Legion.ADF.Auditing.PostgreSQL.Audit.Repositories.VwAuditEntryRepository(ConnectionProvider, AuditEntriesManager);
}
