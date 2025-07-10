using Legion.ADF.ServiceBus.Hosts.SqlServer;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.ServiceBus.Hosts.Queries.Host;

public class GetHostByName :
	QueryDefinition<
		IHostsDbContext,
		Model.Host,
		Model.Host?,
		GetHostByNameQuery>,
		IGetHostByName
{
	public GetHostByName(
		IEFConnectionProvider connectionProvider,
		GetHostByNameQuery getHostByName)
		: base(connectionProvider, getHostByName)
	{
		Throw.IfArgumentNullOrWhiteSpace(getHostByName?.Name);
	}

	protected override IQueryable<Model.Host> GetDefaultQuery(IScopeContext scopeContext)
	{
		var context = GetContext(scopeContext);
		return context.Host;
	}

	public override IQueryable<Model.Host> GetQuery(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return
			QueryRequest.GetDisabledHost
			? ApplyIncludesThenWhere<IServiceBusAccessControlManager>(
				scopeContext,
				QueryRequest.QueryableBuilder,
				QueryRequest.CheckReadPermissions,
				QueryRequest.AsNoTracking,
				x => x.Name == QueryRequest.Name)
			: ApplyIncludesThenWhere<IServiceBusAccessControlManager>(
				scopeContext,
				QueryRequest.QueryableBuilder,
				QueryRequest.CheckReadPermissions,
				QueryRequest.AsNoTracking,
				x => x.Name == QueryRequest.Name && x.IsEnabled);
	}

	public override async Task<Model.Host?> ToResultAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return await GetQuery(scopeContext)
			.FirstOrDefaultAsync(cancellationToken);
	}

	public Model.Host? ToResult(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return GetQuery(scopeContext).FirstOrDefault();
	}

	public async Task<Guid?> GetIdAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return await GetQuery(scopeContext)
			.Select(x => x.IdHost)
			.FirstOrDefaultAsync(cancellationToken);
	}

	public Guid? GetId(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return GetQuery(scopeContext)
			.Select(x => x.IdHost)
			.FirstOrDefault();
	}

	public async Task<bool> ExistsAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return await GetQuery(scopeContext)
			.AnyAsync(cancellationToken);
	}

	public bool Exists(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return GetQuery(scopeContext).Any();
	}
}
