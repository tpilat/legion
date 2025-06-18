using Legion.ADF.Messaging.Inbox.Services.Internal;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Legion.ADF.Messaging.Inbox;

public class ADFMessagingInboxBuilder
{
	public ADFMessagingBuilder ADFMessagingBuilder { get; }
	internal InboxMessageTypeRegistry InboxMessageTypeRegistry { get; }
	internal InboxQueueRegistry InboxQueueRegistry { get; }

	public ADFMessagingInboxBuilder(ADFMessagingBuilder adfMessagingBuilder)
	{
		Throw.IfArgumentNull(adfMessagingBuilder);

		ADFMessagingBuilder = adfMessagingBuilder;

		InboxMessageTypeRegistry = new InboxMessageTypeRegistry();
		InboxQueueRegistry = new InboxQueueRegistry();

		ADFMessagingBuilder.Services.TryAddSingleton(InboxMessageTypeRegistry);
		ADFMessagingBuilder.Services.TryAddSingleton(InboxQueueRegistry);
	}
}
