using Legion.ADF.ESB.Components.PostgreSQL;
using Legion.EntityFrameworkCore;
using Legion.Model.Audit;
using Legion.EntityFrameworkCore.Queries;
using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.ESB.Components.Queries.VwJob;

public class GetVwJobById :
	QueryDefinition<
		IComponentsQueryDbContext,
		Components.Model.VwJob,
		Components.Model.VwJob?,
		GetVwJobByIdQuery>,
	IGetVwJobById
{
	public GetVwJobById(
		IEFConnectionProvider connectionProvider,
		IAuditEntryStore? auditEntryStore,
		GetVwJobByIdQuery getVwJobByIdQuery)
		: base(connectionProvider, auditEntryStore, getVwJobByIdQuery)
	{
	}

	protected override IQueryable<Components.Model.VwJob> GetDefaultQuery(IScopeContext scopeContext)
	{
		var context = GetContext(scopeContext);
		return context.VwJob;
	}

	public override IQueryable<Components.Model.VwJob> GetQuery(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return ApplyIncludesThenWhere(
			scopeContext,
			QueryRequest.QueryableBuilder,
			x => x.IdJob == QueryRequest.IdJob);
	}

	public override async Task<Components.Model.VwJob?> ToResultAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return await GetQuery(scopeContext)
			.FirstOrDefaultAsync(cancellationToken);
	}
}
