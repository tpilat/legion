using Legion;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Legion.Exceptions;
using Legion.Transactions;
using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.Auditing.PostgreSQL.Audit.Repositories;

public partial class VwApplicationEntryRepository : Legion.ADF.Auditing.PostgreSQL.AuditQueryRepositoryBase, Legion.ADF.Auditing.IAuditQueryRepository<Legion.ADF.Auditing.Audit.VwApplicationEntry>, Legion.ADF.Auditing.Audit.Repositories.IVwApplicationEntryRepository
{
	public VwApplicationEntryRepository(IEFConnectionProvider connectionProvider, Legion.EntityFrameworkCore.Audit.IAuditEntriesManager? auditEntriesManager)
		: base(connectionProvider, auditEntriesManager)
	{
	}

	private Legion.ADF.Auditing.PostgreSQL.IAuditQueryDbContext? context;
	public IQueryable<Legion.ADF.Auditing.Audit.VwApplicationEntry> AsQueryable(IScopeContext scopeContext)
		=> (context ??= ConnectionProvider.GetOrCreateDbContext<Legion.ADF.Auditing.PostgreSQL.IAuditQueryDbContext>(scopeContext, AuditEntriesManager)).VwApplicationEntry;

	public IQueryable<Legion.ADF.Auditing.Audit.VwApplicationEntry> AsReadOnlyQueryable(IScopeContext scopeContext)
		=> AsQueryable(scopeContext).AsNoTracking();
	}
