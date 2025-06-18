using Legion.MessageBus.MessageHandlers;
using Legion.Model.Repositories;

namespace Legion.ADF.Messaging.Inbox.QueryHandlers.InboxMessageType;

public class GetInboxMessageTypeByNamespaceQueryHandler : AsyncMessageHandlerBase<Legion.ADF.Messaging.Inbox.Queries.InboxMessageType.GetInboxMessageTypeByNamespaceQuery, Legion.ADF.Messaging.Inbox.Model.InboxMessageType?>
{
	public override async Task<IResult<Legion.ADF.Messaging.Inbox.Model.InboxMessageType?>> HandleAsync(
		IInvocationContext invocationContext,
		Legion.ADF.Messaging.Inbox.Queries.InboxMessageType.GetInboxMessageTypeByNamespaceQuery query,
		Legion.Database.IConnectionProvider connectionProvider,
		CancellationToken cancellationToken = default)
	{
		invocationContext = invocationContext.InvocationCreateNew();

		var result = new ResultBuilder<Legion.ADF.Messaging.Inbox.Model.InboxMessageType?>();

		if (result.IsArgumentNull(invocationContext, query))
			return result.Build();

		if (result.IsArgumentNull(invocationContext, connectionProvider))
			return result.Build();

		var uowResult = connectionProvider.UnitOfWorkProvider.Create<IInboxUnitOfWork>(invocationContext);
		if (result.MergeHasError(uowResult))
			return result.Build();

		await using var uow = uowResult.Data!;
		var data = await uow.InboxMessageTypeRepository.GetInboxMessageTypeByNamespace(query with { AsNoTracking = true })
			.ToResultAsync(invocationContext, cancellationToken);

		return result.WithData(data).Build();
	}
}
