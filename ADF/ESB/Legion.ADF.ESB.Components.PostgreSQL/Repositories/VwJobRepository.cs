using Legion;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Legion.Exceptions;
using Legion.Transactions;
using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.ESB.Components.PostgreSQL.Model.Repositories;

public partial class VwJobRepository : Legion.ADF.ESB.Components.PostgreSQL.ComponentsQueryRepositoryBase, Legion.ADF.ESB.Components.IComponentsQueryRepository<Legion.ADF.ESB.Components.Model.VwJob>, Legion.ADF.ESB.Components.Model.Repositories.IVwJobRepository
{
	public VwJobRepository(IEFConnectionProvider connectionProvider, Legion.Model.Audit.IAuditEntryStore? auditEntryStore)
		: base(connectionProvider, auditEntryStore)
	{
	}

	private Legion.ADF.ESB.Components.PostgreSQL.IComponentsQueryDbContext? context;
	public IQueryable<Legion.ADF.ESB.Components.Model.VwJob> AsQueryable(IScopeContext scopeContext)
		=> (context ??= ConnectionProvider.GetOrCreateDbContext<Legion.ADF.ESB.Components.PostgreSQL.IComponentsQueryDbContext>(scopeContext, AuditEntryStore)).VwJob;

	public IQueryable<Legion.ADF.ESB.Components.Model.VwJob> AsReadOnlyQueryable(IScopeContext scopeContext)
		=> AsQueryable(scopeContext).AsNoTracking();
	
	public Legion.ADF.ESB.Components.Queries.VwJob.IGetVwJobById GetVwJobById(
		Legion.ADF.ESB.Components.Queries.VwJob.GetVwJobByIdQuery getVwJobByIdQuery)
		=> new Legion.ADF.ESB.Components.Queries.VwJob.GetVwJobById(
			ConnectionProvider,
			AuditEntryStore,
			getVwJobByIdQuery);
}
