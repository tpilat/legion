using Legion;
using Legion.MessageBus.MessageHandlers;
using Legion.Model.Repositories;

namespace Legion.ADF.Messaging.DomainEvents.QueryHandlers.BlockedDomainEventType;

public class GetAllBlockedDomainEventTypesQueryHandler : AsyncMessageHandlerBase<Legion.ADF.Messaging.DomainEvents.Queries.BlockedDomainEventType.GetAllBlockedDomainEventTypesQuery, System.Collections.Generic.List<Legion.ADF.Messaging.DomainEvents.Model.BlockedDomainEventType>>
{
	public override async Task<IResult<System.Collections.Generic.List<Legion.ADF.Messaging.DomainEvents.Model.BlockedDomainEventType>>> HandleAsync(
		IInvocationContext invocationContext,
		Legion.ADF.Messaging.DomainEvents.Queries.BlockedDomainEventType.GetAllBlockedDomainEventTypesQuery query,
		Legion.Database.IConnectionProvider connectionProvider,
		CancellationToken cancellationToken = default)
	{
		invocationContext = invocationContext.InvocationCreateNew();

		var result = new ResultBuilder<System.Collections.Generic.List<Legion.ADF.Messaging.DomainEvents.Model.BlockedDomainEventType>>();

		if (result.IsArgumentNull(invocationContext, query))
			return result.Build();

		if (result.IsArgumentNull(invocationContext, connectionProvider))
			return result.Build();

		var uowResult = connectionProvider.UnitOfWorkProvider.Create<IDomainEventsUnitOfWork>(invocationContext);
		if (result.MergeHasError(uowResult))
			return result.Build();

		await using var uow = uowResult.Data!;
		var data = await uow.BlockedDomainEventTypeRepository.GetAllBlockedDomainEventTypes(query with { AsNoTracking = true })
			.ToResultAsync(invocationContext, cancellationToken);

		return result.WithData(data).Build();
	}
}
