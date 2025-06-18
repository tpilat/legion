using Legion.MessageBus.MessageHandlers;
using Legion.Model.Repositories;

namespace Legion.ADF.Messaging.DomainEvents.QueryHandlers.DomainEventContent;

public class GetDomainEventContentByIdQueryHandler : AsyncMessageHandlerBase<Legion.ADF.Messaging.DomainEvents.Queries.DomainEventContent.GetDomainEventContentByIdQuery, Legion.ADF.Messaging.DomainEvents.Model.DomainEventContent?>
{
	public override async Task<IResult<Legion.ADF.Messaging.DomainEvents.Model.DomainEventContent?>> HandleAsync(
		IInvocationContext invocationContext,
		Legion.ADF.Messaging.DomainEvents.Queries.DomainEventContent.GetDomainEventContentByIdQuery query,
		Legion.Database.IConnectionProvider connectionProvider,
		CancellationToken cancellationToken = default)
	{
		invocationContext = invocationContext.InvocationCreateNew();

		var result = new ResultBuilder<Legion.ADF.Messaging.DomainEvents.Model.DomainEventContent?>();

		if (result.IsArgumentNull(invocationContext, query))
			return result.Build();

		if (result.IsArgumentNull(invocationContext, connectionProvider))
			return result.Build();

		var uowResult = connectionProvider.UnitOfWorkProvider.Create<IDomainEventsUnitOfWork>(invocationContext);
		if (result.MergeHasError(uowResult))
			return result.Build();

		await using var uow = uowResult.Data!;
		var data = await uow.DomainEventContentRepository.GetDomainEventContentById(query with { AsNoTracking = true })
			.ToResultAsync(invocationContext, cancellationToken);

		return result.WithData(data).Build();
	}
}
