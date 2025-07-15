using Legion.ADF.ServiceBus.PostgreSQL;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.ServiceBus.Queries.Job;

public class GetJobById :
	QueryDefinition<
		IServiceBusDbContext,
		Model.Job,
		Model.Job?,
		GetJobByIdQuery>,
		IGetJobById
{
	public GetJobById(
		IEFConnectionProvider connectionProvider,
		GetJobByIdQuery getJobById)
		: base(connectionProvider, getJobById)
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
			x => x.IdJob == QueryRequest.IdJob);
	}

	public override async Task<Model.Job?> ToResultAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return await GetQuery(scopeContext)
			.FirstOrDefaultAsync(cancellationToken);
	}

	public Model.Job? ToResult(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return GetQuery(scopeContext).FirstOrDefault();
	}
}
