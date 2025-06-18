using Legion.MessageBus.MessageHandlers;
using Legion.Model.Repositories;

namespace Legion.ADF.Messaging.Outbox.QueryHandlers.OutboxInstance;

public class ExistsOutboxInstanceByIdQueryHandler : AsyncMessageHandlerBase<Legion.ADF.Messaging.Outbox.Queries.OutboxInstance.ExistsOutboxInstanceByIdQuery, bool>
{
	public override async Task<IResult<bool>> HandleAsync(
		IInvocationContext invocationContext,
		Legion.ADF.Messaging.Outbox.Queries.OutboxInstance.ExistsOutboxInstanceByIdQuery query,
		Legion.Database.IConnectionProvider connectionProvider,
		CancellationToken cancellationToken = default)
	{
		invocationContext = invocationContext.InvocationCreateNew();

		var result = new ResultBuilder<bool>();

		if (result.IsArgumentNull(invocationContext, query))
			return result.Build();

		if (result.IsArgumentNull(invocationContext, connectionProvider))
			return result.Build();

		var uowResult = connectionProvider.UnitOfWorkProvider.Create<IOutboxUnitOfWork>(invocationContext);
		if (result.MergeHasError(uowResult))
			return result.Build();

		await using var uow = uowResult.Data!;
		var data = await uow.OutboxInstanceRepository.ExistsOutboxInstanceById(query with { AsNoTracking = true })
			.ToResultAsync(invocationContext, cancellationToken);

		return result.WithData(data).Build();
	}
}
