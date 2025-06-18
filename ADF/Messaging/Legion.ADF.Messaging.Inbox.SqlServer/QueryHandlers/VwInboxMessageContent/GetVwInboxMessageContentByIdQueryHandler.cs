using Legion.MessageBus.MessageHandlers;
using Legion.Model.Repositories;

namespace Legion.ADF.Messaging.Inbox.QueryHandlers.VwInboxMessageContent;

public class GetVwInboxMessageContentByIdQueryHandler : AsyncMessageHandlerBase<Legion.ADF.Messaging.Inbox.Queries.VwInboxMessageContent.GetVwInboxMessageContentByIdQuery, Legion.ADF.Messaging.Inbox.Model.VwInboxMessageContent?>
{
	public override async Task<IResult<Legion.ADF.Messaging.Inbox.Model.VwInboxMessageContent?>> HandleAsync(
		IInvocationContext invocationContext,
		Legion.ADF.Messaging.Inbox.Queries.VwInboxMessageContent.GetVwInboxMessageContentByIdQuery query,
		Legion.Database.IConnectionProvider connectionProvider,
		CancellationToken cancellationToken = default)
	{
		invocationContext = invocationContext.InvocationCreateNew();

		var result = new ResultBuilder<Legion.ADF.Messaging.Inbox.Model.VwInboxMessageContent?>();

		if (result.IsArgumentNull(invocationContext, query))
			return result.Build();

		if (result.IsArgumentNull(invocationContext, connectionProvider))
			return result.Build();

		var uowResult = connectionProvider.UnitOfWorkProvider.CreateQuery<IInboxQueryUnitOfWork>(invocationContext);
		if (result.MergeHasError(uowResult))
			return result.Build();

		await using var uow = uowResult.Data!;
		var data = await uow.VwInboxMessageContentRepository.GetVwInboxMessageContentById(query with { AsNoTracking = true })
			.ToResultAsync(invocationContext, cancellationToken);

		return result.WithData(data).Build();
	}
}
