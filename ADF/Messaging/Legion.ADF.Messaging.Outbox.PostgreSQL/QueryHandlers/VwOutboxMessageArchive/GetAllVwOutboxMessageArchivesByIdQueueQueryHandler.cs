using Legion.MessageBus.MessageHandlers;
using Legion.Model.Repositories;

namespace Legion.ADF.Messaging.Outbox.QueryHandlers.VwOutboxMessageArchive;

public class GetAllVwOutboxMessageArchivesByIdQueueQueryHandler : AsyncMessageHandlerBase<Legion.ADF.Messaging.Outbox.Queries.VwOutboxMessageArchive.GetAllVwOutboxMessageArchivesByIdQueueQuery, System.Collections.Generic.List<Legion.ADF.Messaging.Outbox.Model.VwOutboxMessageArchive>>
{
	public override async Task<IResult<System.Collections.Generic.List<Legion.ADF.Messaging.Outbox.Model.VwOutboxMessageArchive>>> HandleAsync(
		IInvocationContext invocationContext,
		Legion.ADF.Messaging.Outbox.Queries.VwOutboxMessageArchive.GetAllVwOutboxMessageArchivesByIdQueueQuery query,
		Legion.Database.IConnectionProvider connectionProvider,
		CancellationToken cancellationToken = default)
	{
		invocationContext = invocationContext.InvocationCreateNew();

		var result = new ResultBuilder<System.Collections.Generic.List<Legion.ADF.Messaging.Outbox.Model.VwOutboxMessageArchive>>();

		if (result.IsArgumentNull(invocationContext, query))
			return result.Build();

		if (result.IsArgumentNull(invocationContext, connectionProvider))
			return result.Build();

		var uowResult = connectionProvider.UnitOfWorkProvider.CreateQuery<IOutboxQueryUnitOfWork>(invocationContext);
		if (result.MergeHasError(uowResult))
			return result.Build();

		await using var uow = uowResult.Data!;
		var data = await uow.VwOutboxMessageArchiveRepository.GetAllVwOutboxMessageArchivesByIdQueue(query with { AsNoTracking = true })
			.ToResultAsync(invocationContext, cancellationToken);

		return result.WithData(data).Build();
	}
}
