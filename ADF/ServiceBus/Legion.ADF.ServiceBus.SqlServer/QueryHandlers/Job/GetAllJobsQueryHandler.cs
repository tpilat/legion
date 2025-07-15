using Legion;
using Legion.MessageBus.MessageHandlers;
using Legion.Model.Repositories;

namespace Legion.ADF.ServiceBus.QueryHandlers.Job;

public class GetAllJobsQueryHandler : AsyncMessageHandlerBase<Legion.ADF.ServiceBus.Queries.Job.GetAllJobsQuery, System.Collections.Generic.List<Legion.ADF.ServiceBus.Model.Job>>
{
	public override async Task<IResult<System.Collections.Generic.List<Legion.ADF.ServiceBus.Model.Job>>> HandleAsync(
		IInvocationContext invocationContext,
		Legion.ADF.ServiceBus.Queries.Job.GetAllJobsQuery query,
		Legion.Database.IConnectionProvider connectionProvider,
		CancellationToken cancellationToken = default)
	{
		invocationContext = invocationContext.InvocationCreateNew();

		var result = new ResultBuilder<System.Collections.Generic.List<Legion.ADF.ServiceBus.Model.Job>>();

		if (result.IsArgumentNull(invocationContext, query))
			return result.Build();

		if (result.IsArgumentNull(invocationContext, connectionProvider))
			return result.Build();

		var uowResult = connectionProvider.UnitOfWorkProvider.Create<IServiceBusUnitOfWork>(invocationContext);
		if (result.MergeHasError(uowResult))
			return result.Build();

		await using var uow = uowResult.Data!;
		var data = await uow.JobRepository.GetAllJobs(query with { AsNoTracking = true })
			.ToResultAsync(invocationContext, cancellationToken);

		return result.WithData(data).Build();
	}
}
