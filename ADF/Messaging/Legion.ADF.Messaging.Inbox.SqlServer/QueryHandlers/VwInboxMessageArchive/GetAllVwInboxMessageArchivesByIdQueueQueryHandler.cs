using Legion.MessageBus.MessageHandlers;
using Legion.Model.Repositories;

namespace Legion.ADF.Messaging.Inbox.QueryHandlers.VwInboxMessageArchive;

public class GetAllVwInboxMessageArchivesByIdQueueQueryHandler : AsyncMessageHandlerBase<Legion.ADF.Messaging.Inbox.Queries.VwInboxMessageArchive.GetAllVwInboxMessageArchivesByIdQueueQuery, System.Collections.Generic.List<Legion.ADF.Messaging.Inbox.Model.VwInboxMessageArchive>>
{
	public override async Task<IResult<System.Collections.Generic.List<Legion.ADF.Messaging.Inbox.Model.VwInboxMessageArchive>>> HandleAsync(
		IInvocationContext invocationContext,
		Legion.ADF.Messaging.Inbox.Queries.VwInboxMessageArchive.GetAllVwInboxMessageArchivesByIdQueueQuery query,
		Legion.Database.IConnectionProvider connectionProvider,
		CancellationToken cancellationToken = default)
	{
		invocationContext = invocationContext.InvocationCreateNew();

		var result = new ResultBuilder<System.Collections.Generic.List<Legion.ADF.Messaging.Inbox.Model.VwInboxMessageArchive>>();

		if (result.IsArgumentNull(invocationContext, query))
			return result.Build();

		if (result.IsArgumentNull(invocationContext, connectionProvider))
			return result.Build();

		var uowResult = connectionProvider.UnitOfWorkProvider.CreateQuery<IInboxQueryUnitOfWork>(invocationContext);
		if (result.MergeHasError(uowResult))
			return result.Build();

		await using var uow = uowResult.Data!;
		var data = await uow.VwInboxMessageArchiveRepository.GetAllVwInboxMessageArchivesByIdQueue(query with { AsNoTracking = true })
			.ToResultAsync(invocationContext, cancellationToken);

		return result.WithData(data).Build();
	}
}
