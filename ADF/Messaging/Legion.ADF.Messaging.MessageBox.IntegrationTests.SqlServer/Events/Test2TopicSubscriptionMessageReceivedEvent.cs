using Legion.ADF.Messaging.MessageBox.Events;
using Legion.Database;
using Legion.MessageBus.MessageHandlers;

namespace Legion.ADF.Messaging.MessageBox.IntegrationTests.Events;
internal record Test2TopicSubscriptionMessageReceivedEvent : MessageReceivedEvent
{
	public Test2TopicSubscriptionMessageReceivedEvent(Model.Message message)
		: base(message)
	{
	}
}

internal class Test2TopicSubscriptionMessageReceivedEventHandler : AsyncEventHandlerBase<Test2TopicSubscriptionMessageReceivedEvent>
{
	public override async Task<IResult> HandleAsync(
		IInvocationContext invocationContext,
		Test2TopicSubscriptionMessageReceivedEvent @event,
		IConnectionProvider connectionProvider,
		CancellationToken cancellationToken = default)
	{
		var result = new ResultBuilder();

		return result.WithInvalidOperationException(invocationContext, null, "zle je");
	}
}
