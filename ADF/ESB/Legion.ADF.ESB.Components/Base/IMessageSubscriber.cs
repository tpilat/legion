using Legion.MessageBus.MessageHandlers;
using Legion.MessageBus.Messages;

namespace Legion.ADF.ESB.Components;

public interface IMessageSubscriber<TRequest> : IAsyncMessageHandler<TRequest>, IMessageHandler
	where TRequest : IRequestMessage
{
}

public interface IMessageSubscriber<TRequest, TResponse> : IAsyncMessageHandler<TRequest, TResponse>, IMessageHandler
	where TRequest : IRequestMessage<TResponse>
{
}

public interface IEventSubscriber<TEvent> : IAsyncEventHandler<TEvent>, IEventHandler
	where TEvent : IEvent
{
}
