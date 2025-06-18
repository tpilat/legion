using Legion.MessageBus.MessageHandlers;
using Legion.Model.Repositories;

namespace Legion.ADF.Messaging.MessageBox.QueryHandlers.VwTopic;

public class GetVwTopicByIdQueryHandler : AsyncMessageHandlerBase<Legion.ADF.Messaging.MessageBox.Queries.VwTopic.GetVwTopicByIdQuery, Legion.ADF.Messaging.MessageBox.Model.VwTopic?>
{
	public override async Task<IResult<Legion.ADF.Messaging.MessageBox.Model.VwTopic?>> HandleAsync(
		IInvocationContext invocationContext,
		Legion.ADF.Messaging.MessageBox.Queries.VwTopic.GetVwTopicByIdQuery query,
		Legion.Database.IConnectionProvider connectionProvider,
		CancellationToken cancellationToken = default)
	{
		invocationContext = invocationContext.InvocationCreateNew();

		var result = new ResultBuilder<Legion.ADF.Messaging.MessageBox.Model.VwTopic?>();

		if (result.IsArgumentNull(invocationContext, query))
			return result.Build();

		if (result.IsArgumentNull(invocationContext, connectionProvider))
			return result.Build();

		var uowResult = connectionProvider.UnitOfWorkProvider.CreateQuery<IMessageBoxQueryUnitOfWork>(invocationContext);
		if (result.MergeHasError(uowResult))
			return result.Build();

		await using var uow = uowResult.Data!;
		var data = await uow.VwTopicRepository.GetVwTopicById(query with { AsNoTracking = true })
			.ToResultAsync(invocationContext, cancellationToken);

		return result.WithData(data).Build();
	}
}
