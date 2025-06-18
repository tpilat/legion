using Legion.MessageBus.MessageHandlers;
using Legion.Model.Repositories;

namespace Legion.ADF.Messaging.Outbox.QueryHandlers.VwOutboxQueue;

public class GetVwOutboxQueueByIdQueryHandler : AsyncMessageHandlerBase<Legion.ADF.Messaging.Outbox.Queries.VwOutboxQueue.GetVwOutboxQueueByIdQuery, Legion.ADF.Messaging.Outbox.Model.VwOutboxQueue?>
{
	public override async Task<IResult<Legion.ADF.Messaging.Outbox.Model.VwOutboxQueue?>> HandleAsync(
		IInvocationContext invocationContext,
		Legion.ADF.Messaging.Outbox.Queries.VwOutboxQueue.GetVwOutboxQueueByIdQuery query,
		Legion.Database.IConnectionProvider connectionProvider,
		CancellationToken cancellationToken = default)
	{
		invocationContext = invocationContext.InvocationCreateNew();

		var result = new ResultBuilder<Legion.ADF.Messaging.Outbox.Model.VwOutboxQueue?>();

		if (result.IsArgumentNull(invocationContext, query))
			return result.Build();

		if (result.IsArgumentNull(invocationContext, connectionProvider))
			return result.Build();

		var uowResult = connectionProvider.UnitOfWorkProvider.CreateQuery<IOutboxQueryUnitOfWork>(invocationContext);
		if (result.MergeHasError(uowResult))
			return result.Build();

		await using var uow = uowResult.Data!;
		var data = await uow.VwOutboxQueueRepository.GetVwOutboxQueueById(query with { AsNoTracking = true })
			.ToResultAsync(invocationContext, cancellationToken);

		return result.WithData(data).Build();
	}
}
