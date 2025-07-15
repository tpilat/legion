using Legion.ADF.ServiceBus.PostgreSQL;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.ServiceBus.Queries.JobExecution;

public class GetJobExecutionById :
	QueryDefinition<
		IServiceBusDbContext,
		Model.JobExecution,
		List<Model.JobExecution>,
		GetJobExecutionByIdQuery>,
		IGetJobExecutionById
{
	public GetJobExecutionById(
		IEFConnectionProvider connectionProvider,
		GetJobExecutionByIdQuery getJobExecutionById)
		: base(connectionProvider, getJobExecutionById)
	{
	}

	protected override IQueryable<Model.JobExecution> GetDefaultQuery(IScopeContext scopeContext)
	{
		var context = GetContext(scopeContext);
		return context.JobExecution;
	}

	public override IQueryable<Model.JobExecution> GetQuery(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return ApplyIncludesThenWhere<IServiceBusAccessControlManager>(
			scopeContext,
			QueryRequest.QueryableBuilder,
			QueryRequest.CheckReadPermissions,
			QueryRequest.AsNoTracking,
			x => x.IdJob == QueryRequest.IdJob
				&& QueryRequest.FromUtc <= x.StartUtc
				&& x.StartUtc <= QueryRequest.ToUtc);
	}

	public override async Task<List<Model.JobExecution>> ToResultAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return await GetQuery(scopeContext)
			.ToListAsync(cancellationToken);
	}

	public List<Model.JobExecution> ToResult(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return GetQuery(scopeContext).ToList();
	}
}
