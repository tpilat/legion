namespace Legion.MessageBus.MessageResolvers;

public interface IMessageTypeResolver
{
	string ToName(Type type);
	Type ToType(string name);
}
