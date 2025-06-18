using Legion;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Legion.Exceptions;
using Legion.Transactions;
using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.Auditing.PostgreSQL.Audit.Repositories;

public partial class ApplicationEntryTokenRepository : Legion.ADF.Auditing.PostgreSQL.AuditRepositoryBase, Legion.ADF.Auditing.IAuditRepository<Legion.ADF.Auditing.Audit.ApplicationEntryToken>, Legion.ADF.Auditing.Audit.Repositories.IApplicationEntryTokenRepository
{
	public ApplicationEntryTokenRepository(IEFConnectionProvider connectionProvider, Legion.EntityFrameworkCore.Audit.IAuditEntriesManager? auditEntriesManager)
		: base(connectionProvider, auditEntriesManager)
	{
	}

	private Legion.ADF.Auditing.PostgreSQL.IAuditDbContext? context;
	public IQueryable<Legion.ADF.Auditing.Audit.ApplicationEntryToken> AsQueryable(IScopeContext scopeContext)
		=> (context ??= ConnectionProvider.GetOrCreateDbContext<Legion.ADF.Auditing.PostgreSQL.IAuditDbContext>(scopeContext, AuditEntriesManager)).ApplicationEntryToken;

	public IQueryable<Legion.ADF.Auditing.Audit.ApplicationEntryToken> AsReadOnlyQueryable(IScopeContext scopeContext)
		=> AsQueryable(scopeContext).AsNoTracking();
	
	public void Add(IScopeContext scopeContext, Legion.ADF.Auditing.Audit.ApplicationEntryToken entity)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		dbContext.ApplicationEntryToken.Add(entity);
	}

	public void Remove(IScopeContext scopeContext, Legion.ADF.Auditing.Audit.ApplicationEntryToken entity)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		dbContext.ApplicationEntryToken.Remove(entity);
	}
}
