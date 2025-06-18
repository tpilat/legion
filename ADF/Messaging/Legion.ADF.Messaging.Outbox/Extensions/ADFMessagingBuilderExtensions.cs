using Legion.ADF.Messaging.Outbox;
using Legion.ADF.Messaging.Outbox.Services;
using Legion.ADF.Messaging.Outbox.Services.Internal;
using Legion.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Reflection;

namespace Legion.ADF.Messaging;

public static class ADFMessagingBuilderExtensions
{
	public static ADFMessagingBuilder AddOutbox(
		this ADFMessagingBuilder adfMessagingBuilder,
		Action<ADFMessagingOutboxBuilder> configure)
	{
		Throw.IfArgumentNull(adfMessagingBuilder);
		Throw.IfArgumentNull(configure);

		if (!adfMessagingBuilder.ADFMessagingBuilderContext.AddOutbox())
			Throw.InvalidOperationException($"{nameof(Outbox)} already configured");

		Assembly[] assemblies = [
			typeof(OutboxStore).Assembly
		];

		//Add all validators from Legion.ADF.Messaging.Outbox.dll
		adfMessagingBuilder.Services.AddValidators(ServiceLifetime.Singleton, assemblies);

		//add all TOption builders
		adfMessagingBuilder.Services.ConfigureOptionsBuilders(assemblies);

		if (adfMessagingBuilder.Configuration != null)
		{
			//add all service builders
			adfMessagingBuilder.Services.ConfigureServiceCollectionBuilders(adfMessagingBuilder.Configuration, assemblies);
		}

		adfMessagingBuilder.Services.TryAddTransient<IOutboxStore, OutboxStore>();

		adfMessagingBuilder.Services.AddStartupTask<OutboxStartup>();

		var adfMessagingOutboxBuilder = new ADFMessagingOutboxBuilder(adfMessagingBuilder);
		configure.Invoke(adfMessagingOutboxBuilder);

		return adfMessagingBuilder;
	}
}
