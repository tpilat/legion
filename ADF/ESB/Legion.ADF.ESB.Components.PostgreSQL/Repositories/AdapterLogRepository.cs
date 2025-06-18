using Legion;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Legion.Exceptions;
using Legion.Transactions;
using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.ESB.Components.PostgreSQL.Model.Repositories;

public partial class AdapterLogRepository : Legion.ADF.ESB.Components.PostgreSQL.ComponentsRepositoryBase, Legion.ADF.ESB.Components.IComponentsRepository<Legion.ADF.ESB.Components.Model.AdapterLog>, Legion.ADF.ESB.Components.Model.Repositories.IAdapterLogRepository
{
	public AdapterLogRepository(IEFConnectionProvider connectionProvider, Legion.Model.Audit.IAuditEntryStore? auditEntryStore)
		: base(connectionProvider, auditEntryStore)
	{
	}

	private Legion.ADF.ESB.Components.PostgreSQL.IComponentsDbContext? context;
	public IQueryable<Legion.ADF.ESB.Components.Model.AdapterLog> AsQueryable(IScopeContext scopeContext)
		=> (context ??= ConnectionProvider.GetOrCreateDbContext<Legion.ADF.ESB.Components.PostgreSQL.IComponentsDbContext>(scopeContext, AuditEntryStore)).AdapterLog;

	public IQueryable<Legion.ADF.ESB.Components.Model.AdapterLog> AsReadOnlyQueryable(IScopeContext scopeContext)
		=> AsQueryable(scopeContext).AsNoTracking();
	
	public void Add(IScopeContext scopeContext, Legion.ADF.ESB.Components.Model.AdapterLog entity)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		dbContext.AdapterLog.Add(entity);
	}

	public void Remove(IScopeContext scopeContext, Legion.ADF.ESB.Components.Model.AdapterLog entity)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		dbContext.AdapterLog.Remove(entity);
	}
}
