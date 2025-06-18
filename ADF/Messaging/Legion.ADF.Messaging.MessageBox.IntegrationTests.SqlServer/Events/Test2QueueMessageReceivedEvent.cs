using Legion.ADF.Messaging.MessageBox.Events;
using Legion.Database;
using Legion.MessageBus.MessageHandlers;

namespace Legion.ADF.Messaging.MessageBox.IntegrationTests.Events;
internal record Test2QueueMessageReceivedEvent : MessageReceivedEvent
{
	public Test2QueueMessageReceivedEvent(Model.Message message)
		: base(message)
	{
	}
}

internal class Test2QueueMessageReceivedEventHandler : AsyncEventHandlerBase<Test2QueueMessageReceivedEvent>
{
	public override async Task<IResult> HandleAsync(
		IInvocationContext invocationContext,
		Test2QueueMessageReceivedEvent @event,
		IConnectionProvider connectionProvider,
		CancellationToken cancellationToken = default)
	{
		var result = new ResultBuilder();

		return result.WithInvalidOperationException(invocationContext, null, "zle je");
	}
}