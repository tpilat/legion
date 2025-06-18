using Legion.Serializer;

namespace Legion.MessageBus.MessageResolvers;

public interface IMessageType : IDictionaryObject
{
	string Name { get; }
	string CrlType { get; }
	MessageMetaType MessageMetaType { get; }
	IMessageType? ResponseMessageType { get; }
}
