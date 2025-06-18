using Legion.ADF.Messaging.Inbox.Events;
using Legion.Database;
using Legion.MessageBus.MessageHandlers;

namespace Legion.ADF.Messaging.Inbox.IntegrationTests.Events;
internal record Test1QueueInboxMessageReceivedEvent : InboxMessageReceivedEvent
{
	public Test1QueueInboxMessageReceivedEvent(Model.InboxMessage inboxMessage)
		: base(inboxMessage)
	{
	}
}

internal class Test1QueueInboxMessageReceivedEventHandler : AsyncEventHandlerBase<Test1QueueInboxMessageReceivedEvent>
{
	public override async Task<IResult> HandleAsync(
		IInvocationContext invocationContext,
		Test1QueueInboxMessageReceivedEvent @event,
		IConnectionProvider connectionProvider,
		CancellationToken cancellationToken = default)
	{
		var result = new ResultBuilder();

		return result.Build();
	}
}

