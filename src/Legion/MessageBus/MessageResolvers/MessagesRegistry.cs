using Legion.MessageBus.MessageResolvers.Internal;

namespace Legion.MessageBus.MessageResolvers;

public class MessagesRegistry
{
	public static IEnumerable<IMessageType>? GetAllMessageTypes()
	{
		var result = MessageHandlerRegistry.GetAllMessageTypes();
		result.AddRange(EventHandlerRegistry.GetAllEventTypes());
		return result;
	}

	public static IMessageType? GetMessageType(Type type)
		=> MessageHandlerRegistry.GetMessageType(type)
			?? EventHandlerRegistry.GetEventType(type);

	public static bool CanBeHandled(Type type)
		=> MessageHandlerRegistry.CanBeHandled(type)
			|| EventHandlerRegistry.CanBeHandled(type);
}
