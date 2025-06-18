using Legion.ADF.Messaging.MessageBox;
using Legion.ADF.Messaging.MessageBox.Services;
using Legion.ADF.Messaging.MessageBox.Services.Internal;
using Legion.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Reflection;

namespace Legion.ADF.Messaging;

public static class ADFMessagingBuilderExtensions
{
	public static ADFMessagingBuilder AddMessageBox(
		this ADFMessagingBuilder adfMessagingBuilder,
		Action<ADFMessagingMessageBoxBuilder> configure)
	{
		Throw.IfArgumentNull(adfMessagingBuilder);
		Throw.IfArgumentNull(configure);

		if (!adfMessagingBuilder.ADFMessagingBuilderContext.AddMessageBox())
			Throw.InvalidOperationException($"{nameof(MessageBox)} already configured");

		Assembly[] assemblies = [
			typeof(MessageBoxStore).Assembly
		];

		//Add all validators from Legion.ADF.Messaging.MessageBox.dll
		adfMessagingBuilder.Services.AddValidators(ServiceLifetime.Singleton, assemblies);

		//add all TOption builders
		adfMessagingBuilder.Services.ConfigureOptionsBuilders(assemblies);

		if (adfMessagingBuilder.Configuration != null)
		{
			//add all service builders
			adfMessagingBuilder.Services.ConfigureServiceCollectionBuilders(adfMessagingBuilder.Configuration, assemblies);
		}

		adfMessagingBuilder.Services.TryAddTransient<IMessageBoxStore, MessageBoxStore>();

		adfMessagingBuilder.Services.AddStartupTask<MessageBoxStartup>();

		var adfMessagingMessageBoxBuilder = new ADFMessagingMessageBoxBuilder(adfMessagingBuilder);
		configure.Invoke(adfMessagingMessageBoxBuilder);

		return adfMessagingBuilder;
	}
}
