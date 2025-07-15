using Legion.ADF.ServiceBus.SqlServer;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.ServiceBus.Queries.HostLog;

public class GetHostLogsByIdHost :
	QueryDefinition<
		IServiceBusDbContext,
		Model.HostLog,
		List<Model.HostLog>,
		GetHostLogsByIdHostQuery>,
		IGetHostLogsByIdHost
{
	public GetHostLogsByIdHost(
		IEFConnectionProvider connectionProvider,
		GetHostLogsByIdHostQuery getHostLogsByIdHostQuery)
		: base(connectionProvider, getHostLogsByIdHostQuery)
	{
	}

	protected override IQueryable<Model.HostLog> GetDefaultQuery(IScopeContext scopeContext)
	{
		var context = GetContext(scopeContext);
		return context.HostLog;
	}

	public override IQueryable<Model.HostLog> GetQuery(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return ApplyIncludesThenWhere<IServiceBusAccessControlManager>(
			scopeContext,
			QueryRequest.QueryableBuilder,
			QueryRequest.CheckReadPermissions,
			QueryRequest.AsNoTracking,
			x => x.IdHost == QueryRequest.IdHost
				&& QueryRequest.From <= x.CreatedUtc
				&& x.CreatedUtc <= QueryRequest.To);
	}

	public override async Task<List<Model.HostLog>> ToResultAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return await GetQuery(scopeContext)
			.ToListAsync(cancellationToken);
	}

	public List<Model.HostLog> ToResult(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return GetQuery(scopeContext).ToList();
	}
}
