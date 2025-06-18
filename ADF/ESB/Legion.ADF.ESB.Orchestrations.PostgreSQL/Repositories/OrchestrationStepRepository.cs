using Legion;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Legion.Exceptions;
using Legion.Transactions;
using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.ESB.Orchestrations.PostgreSQL.Model.Repositories;

public partial class OrchestrationStepRepository : Legion.ADF.ESB.Orchestrations.PostgreSQL.OrchestrationsRepositoryBase, Legion.ADF.ESB.Orchestrations.IOrchestrationsRepository<Legion.ADF.ESB.Orchestrations.Model.OrchestrationStep>, Legion.ADF.ESB.Orchestrations.Model.Repositories.IOrchestrationStepRepository
{
	public OrchestrationStepRepository(IEFConnectionProvider connectionProvider, Legion.Model.Audit.IAuditEntryStore? auditEntryStore)
		: base(connectionProvider, auditEntryStore)
	{
	}

	private Legion.ADF.ESB.Orchestrations.PostgreSQL.IOrchestrationsDbContext? context;
	public IQueryable<Legion.ADF.ESB.Orchestrations.Model.OrchestrationStep> AsQueryable(IScopeContext scopeContext)
		=> (context ??= ConnectionProvider.GetOrCreateDbContext<Legion.ADF.ESB.Orchestrations.PostgreSQL.IOrchestrationsDbContext>(scopeContext, AuditEntryStore)).OrchestrationStep;

	public IQueryable<Legion.ADF.ESB.Orchestrations.Model.OrchestrationStep> AsReadOnlyQueryable(IScopeContext scopeContext)
		=> AsQueryable(scopeContext).AsNoTracking();
	
	public void Add(IScopeContext scopeContext, Legion.ADF.ESB.Orchestrations.Model.OrchestrationStep entity)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		dbContext.OrchestrationStep.Add(entity);
	}

	public void Remove(IScopeContext scopeContext, Legion.ADF.ESB.Orchestrations.Model.OrchestrationStep entity)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		dbContext.OrchestrationStep.Remove(entity);
	}
}
