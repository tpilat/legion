using Legion.ADF.ESB.Components.PostgreSQL;
using Legion.EntityFrameworkCore;
using Legion.Model.Audit;
using Legion.EntityFrameworkCore.Queries;
using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.ESB.Components.Queries.Adapter;

public class GetAdapterById :
	QueryDefinition<
		IComponentsDbContext,
		Components.Model.Adapter,
		Components.Model.Adapter?,
		GetAdapterByIdQuery>,
	IGetAdapterById
{
	public GetAdapterById(
		IEFConnectionProvider connectionProvider,
		IAuditEntryStore? auditEntryStore,
		GetAdapterByIdQuery getAdapterByIdQuery)
		: base(connectionProvider, auditEntryStore, getAdapterByIdQuery)
	{
	}

	protected override IQueryable<Components.Model.Adapter> GetDefaultQuery(IScopeContext scopeContext)
	{
		var context = GetContext(scopeContext);
		return context.Adapter;
	}

	public override IQueryable<Components.Model.Adapter> GetQuery(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return ApplyIncludesThenWhere(
			scopeContext,
			QueryRequest.QueryableBuilder,
			x => x.IdAdapter == QueryRequest.IdAdapter);
	}

	public override async Task<Components.Model.Adapter?> ToResultAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return await GetQuery(scopeContext)
			.FirstOrDefaultAsync(cancellationToken);
	}
}
