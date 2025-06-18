using Legion.ADF.Messaging.MessageBox.Events;
using Legion.Database;
using Legion.MessageBus.MessageHandlers;

namespace Legion.ADF.Messaging.MessageBox.IntegrationTests.Events;
internal record Test1TopicSubscriptionMessageReceivedEvent : MessageReceivedEvent
{
	public Test1TopicSubscriptionMessageReceivedEvent(Model.Message message)
		: base(message)
	{
	}
}

internal class Test1TopicSubscriptionMessageReceivedEventHandler : AsyncEventHandlerBase<Test1TopicSubscriptionMessageReceivedEvent>
{
	public override async Task<IResult> HandleAsync(
		IInvocationContext invocationContext,
		Test1TopicSubscriptionMessageReceivedEvent @event,
		IConnectionProvider connectionProvider,
		CancellationToken cancellationToken = default)
	{
		var result = new ResultBuilder();

		return result.Build();
	}
}

