using Legion.ADF.Messaging.MessageBox.Events;
using Legion.Database;
using Legion.MessageBus.MessageHandlers;

namespace Legion.ADF.Messaging.MessageBox.IntegrationTests.Events;
internal record Test1QueueMessageReceivedEvent : MessageReceivedEvent
{
	public Test1QueueMessageReceivedEvent(Model.Message message)
		: base(message)
	{
	}
}

internal class Test1QueueMessageReceivedEventHandler : AsyncEventHandlerBase<Test1QueueMessageReceivedEvent>
{
	public override async Task<IResult> HandleAsync(
		IInvocationContext invocationContext,
		Test1QueueMessageReceivedEvent @event,
		IConnectionProvider connectionProvider,
		CancellationToken cancellationToken = default)
	{
		var result = new ResultBuilder();

		return result.Build();
	}
}

