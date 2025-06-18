using Legion.MessageBus.MessageHandlers;
using Legion.Model.Repositories;

namespace Legion.ADF.Messaging.MessageBox.QueryHandlers.VwMessageContent;

public class GetVwMessageContentByIdQueryHandler : AsyncMessageHandlerBase<Legion.ADF.Messaging.MessageBox.Queries.VwMessageContent.GetVwMessageContentByIdQuery, Legion.ADF.Messaging.MessageBox.Model.VwMessageContent?>
{
	public override async Task<IResult<Legion.ADF.Messaging.MessageBox.Model.VwMessageContent?>> HandleAsync(
		IInvocationContext invocationContext,
		Legion.ADF.Messaging.MessageBox.Queries.VwMessageContent.GetVwMessageContentByIdQuery query,
		Legion.Database.IConnectionProvider connectionProvider,
		CancellationToken cancellationToken = default)
	{
		invocationContext = invocationContext.InvocationCreateNew();

		var result = new ResultBuilder<Legion.ADF.Messaging.MessageBox.Model.VwMessageContent?>();

		if (result.IsArgumentNull(invocationContext, query))
			return result.Build();

		if (result.IsArgumentNull(invocationContext, connectionProvider))
			return result.Build();

		var uowResult = connectionProvider.UnitOfWorkProvider.CreateQuery<IMessageBoxQueryUnitOfWork>(invocationContext);
		if (result.MergeHasError(uowResult))
			return result.Build();

		await using var uow = uowResult.Data!;
		var data = await uow.VwMessageContentRepository.GetVwMessageContentById(query with { AsNoTracking = true })
			.ToResultAsync(invocationContext, cancellationToken);

		return result.WithData(data).Build();
	}
}
