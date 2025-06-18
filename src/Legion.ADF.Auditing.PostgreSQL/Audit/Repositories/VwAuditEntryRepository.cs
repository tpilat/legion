using Legion;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Legion.Exceptions;
using Legion.Transactions;
using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.Auditing.PostgreSQL.Audit.Repositories;

public partial class VwAuditEntryRepository : Legion.ADF.Auditing.PostgreSQL.AuditQueryRepositoryBase, Legion.ADF.Auditing.IAuditQueryRepository<Legion.ADF.Auditing.Audit.VwAuditEntry>, Legion.ADF.Auditing.Audit.Repositories.IVwAuditEntryRepository
{
	public VwAuditEntryRepository(IEFConnectionProvider connectionProvider, Legion.EntityFrameworkCore.Audit.IAuditEntriesManager? auditEntriesManager)
		: base(connectionProvider, auditEntriesManager)
	{
	}

	private Legion.ADF.Auditing.PostgreSQL.IAuditQueryDbContext? context;
	public IQueryable<Legion.ADF.Auditing.Audit.VwAuditEntry> AsQueryable(IScopeContext scopeContext)
		=> (context ??= ConnectionProvider.GetOrCreateDbContext<Legion.ADF.Auditing.PostgreSQL.IAuditQueryDbContext>(scopeContext, AuditEntriesManager)).VwAuditEntry;

	public IQueryable<Legion.ADF.Auditing.Audit.VwAuditEntry> AsReadOnlyQueryable(IScopeContext scopeContext)
		=> AsQueryable(scopeContext).AsNoTracking();
	}
