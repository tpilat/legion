using Legion.MessageBus.MessageHandlers;
using Legion.Model.Repositories;

namespace Legion.ADF.Messaging.Outbox.QueryHandlers.OutboxMessage;

public class GetNextOutboxMessagesByQueueQueryHandler : AsyncMessageHandlerBase<Legion.ADF.Messaging.Outbox.Queries.OutboxMessage.GetNextOutboxMessagesByQueueQuery, System.Collections.Generic.List<Legion.ADF.Messaging.Outbox.Model.OutboxMessage>>
{
	public override async Task<IResult<System.Collections.Generic.List<Legion.ADF.Messaging.Outbox.Model.OutboxMessage>>> HandleAsync(
		IInvocationContext invocationContext,
		Legion.ADF.Messaging.Outbox.Queries.OutboxMessage.GetNextOutboxMessagesByQueueQuery query,
		Legion.Database.IConnectionProvider connectionProvider,
		CancellationToken cancellationToken = default)
	{
		invocationContext = invocationContext.InvocationCreateNew();

		var result = new ResultBuilder<System.Collections.Generic.List<Legion.ADF.Messaging.Outbox.Model.OutboxMessage>>();

		if (result.IsArgumentNull(invocationContext, query))
			return result.Build();

		if (result.IsArgumentNull(invocationContext, connectionProvider))
			return result.Build();

		var uowResult = connectionProvider.UnitOfWorkProvider.Create<IOutboxUnitOfWork>(invocationContext);
		if (result.MergeHasError(uowResult))
			return result.Build();

		await using var uow = uowResult.Data!;
		var data = await uow.OutboxMessageRepository.GetNextOutboxMessagesByQueue(query with { AsNoTracking = true })
			.ToResultAsync(invocationContext, cancellationToken);

		return result.WithData(data).Build();
	}
}
