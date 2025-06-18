using Legion.Extensions;
using Legion.Reflection.ObjectPaths;
using Legion.Validation;
using Microsoft.Extensions.DependencyInjection;

namespace Legion.ADF.Config.Settings;

public class ConfigStoreOptions
{
	public string ConfigStoreId { get; set; }
	public bool EnableAuditing { get; set; }

	public class Validator : ValidatorBase<ConfigStoreOptions>
	{
		public Validator() { }
		public Validator(IObjectPath objectPath) : base(objectPath) { }

		public override void SetDefaultRuels(ValidatorBuilder<ConfigStoreOptions> builder)
			=> RulesBuilder(builder);

		public static void RulesBuilder(ValidatorBuilder<ConfigStoreOptions> builder)
		{
			builder?
				.ForProperty(x => x.ConfigStoreId, v => v.MinLength(0));
		}
	}
}

public static class ConfigStoreOptionsExtensions
{
	public static IServiceCollection AddConfigStoreOptions(this IServiceCollection services)
	{
		services
			.AddOptions<ConfigStoreOptions>()
			.BindConfiguration($"{AppSettings.PREFIX}:{nameof(ConfigStoreOptions)}")
			.AddOptionsValidator($"{AppSettings.PREFIX}.{nameof(ConfigStoreOptions)}")
			.ValidateOnStart();

		return services;
	}
}