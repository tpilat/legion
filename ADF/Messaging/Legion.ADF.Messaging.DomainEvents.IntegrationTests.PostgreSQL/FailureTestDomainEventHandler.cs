using Legion.Database;
using Legion.MessageBus.MessageHandlers;

namespace Legion.ADF.Messaging.DomainEvents.IntegrationTests.PostgreSQL;

public class FailureTestDomainEventHandler : AsyncEventHandlerBase<FailureTestDomainEvent>
{
	public override async Task<IResult> HandleAsync(
		IInvocationContext invocationContext,
		FailureTestDomainEvent @event,
		IConnectionProvider connectionProvider,
		CancellationToken cancellationToken = default)
	{
		invocationContext = invocationContext.InvocationCreateNew();

		var result = new ResultBuilder();

		if (result.IsArgumentNull(invocationContext, @event))
			return result.Build();

		if (result.IsArgumentNull(invocationContext, connectionProvider))
			return result.Build();

		await Task.CompletedTask;

		return result.WithInvalidOperationException(invocationContext, null, x => x.InternalMessage("Handler custom failure"));
	}
}
