using Legion.ADF.Messaging.DomainEvents;
using Legion.ADF.Messaging.DomainEvents.Services;
using Legion.ADF.Messaging.DomainEvents.Services.Internal;
using Legion.Extensions;
using Legion.Model.Messaging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Reflection;

namespace Legion.ADF.Messaging;

public static class ADFMessagingBuilderExtensions
{
	public static ADFMessagingBuilder AddDomainEvents(
		this ADFMessagingBuilder adfMessagingBuilder,
		Action<ADFMessagingDomainEventsBuilder> configure)
	{
		Throw.IfArgumentNull(adfMessagingBuilder);
		Throw.IfArgumentNull(configure);

		if (!adfMessagingBuilder.ADFMessagingBuilderContext.AddDomainEvents())
			Throw.InvalidOperationException($"{nameof(DomainEvents)} already configured");

		Assembly[] assemblies = [
			typeof(DomainEventStore).Assembly
		];

		//Add all validators from Legion.ADF.Messaging.DomainEvents.dll
		adfMessagingBuilder.Services.AddValidators(ServiceLifetime.Singleton, assemblies);

		//add all TOption builders
		adfMessagingBuilder.Services.ConfigureOptionsBuilders(assemblies);

		if (adfMessagingBuilder.Configuration != null)
		{
			//add all service builders
			adfMessagingBuilder.Services.ConfigureServiceCollectionBuilders(adfMessagingBuilder.Configuration, assemblies);
		}

		adfMessagingBuilder.Services.TryAddSingleton<IDomainEventStoreFactory, DomainEventStoreFactory>();
		adfMessagingBuilder.Services.TryAddTransient<DomainEventStore>();
		adfMessagingBuilder.Services.TryAddTransient<Legion.Model.Messaging.IDomainEventStore, DomainEventStore>();

		var adfMessagingDomainEventsBuilder = new ADFMessagingDomainEventsBuilder(adfMessagingBuilder);
		configure.Invoke(adfMessagingDomainEventsBuilder);

		return adfMessagingBuilder;
	}
}
