using Legion;
using Legion.MessageBus.MessageHandlers;
using Legion.Model.Repositories;

namespace Legion.ADF.ServiceBus.QueryHandlers.JobLog;

public class GetJobLogsByIdJobQueryHandler : AsyncMessageHandlerBase<Legion.ADF.ServiceBus.Queries.JobLog.GetJobLogsByIdJobQuery, System.Collections.Generic.List<Legion.ADF.ServiceBus.Model.JobLog>>
{
	public override async Task<IResult<System.Collections.Generic.List<Legion.ADF.ServiceBus.Model.JobLog>>> HandleAsync(
		IInvocationContext invocationContext,
		Legion.ADF.ServiceBus.Queries.JobLog.GetJobLogsByIdJobQuery query,
		Legion.Database.IConnectionProvider connectionProvider,
		CancellationToken cancellationToken = default)
	{
		invocationContext = invocationContext.InvocationCreateNew();

		var result = new ResultBuilder<System.Collections.Generic.List<Legion.ADF.ServiceBus.Model.JobLog>>();

		if (result.IsArgumentNull(invocationContext, query))
			return result.Build();

		if (result.IsArgumentNull(invocationContext, connectionProvider))
			return result.Build();

		var uowResult = connectionProvider.UnitOfWorkProvider.Create<IServiceBusUnitOfWork>(invocationContext);
		if (result.MergeHasError(uowResult))
			return result.Build();

		await using var uow = uowResult.Data!;
		var data = await uow.JobLogRepository.GetJobLogsByIdJob(query with { AsNoTracking = true })
			.ToResultAsync(invocationContext, cancellationToken);

		return result.WithData(data).Build();
	}
}
