using Legion.ADF.Messaging.Outbox.Services.Internal;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Legion.ADF.Messaging.Outbox;

public class ADFMessagingOutboxBuilder
{
	public ADFMessagingBuilder ADFMessagingBuilder { get; }
	internal OutboxMessageTypeRegistry OutboxMessageTypeRegistry { get; }
	internal OutboxQueueRegistry OutboxQueueRegistry { get; }

	public ADFMessagingOutboxBuilder(ADFMessagingBuilder adfMessagingBuilder)
	{
		Throw.IfArgumentNull(adfMessagingBuilder);

		ADFMessagingBuilder = adfMessagingBuilder;

		OutboxMessageTypeRegistry = new OutboxMessageTypeRegistry();
		OutboxQueueRegistry = new OutboxQueueRegistry();

		ADFMessagingBuilder.Services.TryAddSingleton(OutboxMessageTypeRegistry);
		ADFMessagingBuilder.Services.TryAddSingleton(OutboxQueueRegistry);
	}
}
