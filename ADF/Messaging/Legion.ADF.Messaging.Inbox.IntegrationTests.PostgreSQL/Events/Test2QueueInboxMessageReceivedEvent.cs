using Legion.ADF.Messaging.Inbox.Events;
using Legion.Database;
using Legion.MessageBus.MessageHandlers;

namespace Legion.ADF.Messaging.Inbox.IntegrationTests.Events;
internal record Test2QueueInboxMessageReceivedEvent : InboxMessageReceivedEvent
{
	public Test2QueueInboxMessageReceivedEvent(Model.InboxMessage inboxMessage)
		: base(inboxMessage)
	{
	}
}

internal class Test2QueueInboxMessageReceivedEventHandler : AsyncEventHandlerBase<Test2QueueInboxMessageReceivedEvent>
{
	public override async Task<IResult> HandleAsync(
		IInvocationContext invocationContext,
		Test2QueueInboxMessageReceivedEvent @event,
		IConnectionProvider connectionProvider,
		CancellationToken cancellationToken = default)
	{
		var result = new ResultBuilder();

		return result.WithInvalidOperationException(invocationContext, null, "zle je");
	}
}