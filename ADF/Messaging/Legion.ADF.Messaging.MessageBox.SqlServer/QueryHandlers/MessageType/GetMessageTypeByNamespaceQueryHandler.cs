using Legion.MessageBus.MessageHandlers;
using Legion.Model.Repositories;

namespace Legion.ADF.Messaging.MessageBox.QueryHandlers.MessageType;

public class GetMessageTypeByNamespaceQueryHandler : AsyncMessageHandlerBase<Legion.ADF.Messaging.MessageBox.Queries.MessageType.GetMessageTypeByNamespaceQuery, Legion.ADF.Messaging.MessageBox.Model.MessageType?>
{
	public override async Task<IResult<Legion.ADF.Messaging.MessageBox.Model.MessageType?>> HandleAsync(
		IInvocationContext invocationContext,
		Legion.ADF.Messaging.MessageBox.Queries.MessageType.GetMessageTypeByNamespaceQuery query,
		Legion.Database.IConnectionProvider connectionProvider,
		CancellationToken cancellationToken = default)
	{
		invocationContext = invocationContext.InvocationCreateNew();

		var result = new ResultBuilder<Legion.ADF.Messaging.MessageBox.Model.MessageType?>();

		if (result.IsArgumentNull(invocationContext, query))
			return result.Build();

		if (result.IsArgumentNull(invocationContext, connectionProvider))
			return result.Build();

		var uowResult = connectionProvider.UnitOfWorkProvider.Create<IMessageBoxUnitOfWork>(invocationContext);
		if (result.MergeHasError(uowResult))
			return result.Build();

		await using var uow = uowResult.Data!;
		var data = await uow.MessageTypeRepository.GetMessageTypeByNamespace(query with { AsNoTracking = true })
			.ToResultAsync(invocationContext, cancellationToken);

		return result.WithData(data).Build();
	}
}
