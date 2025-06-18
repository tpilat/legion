using Legion;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Legion.Exceptions;
using Legion.Transactions;
using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.ESB.Orchestrations.PostgreSQL.Model.Repositories;

public partial class OrchestrationStepLogRepository : Legion.ADF.ESB.Orchestrations.PostgreSQL.OrchestrationsRepositoryBase, Legion.ADF.ESB.Orchestrations.IOrchestrationsRepository<Legion.ADF.ESB.Orchestrations.Model.OrchestrationStepLog>, Legion.ADF.ESB.Orchestrations.Model.Repositories.IOrchestrationStepLogRepository
{
	public OrchestrationStepLogRepository(IEFConnectionProvider connectionProvider, Legion.Model.Audit.IAuditEntryStore? auditEntryStore)
		: base(connectionProvider, auditEntryStore)
	{
	}

	private Legion.ADF.ESB.Orchestrations.PostgreSQL.IOrchestrationsDbContext? context;
	public IQueryable<Legion.ADF.ESB.Orchestrations.Model.OrchestrationStepLog> AsQueryable(IScopeContext scopeContext)
		=> (context ??= ConnectionProvider.GetOrCreateDbContext<Legion.ADF.ESB.Orchestrations.PostgreSQL.IOrchestrationsDbContext>(scopeContext, AuditEntryStore)).OrchestrationStepLog;

	public IQueryable<Legion.ADF.ESB.Orchestrations.Model.OrchestrationStepLog> AsReadOnlyQueryable(IScopeContext scopeContext)
		=> AsQueryable(scopeContext).AsNoTracking();
	
	public void Add(IScopeContext scopeContext, Legion.ADF.ESB.Orchestrations.Model.OrchestrationStepLog entity)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		dbContext.OrchestrationStepLog.Add(entity);
	}

	public void Remove(IScopeContext scopeContext, Legion.ADF.ESB.Orchestrations.Model.OrchestrationStepLog entity)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		dbContext.OrchestrationStepLog.Remove(entity);
	}
}
