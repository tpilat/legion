using Legion.ADF.Messaging.Outbox.Events;
using Legion.Database;
using Legion.MessageBus.MessageHandlers;

namespace Legion.ADF.Messaging.Outbox.IntegrationTests.Events;
internal record Test2QueueOutboxMessageReceivedEvent : OutboxMessageReceivedEvent
{
	public Test2QueueOutboxMessageReceivedEvent(Model.OutboxMessage outboxMessage)
		: base(outboxMessage)
	{
	}
}

internal class Test2QueueOutboxMessageReceivedEventHandler : AsyncEventHandlerBase<Test2QueueOutboxMessageReceivedEvent>
{
	public override async Task<IResult> HandleAsync(
		IInvocationContext invocationContext,
		Test2QueueOutboxMessageReceivedEvent @event,
		IConnectionProvider connectionProvider,
		CancellationToken cancellationToken = default)
	{
		var result = new ResultBuilder();

		return result.WithInvalidOperationException(invocationContext, null, "zle je");
	}
}