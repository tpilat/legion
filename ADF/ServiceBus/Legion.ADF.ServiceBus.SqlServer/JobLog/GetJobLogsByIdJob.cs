using Legion.ADF.ServiceBus.SqlServer;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.ServiceBus.Queries.JobLog;

public class GetJobLogsByIdJob :
	QueryDefinition<
		IServiceBusDbContext,
		Model.JobLog,
		List<Model.JobLog>,
		GetJobLogsByIdJobQuery>,
		IGetJobLogsByIdJob
{
	public GetJobLogsByIdJob(
		IEFConnectionProvider connectionProvider,
		GetJobLogsByIdJobQuery getJobLogsByIdJobQuery)
		: base(connectionProvider, getJobLogsByIdJobQuery)
	{
	}

	protected override IQueryable<Model.JobLog> GetDefaultQuery(IScopeContext scopeContext)
	{
		var context = GetContext(scopeContext);
		return context.JobLog;
	}

	public override IQueryable<Model.JobLog> GetQuery(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		if (QueryRequest.IdJobExecution.HasValue)
		{
			return ApplyIncludesThenWhere<IServiceBusAccessControlManager>(
				scopeContext,
				QueryRequest.QueryableBuilder,
				QueryRequest.CheckReadPermissions,
				QueryRequest.AsNoTracking,
				x => x.IdJob == QueryRequest.IdJob
					&& x.IdJobExecution == QueryRequest.IdJobExecution
					&& QueryRequest.From <= x.CreatedUtc
					&& x.CreatedUtc <= QueryRequest.To);
		}
		else
		{
			return ApplyIncludesThenWhere<IServiceBusAccessControlManager>(
				scopeContext,
				QueryRequest.QueryableBuilder,
				QueryRequest.CheckReadPermissions,
				QueryRequest.AsNoTracking,
				x => x.IdJob == QueryRequest.IdJob
					&& QueryRequest.From <= x.CreatedUtc
					&& x.CreatedUtc <= QueryRequest.To);
		}
	}

	public override async Task<List<Model.JobLog>> ToResultAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return await GetQuery(scopeContext)
			.ToListAsync(cancellationToken);
	}

	public List<Model.JobLog> ToResult(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return GetQuery(scopeContext).ToList();
	}
}
