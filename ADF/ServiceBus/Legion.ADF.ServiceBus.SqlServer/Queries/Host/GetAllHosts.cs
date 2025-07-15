using Legion.ADF.ServiceBus.SqlServer;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.ServiceBus.Queries.Host;

public class GetAllHosts :
	QueryDefinition<
		IServiceBusDbContext,
		Model.Host,
		List<Model.Host>,
		GetAllHostsQuery>,
		IGetAllHosts
{
	public GetAllHosts(
		IEFConnectionProvider connectionProvider,
		GetAllHostsQuery getAllHosts)
		: base(connectionProvider, getAllHosts)
	{
	}

	protected override IQueryable<Model.Host> GetDefaultQuery(IScopeContext scopeContext)
	{
		var context = GetContext(scopeContext);
		return context.Host.Include(x => x.HostActivity);
	}

	public override IQueryable<Model.Host> GetQuery(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return ApplyIncludesThenWhere<IServiceBusAccessControlManager>(
			scopeContext,
			QueryRequest.QueryableBuilder,
			QueryRequest.CheckReadPermissions,
			QueryRequest.AsNoTracking,
			null);
	}

	public override async Task<List<Model.Host>> ToResultAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return await GetQuery(scopeContext)
			.ToListAsync(cancellationToken);
	}

	public List<Model.Host> ToResult(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return GetQuery(scopeContext).ToList();
	}
}
