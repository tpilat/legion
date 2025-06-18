using Legion;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Legion.Exceptions;
using Legion.Transactions;
using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.Auditing.PostgreSQL.Audit.Repositories;

public partial class AuditTypeRepository : Legion.ADF.Auditing.PostgreSQL.AuditRepositoryBase, Legion.ADF.Auditing.IAuditRepository<Legion.ADF.Auditing.Audit.AuditType>, Legion.ADF.Auditing.Audit.Repositories.IAuditTypeRepository
{
	public AuditTypeRepository(IEFConnectionProvider connectionProvider, Legion.EntityFrameworkCore.Audit.IAuditEntriesManager? auditEntriesManager)
		: base(connectionProvider, auditEntriesManager)
	{
	}

	private Legion.ADF.Auditing.PostgreSQL.IAuditDbContext? context;
	public IQueryable<Legion.ADF.Auditing.Audit.AuditType> AsQueryable(IScopeContext scopeContext)
		=> (context ??= ConnectionProvider.GetOrCreateDbContext<Legion.ADF.Auditing.PostgreSQL.IAuditDbContext>(scopeContext, AuditEntriesManager)).AuditType;

	public IQueryable<Legion.ADF.Auditing.Audit.AuditType> AsReadOnlyQueryable(IScopeContext scopeContext)
		=> AsQueryable(scopeContext).AsNoTracking();
	
	public void Add(IScopeContext scopeContext, Legion.ADF.Auditing.Audit.AuditType entity)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		dbContext.AuditType.Add(entity);
	}

	public void Remove(IScopeContext scopeContext, Legion.ADF.Auditing.Audit.AuditType entity)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		dbContext.AuditType.Remove(entity);
	}
}
