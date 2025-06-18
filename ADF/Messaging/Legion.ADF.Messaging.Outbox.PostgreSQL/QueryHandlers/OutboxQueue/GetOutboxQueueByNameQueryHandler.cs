using Legion.MessageBus.MessageHandlers;
using Legion.Model.Repositories;

namespace Legion.ADF.Messaging.Outbox.QueryHandlers.OutboxQueue;

public class GetOutboxQueueByNameQueryHandler : AsyncMessageHandlerBase<Legion.ADF.Messaging.Outbox.Queries.OutboxQueue.GetOutboxQueueByNameQuery, Legion.ADF.Messaging.Outbox.Model.OutboxQueue?>
{
	public override async Task<IResult<Legion.ADF.Messaging.Outbox.Model.OutboxQueue?>> HandleAsync(
		IInvocationContext invocationContext,
		Legion.ADF.Messaging.Outbox.Queries.OutboxQueue.GetOutboxQueueByNameQuery query,
		Legion.Database.IConnectionProvider connectionProvider,
		CancellationToken cancellationToken = default)
	{
		invocationContext = invocationContext.InvocationCreateNew();

		var result = new ResultBuilder<Legion.ADF.Messaging.Outbox.Model.OutboxQueue?>();

		if (result.IsArgumentNull(invocationContext, query))
			return result.Build();

		if (result.IsArgumentNull(invocationContext, connectionProvider))
			return result.Build();

		var uowResult = connectionProvider.UnitOfWorkProvider.Create<IOutboxUnitOfWork>(invocationContext);
		if (result.MergeHasError(uowResult))
			return result.Build();

		await using var uow = uowResult.Data!;
		var data = await uow.OutboxQueueRepository.GetOutboxQueueByName(query with { AsNoTracking = true })
			.ToResultAsync(invocationContext, cancellationToken);

		return result.WithData(data).Build();
	}
}
