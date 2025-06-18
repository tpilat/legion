using Legion.ADF.Messaging.Outbox.Events;
using Legion.Database;
using Legion.MessageBus.MessageHandlers;

namespace Legion.ADF.Messaging.Outbox.IntegrationTests.Events;
internal record Test1QueueOutboxMessageReceivedEvent : OutboxMessageReceivedEvent
{
	public Test1QueueOutboxMessageReceivedEvent(Model.OutboxMessage outboxMessage)
		: base(outboxMessage)
	{
	}
}

internal class Test1QueueOutboxMessageReceivedEventHandler : AsyncEventHandlerBase<Test1QueueOutboxMessageReceivedEvent>
{
	public override async Task<IResult> HandleAsync(
		IInvocationContext invocationContext,
		Test1QueueOutboxMessageReceivedEvent @event,
		IConnectionProvider connectionProvider,
		CancellationToken cancellationToken = default)
	{
		var result = new ResultBuilder();

		return result.Build();
	}
}

