using Legion.MessageBus.MessageHandlers;
using Legion.Model.Repositories;

namespace Legion.ADF.Messaging.MessageBox.QueryHandlers.SubscribedMessage;

public class GetSubscribedMessageByIdQueryHandler : AsyncMessageHandlerBase<Legion.ADF.Messaging.MessageBox.Queries.SubscribedMessage.GetSubscribedMessageByIdQuery, Legion.ADF.Messaging.MessageBox.Model.SubscribedMessage?>
{
	public override async Task<IResult<Legion.ADF.Messaging.MessageBox.Model.SubscribedMessage?>> HandleAsync(
		IInvocationContext invocationContext,
		Legion.ADF.Messaging.MessageBox.Queries.SubscribedMessage.GetSubscribedMessageByIdQuery query,
		Legion.Database.IConnectionProvider connectionProvider,
		CancellationToken cancellationToken = default)
	{
		invocationContext = invocationContext.InvocationCreateNew();

		var result = new ResultBuilder<Legion.ADF.Messaging.MessageBox.Model.SubscribedMessage?>();

		if (result.IsArgumentNull(invocationContext, query))
			return result.Build();

		if (result.IsArgumentNull(invocationContext, connectionProvider))
			return result.Build();

		var uowResult = connectionProvider.UnitOfWorkProvider.Create<IMessageBoxUnitOfWork>(invocationContext);
		if (result.MergeHasError(uowResult))
			return result.Build();

		await using var uow = uowResult.Data!;
		var data = await uow.SubscribedMessageRepository.GetSubscribedMessageById(query with { AsNoTracking = true })
			.ToResultAsync(invocationContext, cancellationToken);

		return result.WithData(data).Build();
	}
}
