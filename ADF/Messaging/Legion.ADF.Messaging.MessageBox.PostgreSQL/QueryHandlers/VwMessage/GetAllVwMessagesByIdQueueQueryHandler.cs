using Legion.MessageBus.MessageHandlers;
using Legion.Model.Repositories;

namespace Legion.ADF.Messaging.MessageBox.QueryHandlers.VwMessage;

public class GetAllVwMessagesByIdQueueQueryHandler : AsyncMessageHandlerBase<Legion.ADF.Messaging.MessageBox.Queries.VwMessage.GetAllVwMessagesByIdQueueQuery, System.Collections.Generic.List<Legion.ADF.Messaging.MessageBox.Model.VwMessage>>
{
	public override async Task<IResult<System.Collections.Generic.List<Legion.ADF.Messaging.MessageBox.Model.VwMessage>>> HandleAsync(
		IInvocationContext invocationContext,
		Legion.ADF.Messaging.MessageBox.Queries.VwMessage.GetAllVwMessagesByIdQueueQuery query,
		Legion.Database.IConnectionProvider connectionProvider,
		CancellationToken cancellationToken = default)
	{
		invocationContext = invocationContext.InvocationCreateNew();

		var result = new ResultBuilder<System.Collections.Generic.List<Legion.ADF.Messaging.MessageBox.Model.VwMessage>>();

		if (result.IsArgumentNull(invocationContext, query))
			return result.Build();

		if (result.IsArgumentNull(invocationContext, connectionProvider))
			return result.Build();

		var uowResult = connectionProvider.UnitOfWorkProvider.CreateQuery<IMessageBoxQueryUnitOfWork>(invocationContext);
		if (result.MergeHasError(uowResult))
			return result.Build();

		await using var uow = uowResult.Data!;
		var data = await uow.VwMessageRepository.GetAllVwMessagesByIdQueue(query with { AsNoTracking = true })
			.ToResultAsync(invocationContext, cancellationToken);

		return result.WithData(data).Build();
	}
}
