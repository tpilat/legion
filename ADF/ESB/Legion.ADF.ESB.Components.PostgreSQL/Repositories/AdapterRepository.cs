using Legion;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Legion.Exceptions;
using Legion.Transactions;
using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.ESB.Components.PostgreSQL.Model.Repositories;

public partial class AdapterRepository : Legion.ADF.ESB.Components.PostgreSQL.ComponentsRepositoryBase, Legion.ADF.ESB.Components.IComponentsRepository<Legion.ADF.ESB.Components.Model.Adapter>, Legion.ADF.ESB.Components.Model.Repositories.IAdapterRepository
{
	public AdapterRepository(IEFConnectionProvider connectionProvider, Legion.Model.Audit.IAuditEntryStore? auditEntryStore)
		: base(connectionProvider, auditEntryStore)
	{
	}

	private Legion.ADF.ESB.Components.PostgreSQL.IComponentsDbContext? context;
	public IQueryable<Legion.ADF.ESB.Components.Model.Adapter> AsQueryable(IScopeContext scopeContext)
		=> (context ??= ConnectionProvider.GetOrCreateDbContext<Legion.ADF.ESB.Components.PostgreSQL.IComponentsDbContext>(scopeContext, AuditEntryStore)).Adapter;

	public IQueryable<Legion.ADF.ESB.Components.Model.Adapter> AsReadOnlyQueryable(IScopeContext scopeContext)
		=> AsQueryable(scopeContext).AsNoTracking();
	
	public Legion.ADF.ESB.Components.Queries.Adapter.IGetAdapterById GetAdapterById(
		Legion.ADF.ESB.Components.Queries.Adapter.GetAdapterByIdQuery getAdapterByIdQuery)
		=> new Legion.ADF.ESB.Components.Queries.Adapter.GetAdapterById(
			ConnectionProvider,
			AuditEntryStore,
			getAdapterByIdQuery);

	public Legion.ADF.ESB.Components.Queries.Adapter.IGetAllAdapters GetAllAdapters(
		Legion.ADF.ESB.Components.Queries.Adapter.GetAllAdaptersQuery getAllAdaptersQuery)
		=> new Legion.ADF.ESB.Components.Queries.Adapter.GetAllAdapters(
			ConnectionProvider,
			AuditEntryStore,
			getAllAdaptersQuery);

	public void Add(IScopeContext scopeContext, Legion.ADF.ESB.Components.Model.Adapter entity)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		dbContext.Adapter.Add(entity);
	}

	public void Remove(IScopeContext scopeContext, Legion.ADF.ESB.Components.Model.Adapter entity)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		dbContext.Adapter.Remove(entity);
	}
}
