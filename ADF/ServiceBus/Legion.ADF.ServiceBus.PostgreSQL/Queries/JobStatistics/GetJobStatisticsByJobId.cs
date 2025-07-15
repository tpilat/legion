using Legion.ADF.ServiceBus.PostgreSQL;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.ServiceBus.Queries.JobStatistics;

public class GetJobStatisticsByJobId :
	QueryDefinition<
		IServiceBusDbContext,
		Model.JobStatistics,
		List<Model.JobStatistics>,
		GetJobStatisticsByJobIdQuery>,
		IGetJobStatisticsByJobId
{
	public GetJobStatisticsByJobId(
		IEFConnectionProvider connectionProvider,
		GetJobStatisticsByJobIdQuery getJobStatisticsById)
		: base(connectionProvider, getJobStatisticsById)
	{
	}

	protected override IQueryable<Model.JobStatistics> GetDefaultQuery(IScopeContext scopeContext)
	{
		var context = GetContext(scopeContext);
		return context.JobStatistics;
	}

	public override IQueryable<Model.JobStatistics> GetQuery(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return ApplyIncludesThenWhere<IServiceBusAccessControlManager>(
			scopeContext,
			QueryRequest.QueryableBuilder,
			QueryRequest.CheckReadPermissions,
			QueryRequest.AsNoTracking,
			x => x.IdJob == QueryRequest.IdJob
				&& QueryRequest.FromUtc <= x.StartHourUtc
				&& (!QueryRequest.ToUtc.HasValue || x.StartHourUtc <= QueryRequest.ToUtc));
	}

	public override async Task<List<Model.JobStatistics>> ToResultAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return await GetQuery(scopeContext)
			.ToListAsync(cancellationToken);
	}

	public List<Model.JobStatistics> ToResult(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return GetQuery(scopeContext).ToList();
	}
}
