using Legion.MessageBus.MessageHandlers;
using Legion.Model.Repositories;

namespace Legion.ADF.Messaging.DomainEvents.QueryHandlers.DomainEvent;

public class GetNextDomainEventsQueryHandler : AsyncMessageHandlerBase<Legion.ADF.Messaging.DomainEvents.Queries.DomainEvent.GetNextDomainEventsQuery, System.Collections.Generic.List<Legion.ADF.Messaging.DomainEvents.Model.DomainEvent>>
{
	public override async Task<IResult<System.Collections.Generic.List<Legion.ADF.Messaging.DomainEvents.Model.DomainEvent>>> HandleAsync(
		IInvocationContext invocationContext,
		Legion.ADF.Messaging.DomainEvents.Queries.DomainEvent.GetNextDomainEventsQuery query,
		Legion.Database.IConnectionProvider connectionProvider,
		CancellationToken cancellationToken = default)
	{
		invocationContext = invocationContext.InvocationCreateNew();

		var result = new ResultBuilder<System.Collections.Generic.List<Legion.ADF.Messaging.DomainEvents.Model.DomainEvent>>();

		if (result.IsArgumentNull(invocationContext, query))
			return result.Build();

		if (result.IsArgumentNull(invocationContext, connectionProvider))
			return result.Build();

		var uowResult = connectionProvider.UnitOfWorkProvider.Create<IDomainEventsUnitOfWork>(invocationContext);
		if (result.MergeHasError(uowResult))
			return result.Build();

		await using var uow = uowResult.Data!;
		var data = await uow.DomainEventRepository.GetNextDomainEvents(query with { AsNoTracking = true })
			.ToResultAsync(invocationContext, cancellationToken);

		return result.WithData(data).Build();
	}
}
