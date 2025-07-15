using Legion;
using Legion.MessageBus.MessageHandlers;
using Legion.Model.Repositories;

namespace Legion.ADF.Messaging.Outbox.QueryHandlers.VwBlockedOutboxMessageType;

public class GetAllVwBlockedOutboxMessageTypesQueryHandler : AsyncMessageHandlerBase<Legion.ADF.Messaging.Outbox.Queries.VwBlockedOutboxMessageType.GetAllVwBlockedOutboxMessageTypesQuery, System.Collections.Generic.List<Legion.ADF.Messaging.Outbox.Model.VwBlockedOutboxMessageType>>
{
	public override async Task<IResult<System.Collections.Generic.List<Legion.ADF.Messaging.Outbox.Model.VwBlockedOutboxMessageType>>> HandleAsync(
		IInvocationContext invocationContext,
		Legion.ADF.Messaging.Outbox.Queries.VwBlockedOutboxMessageType.GetAllVwBlockedOutboxMessageTypesQuery query,
		Legion.Database.IConnectionProvider connectionProvider,
		CancellationToken cancellationToken = default)
	{
		invocationContext = invocationContext.InvocationCreateNew();

		var result = new ResultBuilder<System.Collections.Generic.List<Legion.ADF.Messaging.Outbox.Model.VwBlockedOutboxMessageType>>();

		if (result.IsArgumentNull(invocationContext, query))
			return result.Build();

		if (result.IsArgumentNull(invocationContext, connectionProvider))
			return result.Build();

		var uowResult = connectionProvider.UnitOfWorkProvider.CreateQuery<IOutboxQueryUnitOfWork>(invocationContext);
		if (result.MergeHasError(uowResult))
			return result.Build();

		await using var uow = uowResult.Data!;
		var data = await uow.VwBlockedOutboxMessageTypeRepository.GetAllVwBlockedOutboxMessageTypes(query with { AsNoTracking = true })
			.ToResultAsync(invocationContext, cancellationToken);

		return result.WithData(data).Build();
	}
}
