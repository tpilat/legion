using Legion;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Legion.Exceptions;
using Legion.Transactions;
using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.ESB.Orchestrations.PostgreSQL.Model.Repositories;

public partial class VwOrchestrationRepository : Legion.ADF.ESB.Orchestrations.PostgreSQL.OrchestrationsQueryRepositoryBase, Legion.ADF.ESB.Orchestrations.IOrchestrationsQueryRepository<Legion.ADF.ESB.Orchestrations.Model.VwOrchestration>, Legion.ADF.ESB.Orchestrations.Model.Repositories.IVwOrchestrationRepository
{
	public VwOrchestrationRepository(IEFConnectionProvider connectionProvider, Legion.Model.Audit.IAuditEntryStore? auditEntryStore)
		: base(connectionProvider, auditEntryStore)
	{
	}

	private Legion.ADF.ESB.Orchestrations.PostgreSQL.IOrchestrationsQueryDbContext? context;
	public IQueryable<Legion.ADF.ESB.Orchestrations.Model.VwOrchestration> AsQueryable(IScopeContext scopeContext)
		=> (context ??= ConnectionProvider.GetOrCreateDbContext<Legion.ADF.ESB.Orchestrations.PostgreSQL.IOrchestrationsQueryDbContext>(scopeContext, AuditEntryStore)).VwOrchestration;

	public IQueryable<Legion.ADF.ESB.Orchestrations.Model.VwOrchestration> AsReadOnlyQueryable(IScopeContext scopeContext)
		=> AsQueryable(scopeContext).AsNoTracking();
	}
