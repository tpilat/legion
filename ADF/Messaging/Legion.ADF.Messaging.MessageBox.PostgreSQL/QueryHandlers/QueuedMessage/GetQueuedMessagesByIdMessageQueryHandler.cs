using Legion.MessageBus.MessageHandlers;
using Legion.Model.Repositories;

namespace Legion.ADF.Messaging.MessageBox.QueryHandlers.QueuedMessage;

public class GetQueuedMessagesByIdMessageQueryHandler : AsyncMessageHandlerBase<Legion.ADF.Messaging.MessageBox.Queries.QueuedMessage.GetQueuedMessagesByIdMessageQuery, System.Collections.Generic.List<Legion.ADF.Messaging.MessageBox.Model.QueuedMessage>>
{
	public override async Task<IResult<System.Collections.Generic.List<Legion.ADF.Messaging.MessageBox.Model.QueuedMessage>>> HandleAsync(
		IInvocationContext invocationContext,
		Legion.ADF.Messaging.MessageBox.Queries.QueuedMessage.GetQueuedMessagesByIdMessageQuery query,
		Legion.Database.IConnectionProvider connectionProvider,
		CancellationToken cancellationToken = default)
	{
		invocationContext = invocationContext.InvocationCreateNew();

		var result = new ResultBuilder<System.Collections.Generic.List<Legion.ADF.Messaging.MessageBox.Model.QueuedMessage>>();

		if (result.IsArgumentNull(invocationContext, query))
			return result.Build();

		if (result.IsArgumentNull(invocationContext, connectionProvider))
			return result.Build();

		var uowResult = connectionProvider.UnitOfWorkProvider.Create<IMessageBoxUnitOfWork>(invocationContext);
		if (result.MergeHasError(uowResult))
			return result.Build();

		await using var uow = uowResult.Data!;
		var data = await uow.QueuedMessageRepository.GetQueuedMessagesByIdMessage(query with { AsNoTracking = true })
			.ToResultAsync(invocationContext, cancellationToken);

		return result.WithData(data).Build();
	}
}
