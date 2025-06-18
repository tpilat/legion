using Legion.MessageBus.MessageHandlers;
using Legion.Model.Repositories;

namespace Legion.ADF.Messaging.Inbox.QueryHandlers.InboxMessage;

public class ExistsInboxMessageByQueueMessageIdQueryHandler : AsyncMessageHandlerBase<Legion.ADF.Messaging.Inbox.Queries.InboxMessage.ExistsInboxMessageByQueueMessageIdQuery, bool>
{
	public override async Task<IResult<bool>> HandleAsync(
		IInvocationContext invocationContext,
		Legion.ADF.Messaging.Inbox.Queries.InboxMessage.ExistsInboxMessageByQueueMessageIdQuery query,
		Legion.Database.IConnectionProvider connectionProvider,
		CancellationToken cancellationToken = default)
	{
		invocationContext = invocationContext.InvocationCreateNew();

		var result = new ResultBuilder<bool>();

		if (result.IsArgumentNull(invocationContext, query))
			return result.Build();

		if (result.IsArgumentNull(invocationContext, connectionProvider))
			return result.Build();

		var uowResult = connectionProvider.UnitOfWorkProvider.Create<IInboxUnitOfWork>(invocationContext);
		if (result.MergeHasError(uowResult))
			return result.Build();

		await using var uow = uowResult.Data!;
		var data = await uow.InboxMessageRepository.ExistsInboxMessageByQueueMessageId(query with { AsNoTracking = true })
			.ToResultAsync(invocationContext, cancellationToken);

		return result.WithData(data).Build();
	}
}
