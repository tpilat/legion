using Legion.MessageBus.MessageHandlers;
using Legion.Model.Repositories;

namespace Legion.ADF.Messaging.Outbox.QueryHandlers.VwOutboxMessageArchive;

public class GetVwOutboxMessageArchiveByIdMessageQueryHandler : AsyncMessageHandlerBase<Legion.ADF.Messaging.Outbox.Queries.VwOutboxMessageArchive.GetVwOutboxMessageArchiveByIdMessageQuery, Legion.ADF.Messaging.Outbox.Model.VwOutboxMessageArchive?>
{
	public override async Task<IResult<Legion.ADF.Messaging.Outbox.Model.VwOutboxMessageArchive?>> HandleAsync(
		IInvocationContext invocationContext,
		Legion.ADF.Messaging.Outbox.Queries.VwOutboxMessageArchive.GetVwOutboxMessageArchiveByIdMessageQuery query,
		Legion.Database.IConnectionProvider connectionProvider,
		CancellationToken cancellationToken = default)
	{
		invocationContext = invocationContext.InvocationCreateNew();

		var result = new ResultBuilder<Legion.ADF.Messaging.Outbox.Model.VwOutboxMessageArchive?>();

		if (result.IsArgumentNull(invocationContext, query))
			return result.Build();

		if (result.IsArgumentNull(invocationContext, connectionProvider))
			return result.Build();

		var uowResult = connectionProvider.UnitOfWorkProvider.CreateQuery<IOutboxQueryUnitOfWork>(invocationContext);
		if (result.MergeHasError(uowResult))
			return result.Build();

		await using var uow = uowResult.Data!;
		var data = await uow.VwOutboxMessageArchiveRepository.GetVwOutboxMessageArchiveByIdMessage(query with { AsNoTracking = true })
			.ToResultAsync(invocationContext, cancellationToken);

		return result.WithData(data).Build();
	}
}
