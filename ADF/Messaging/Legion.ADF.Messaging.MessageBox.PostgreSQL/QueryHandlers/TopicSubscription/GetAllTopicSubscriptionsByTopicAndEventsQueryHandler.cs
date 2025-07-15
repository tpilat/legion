using Legion;
using Legion.MessageBus.MessageHandlers;
using Legion.Model.Repositories;

namespace Legion.ADF.Messaging.MessageBox.QueryHandlers.TopicSubscription;

public class GetAllTopicSubscriptionsByTopicAndEventsQueryHandler : AsyncMessageHandlerBase<Legion.ADF.Messaging.MessageBox.Queries.TopicSubscription.GetAllTopicSubscriptionsByTopicAndEventsQuery, System.Collections.Generic.List<Legion.ADF.Messaging.MessageBox.Model.TopicSubscription>>
{
	public override async Task<IResult<System.Collections.Generic.List<Legion.ADF.Messaging.MessageBox.Model.TopicSubscription>>> HandleAsync(
		IInvocationContext invocationContext,
		Legion.ADF.Messaging.MessageBox.Queries.TopicSubscription.GetAllTopicSubscriptionsByTopicAndEventsQuery query,
		Legion.Database.IConnectionProvider connectionProvider,
		CancellationToken cancellationToken = default)
	{
		invocationContext = invocationContext.InvocationCreateNew();

		var result = new ResultBuilder<System.Collections.Generic.List<Legion.ADF.Messaging.MessageBox.Model.TopicSubscription>>();

		if (result.IsArgumentNull(invocationContext, query))
			return result.Build();

		if (result.IsArgumentNull(invocationContext, connectionProvider))
			return result.Build();

		var uowResult = connectionProvider.UnitOfWorkProvider.Create<IMessageBoxUnitOfWork>(invocationContext);
		if (result.MergeHasError(uowResult))
			return result.Build();

		await using var uow = uowResult.Data!;
		var data = await uow.TopicSubscriptionRepository.GetAllTopicSubscriptionsByTopicAndEvents(query with { AsNoTracking = true })
			.ToResultAsync(invocationContext, cancellationToken);

		return result.WithData(data).Build();
	}
}
