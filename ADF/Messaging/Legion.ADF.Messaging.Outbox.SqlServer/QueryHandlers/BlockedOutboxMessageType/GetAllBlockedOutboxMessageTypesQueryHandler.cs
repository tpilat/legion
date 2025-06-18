using Legion.MessageBus.MessageHandlers;
using Legion.Model.Repositories;

namespace Legion.ADF.Messaging.Outbox.QueryHandlers.BlockedOutboxMessageType;

public class GetAllBlockedOutboxMessageTypesQueryHandler : AsyncMessageHandlerBase<Legion.ADF.Messaging.Outbox.Queries.BlockedOutboxMessageType.GetAllBlockedOutboxMessageTypesQuery, System.Collections.Generic.List<Legion.ADF.Messaging.Outbox.Model.BlockedOutboxMessageType>>
{
	public override async Task<IResult<System.Collections.Generic.List<Legion.ADF.Messaging.Outbox.Model.BlockedOutboxMessageType>>> HandleAsync(
		IInvocationContext invocationContext,
		Legion.ADF.Messaging.Outbox.Queries.BlockedOutboxMessageType.GetAllBlockedOutboxMessageTypesQuery query,
		Legion.Database.IConnectionProvider connectionProvider,
		CancellationToken cancellationToken = default)
	{
		invocationContext = invocationContext.InvocationCreateNew();

		var result = new ResultBuilder<System.Collections.Generic.List<Legion.ADF.Messaging.Outbox.Model.BlockedOutboxMessageType>>();

		if (result.IsArgumentNull(invocationContext, query))
			return result.Build();

		if (result.IsArgumentNull(invocationContext, connectionProvider))
			return result.Build();

		var uowResult = connectionProvider.UnitOfWorkProvider.Create<IOutboxUnitOfWork>(invocationContext);
		if (result.MergeHasError(uowResult))
			return result.Build();

		await using var uow = uowResult.Data!;
		var data = await uow.BlockedOutboxMessageTypeRepository.GetAllBlockedOutboxMessageTypes(query with { AsNoTracking = true })
			.ToResultAsync(invocationContext, cancellationToken);

		return result.WithData(data).Build();
	}
}
