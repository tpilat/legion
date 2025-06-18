using Legion.ADF.Messaging.Inbox;
using Legion.ADF.Messaging.Inbox.Services;
using Legion.ADF.Messaging.Inbox.Services.Internal;
using Legion.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Reflection;

namespace Legion.ADF.Messaging;

public static class ADFMessagingBuilderExtensions
{
	public static ADFMessagingBuilder AddInbox(
		this ADFMessagingBuilder adfMessagingBuilder,
		Action<ADFMessagingInboxBuilder> configure)
	{
		Throw.IfArgumentNull(adfMessagingBuilder);
		Throw.IfArgumentNull(configure);

		if (!adfMessagingBuilder.ADFMessagingBuilderContext.AddInbox())
			Throw.InvalidOperationException($"{nameof(Inbox)} already configured");

		Assembly[] assemblies = [
			typeof(InboxStore).Assembly
		];

		//Add all validators from Legion.ADF.Messaging.Inbox.dll
		adfMessagingBuilder.Services.AddValidators(ServiceLifetime.Singleton, assemblies);

		//add all TOption builders
		adfMessagingBuilder.Services.ConfigureOptionsBuilders(assemblies);

		if (adfMessagingBuilder.Configuration != null)
		{
			//add all service builders
			adfMessagingBuilder.Services.ConfigureServiceCollectionBuilders(adfMessagingBuilder.Configuration, assemblies);
		}

		adfMessagingBuilder.Services.TryAddTransient<IInboxStore, InboxStore>();

		adfMessagingBuilder.Services.AddStartupTask<InboxStartup>();

		var adfMessagingInboxBuilder = new ADFMessagingInboxBuilder(adfMessagingBuilder);
		configure.Invoke(adfMessagingInboxBuilder);

		return adfMessagingBuilder;
	}
}
