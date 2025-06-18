using Legion.ADF.Messaging.DomainEvents;
using Legion.ADF.Messaging.DomainEvents.Services.Internal;
using Microsoft.Extensions.DependencyInjection;

namespace Legion.ADF.Messaging;

public static class ADFMessagingDomainEventsBuilderExtensions
{
	public static ADFMessagingDomainEventsBuilder AllowProcessingService(
		this ADFMessagingDomainEventsBuilder builder,
		bool allowed)
	{
		Throw.IfArgumentNull(builder);

		if (builder.ProcessingServiceAllowed)
			Throw.InvalidOperationException($"{nameof(builder.ProcessingServiceAllowed)} already configured");

		builder.ProcessingServiceAllowed = allowed;

		if (builder.ProcessingServiceAllowed)
			builder.ADFMessagingBuilder.Services.AddHostedService<DomainEventProcessingService>();

		return builder;
	}
}
