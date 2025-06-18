using Legion.ADF.Messaging.MessageBox.Services.Internal;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Legion.ADF.Messaging.MessageBox;

public class ADFMessagingMessageBoxBuilder
{
	public ADFMessagingBuilder ADFMessagingBuilder { get; }
	internal MessageTypeRegistry MessageTypeRegistry { get; }
	internal QueueRegistry QueueRegistry { get; }
	internal TopicRegistry TopicRegistry { get; }

	public ADFMessagingMessageBoxBuilder(ADFMessagingBuilder adfMessagingBuilder)
	{
		Throw.IfArgumentNull(adfMessagingBuilder);

		ADFMessagingBuilder = adfMessagingBuilder;

		MessageTypeRegistry = new MessageTypeRegistry();
		QueueRegistry = new QueueRegistry();
		TopicRegistry = new TopicRegistry();

		ADFMessagingBuilder.Services.TryAddSingleton(MessageTypeRegistry);
		ADFMessagingBuilder.Services.TryAddSingleton(QueueRegistry);
		ADFMessagingBuilder.Services.TryAddSingleton(TopicRegistry);
	}
}
