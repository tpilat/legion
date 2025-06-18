using Legion.ADF.Logs.Queries.Log;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.Logs.SqlServer.Queries.Log;

public class GetLogById :
	QueryDefinition<
		ILogsDbContext,
		Logs.Model.Log,
		Logs.Model.Log?,
		GetLogByIdQuery>,
	IGetLogById
{
	public GetLogById(
		IEFConnectionProvider connectionProvider,
		GetLogByIdQuery getLogByIdQuery)
		: base(connectionProvider, getLogByIdQuery)
	{
	}

	protected override IQueryable<Logs.Model.Log> GetDefaultQuery(IScopeContext scopeContext)
	{
		var context = GetContext(scopeContext);
		return context.Log;
	}

	public override IQueryable<Logs.Model.Log> GetQuery(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return ApplyIncludesThenWhere<ILogsAccessControlManager>(
			scopeContext,
			QueryRequest.QueryableBuilder,
			QueryRequest.CheckReadPermissions,
			QueryRequest.AsNoTracking,
			u => u.IdLog == QueryRequest.IdLog);
	}

	public override async Task<Logs.Model.Log?> ToResultAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return await GetQuery(scopeContext)
			.FirstOrDefaultAsync(cancellationToken);
	}
}

