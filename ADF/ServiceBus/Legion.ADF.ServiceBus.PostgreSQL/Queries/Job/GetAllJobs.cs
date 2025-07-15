using Legion.ADF.ServiceBus.PostgreSQL;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.ServiceBus.Queries.Job;

public class GetAllJobs :
	QueryDefinition<
		IServiceBusDbContext,
		Model.Job,
		List<Model.Job>,
		GetAllJobsQuery>,
		IGetAllJobs
{
	public GetAllJobs(
		IEFConnectionProvider connectionProvider,
		GetAllJobsQuery getAllJobs)
		: base(connectionProvider, getAllJobs)
	{
	}

	protected override IQueryable<Model.Job> GetDefaultQuery(IScopeContext scopeContext)
	{
		var context = GetContext(scopeContext);
		return context.Job.Include(x => x.JobActivity);
	}

	public override IQueryable<Model.Job> GetQuery(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return ApplyIncludesThenWhere<IServiceBusAccessControlManager>(
			scopeContext,
			QueryRequest.QueryableBuilder,
			QueryRequest.CheckReadPermissions,
			QueryRequest.AsNoTracking,
			null);
	}

	public override async Task<List<Model.Job>> ToResultAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return await GetQuery(scopeContext)
			.ToListAsync(cancellationToken);
	}

	public List<Model.Job> ToResult(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return GetQuery(scopeContext).ToList();
	}
}
