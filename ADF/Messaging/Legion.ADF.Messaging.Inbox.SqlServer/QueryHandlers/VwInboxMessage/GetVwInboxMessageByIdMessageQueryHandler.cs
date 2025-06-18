using Legion.MessageBus.MessageHandlers;
using Legion.Model.Repositories;

namespace Legion.ADF.Messaging.Inbox.QueryHandlers.VwInboxMessage;

public class GetVwInboxMessageByIdMessageQueryHandler : AsyncMessageHandlerBase<Legion.ADF.Messaging.Inbox.Queries.VwInboxMessage.GetVwInboxMessageByIdMessageQuery, Legion.ADF.Messaging.Inbox.Model.VwInboxMessage?>
{
	public override async Task<IResult<Legion.ADF.Messaging.Inbox.Model.VwInboxMessage?>> HandleAsync(
		IInvocationContext invocationContext,
		Legion.ADF.Messaging.Inbox.Queries.VwInboxMessage.GetVwInboxMessageByIdMessageQuery query,
		Legion.Database.IConnectionProvider connectionProvider,
		CancellationToken cancellationToken = default)
	{
		invocationContext = invocationContext.InvocationCreateNew();

		var result = new ResultBuilder<Legion.ADF.Messaging.Inbox.Model.VwInboxMessage?>();

		if (result.IsArgumentNull(invocationContext, query))
			return result.Build();

		if (result.IsArgumentNull(invocationContext, connectionProvider))
			return result.Build();

		var uowResult = connectionProvider.UnitOfWorkProvider.CreateQuery<IInboxQueryUnitOfWork>(invocationContext);
		if (result.MergeHasError(uowResult))
			return result.Build();

		await using var uow = uowResult.Data!;
		var data = await uow.VwInboxMessageRepository.GetVwInboxMessageByIdMessage(query with { AsNoTracking = true })
			.ToResultAsync(invocationContext, cancellationToken);

		return result.WithData(data).Build();
	}
}
