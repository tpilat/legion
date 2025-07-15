using Legion.ADF.ServiceBus.PostgreSQL;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.ServiceBus.Queries.JobStatistics;

public class GetJobStatisticsByJobIdAndStartHour :
	QueryDefinition<
		IServiceBusDbContext,
		Model.JobStatistics,
		Model.JobStatistics?,
		GetJobStatisticsByJobIdAndStartHourQuery>,
		IGetJobStatisticsByJobIdAndStartHour
{
	public GetJobStatisticsByJobIdAndStartHour(
		IEFConnectionProvider connectionProvider,
		GetJobStatisticsByJobIdAndStartHourQuery getJobStatisticsByIdAndStartHour)
		: base(connectionProvider, getJobStatisticsByIdAndStartHour)
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
				&& x.StartHourUtc == QueryRequest.StartHourUtc);
	}

	public override async Task<Model.JobStatistics?> ToResultAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return await GetQuery(scopeContext)
			.FirstOrDefaultAsync(cancellationToken);
	}

	public Model.JobStatistics? ToResult(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return GetQuery(scopeContext).FirstOrDefault();
	}
}
