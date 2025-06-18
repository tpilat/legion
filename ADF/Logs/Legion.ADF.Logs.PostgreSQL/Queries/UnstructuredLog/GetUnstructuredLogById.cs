using Legion.ADF.Logs.Queries.UnstructuredLog;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.Logs.PostgreSQL.Queries.UnstructuredLog;

public class GetUnstructuredLogById :
	QueryDefinition<
		ILogsDbContext,
		Logs.Model.UnstructuredLog,
		Logs.Model.UnstructuredLog?,
		GetUnstructuredLogByIdQuery>,
	IGetUnstructuredLogById
{
	public GetUnstructuredLogById(
		IEFConnectionProvider connectionProvider,
		GetUnstructuredLogByIdQuery getUnstructuredLogByIdQuery)
		: base(connectionProvider, getUnstructuredLogByIdQuery)
	{
	}

	protected override IQueryable<Logs.Model.UnstructuredLog> GetDefaultQuery(IScopeContext scopeContext)
	{
		var context = GetContext(scopeContext);
		return context.UnstructuredLog;
	}

	public override IQueryable<Logs.Model.UnstructuredLog> GetQuery(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return ApplyIncludesThenWhere<ILogsAccessControlManager>(
			scopeContext,
			QueryRequest.QueryableBuilder,
			QueryRequest.CheckReadPermissions,
			QueryRequest.AsNoTracking,
			u => u.IdUnstructuredLog == QueryRequest.IdUnstructuredLog);
	}

	public override async Task<Logs.Model.UnstructuredLog?> ToResultAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return await GetQuery(scopeContext)
			.FirstOrDefaultAsync(cancellationToken);
	}
}

