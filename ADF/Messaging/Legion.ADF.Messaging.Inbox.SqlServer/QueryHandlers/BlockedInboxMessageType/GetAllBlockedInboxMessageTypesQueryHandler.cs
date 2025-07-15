using Legion;
using Legion.MessageBus.MessageHandlers;
using Legion.Model.Repositories;

namespace Legion.ADF.Messaging.Inbox.QueryHandlers.BlockedInboxMessageType;

public class GetAllBlockedInboxMessageTypesQueryHandler : AsyncMessageHandlerBase<Legion.ADF.Messaging.Inbox.Queries.BlockedInboxMessageType.GetAllBlockedInboxMessageTypesQuery, System.Collections.Generic.List<Legion.ADF.Messaging.Inbox.Model.BlockedInboxMessageType>>
{
	public override async Task<IResult<System.Collections.Generic.List<Legion.ADF.Messaging.Inbox.Model.BlockedInboxMessageType>>> HandleAsync(
		IInvocationContext invocationContext,
		Legion.ADF.Messaging.Inbox.Queries.BlockedInboxMessageType.GetAllBlockedInboxMessageTypesQuery query,
		Legion.Database.IConnectionProvider connectionProvider,
		CancellationToken cancellationToken = default)
	{
		invocationContext = invocationContext.InvocationCreateNew();

		var result = new ResultBuilder<System.Collections.Generic.List<Legion.ADF.Messaging.Inbox.Model.BlockedInboxMessageType>>();

		if (result.IsArgumentNull(invocationContext, query))
			return result.Build();

		if (result.IsArgumentNull(invocationContext, connectionProvider))
			return result.Build();

		var uowResult = connectionProvider.UnitOfWorkProvider.Create<IInboxUnitOfWork>(invocationContext);
		if (result.MergeHasError(uowResult))
			return result.Build();

		await using var uow = uowResult.Data!;
		var data = await uow.BlockedInboxMessageTypeRepository.GetAllBlockedInboxMessageTypes(query with { AsNoTracking = true })
			.ToResultAsync(invocationContext, cancellationToken);

		return result.WithData(data).Build();
	}
}
