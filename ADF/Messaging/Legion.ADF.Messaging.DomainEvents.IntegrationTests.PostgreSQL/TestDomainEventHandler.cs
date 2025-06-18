using Legion.Database;
using Legion.MessageBus.MessageHandlers;

namespace Legion.ADF.Messaging.DomainEvents.IntegrationTests.PostgreSQL;

public class TestDomainEventHandler : AsyncEventHandlerBase<TestDomainEvent>
{
	public override async Task<IResult> HandleAsync(
		IInvocationContext invocationContext,
		TestDomainEvent @event,
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

		return result.Build();
	}
}
