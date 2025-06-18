using Legion.ADF.Logs.Queries.EnvironmentInfo;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.Logs.SqlServer.Queries.EnvironmentInfo;

public class GetEnvironmentInfoById :
	QueryDefinition<
		ILogsDbContext,
		Logs.Model.EnvironmentInfo,
		Logs.Model.EnvironmentInfo?,
		GetEnvironmentInfoByIdQuery>,
	IGetEnvironmentInfoById
{
	public GetEnvironmentInfoById(
		IEFConnectionProvider connectionProvider,
		GetEnvironmentInfoByIdQuery getEnvironmentInfoByIdQuery)
		: base(connectionProvider, getEnvironmentInfoByIdQuery)
	{
	}

	protected override IQueryable<Logs.Model.EnvironmentInfo> GetDefaultQuery(IScopeContext scopeContext)
	{
		var context = GetContext(scopeContext);
		return context.EnvironmentInfo;
	}

	public override IQueryable<Logs.Model.EnvironmentInfo> GetQuery(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return ApplyIncludesThenWhere<ILogsAccessControlManager>(
			scopeContext,
			QueryRequest.QueryableBuilder,
			QueryRequest.CheckReadPermissions,
			QueryRequest.AsNoTracking,
			u => u.IdEnvironmentInfo == QueryRequest.IdEnvironmentInfo);
	}

	public override async Task<Logs.Model.EnvironmentInfo?> ToResultAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return await GetQuery(scopeContext)
			.FirstOrDefaultAsync(cancellationToken);
	}

	public Logs.Model.EnvironmentInfo? ToResult(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return GetQuery(scopeContext)
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

		return GetQuery(scopeContext)
			.Any();
	}
}

