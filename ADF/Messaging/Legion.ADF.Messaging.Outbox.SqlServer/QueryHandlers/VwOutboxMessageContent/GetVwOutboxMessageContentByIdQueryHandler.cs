using Legion.MessageBus.MessageHandlers;
using Legion.Model.Repositories;

namespace Legion.ADF.Messaging.Outbox.QueryHandlers.VwOutboxMessageContent;

public class GetVwOutboxMessageContentByIdQueryHandler : AsyncMessageHandlerBase<Legion.ADF.Messaging.Outbox.Queries.VwOutboxMessageContent.GetVwOutboxMessageContentByIdQuery, Legion.ADF.Messaging.Outbox.Model.VwOutboxMessageContent?>
{
	public override async Task<IResult<Legion.ADF.Messaging.Outbox.Model.VwOutboxMessageContent?>> HandleAsync(
		IInvocationContext invocationContext,
		Legion.ADF.Messaging.Outbox.Queries.VwOutboxMessageContent.GetVwOutboxMessageContentByIdQuery query,
		Legion.Database.IConnectionProvider connectionProvider,
		CancellationToken cancellationToken = default)
	{
		invocationContext = invocationContext.InvocationCreateNew();

		var result = new ResultBuilder<Legion.ADF.Messaging.Outbox.Model.VwOutboxMessageContent?>();

		if (result.IsArgumentNull(invocationContext, query))
			return result.Build();

		if (result.IsArgumentNull(invocationContext, connectionProvider))
			return result.Build();

		var uowResult = connectionProvider.UnitOfWorkProvider.CreateQuery<IOutboxQueryUnitOfWork>(invocationContext);
		if (result.MergeHasError(uowResult))
			return result.Build();

		await using var uow = uowResult.Data!;
		var data = await uow.VwOutboxMessageContentRepository.GetVwOutboxMessageContentById(query with { AsNoTracking = true })
			.ToResultAsync(invocationContext, cancellationToken);

		return result.WithData(data).Build();
	}
}
