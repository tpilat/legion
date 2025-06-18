using Legion;
using Legion.ADF.ESB.Components;
using Legion.ADF.ESB.ServiceBus.Initializers;
using Legion.DependencyInjection;
using Legion.Extensions;
using Legion.Reflection.ObjectPaths;
using Legion.Validation;
using Microsoft.Extensions.DependencyInjection;

namespace TestEnterpriseServiceBus.Adapters.DB;

public class SocPoistDBAdapterConfig : ESBAdapterConfig, IESBAdapterConfig, IServiceCollectionOptionsBuilder
{
	private const string BASE_CONFIG_PATH = "TestEnterpriseServiceBusConfig";

	public string StoreId { get; set; }
	public int? TimeoutInSeconds { get; set; }

	public class Validator : ValidatorBase<SocPoistDBAdapterConfig>
	{
		public Validator() { }
		public Validator(IObjectPath objectPath) : base(objectPath) { }

		public override void SetDefaultRuels(ValidatorBuilder<SocPoistDBAdapterConfig> builder)
			=> RulesBuilder(builder);

		public static void RulesBuilder(ValidatorBuilder<SocPoistDBAdapterConfig> builder)
		{
			builder?
				.ForProperty(x => x.StoreId, v => v.NotDefaultOrWhiteSpace())
				.ForProperty(x => x.TimeoutInSeconds, v => v.GreaterThan(0))
			;
		}
	}

	public override SocPoistDBAdapterConfig GetDefaultOptions()
	{
		var result = new SocPoistDBAdapterConfig();
		result.SetDefaultOptions();
		return result;
	}

	public static IServiceCollection ConfigureOptions(IServiceCollection services)
	{
		Throw.IfArgumentNull(services);

		services
			.AddAndConfigureOptions<SocPoistDBAdapterConfig>(
				builder: null, //b => b.BindConfiguration($"{BASE_CONFIG_PATH}:{nameof(SocPoistDBAdapterConfig)}"),
				(sp, o) =>
				{
					if (ESBInitializer.ConfigsInitializationStatus == ESBInitializationStatus.Finished)
					{
						//TODO READ FROM DB
					}
					else
					{
						o.SetDefaultOptions();
					}
				},
				true,
				BASE_CONFIG_PATH);

		return services;
	}

	public override void SetDefaultOptions()
	{
		MinLogLevel = Microsoft.Extensions.Logging.LogLevel.Warning;
	}

	public override IResult Merge(IScopeContext scopeContext, string? savedProperties)
	{
		var result = new ResultBuilder();

		if (string.IsNullOrWhiteSpace(savedProperties))
			return result.Build();

		//TODO merge

		return result.Build();
	}
}
