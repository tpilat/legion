using Legion.ADF.ESB.Components.PostgreSQL;
using Legion.EntityFrameworkCore;
using Legion.Model.Audit;
using Legion.EntityFrameworkCore.Queries;
using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.ESB.Components.Queries.Adapter;

public class GetAllAdapters :
	QueryDefinition<
		IComponentsDbContext,
		Components.Model.Adapter,
		List<Components.Model.Adapter>,
		GetAllAdaptersQuery>,
	IGetAllAdapters
{
	public GetAllAdapters(
		IEFConnectionProvider connectionProvider,
		IAuditEntryStore? auditEntryStore,
		GetAllAdaptersQuery getAllAdaptersQuery)
		: base(connectionProvider, auditEntryStore, getAllAdaptersQuery)
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
			null);
	}

	public override async Task<List<Components.Model.Adapter>> ToResultAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return await GetQuery(scopeContext)
			.ToListAsync(cancellationToken);
	}
}
