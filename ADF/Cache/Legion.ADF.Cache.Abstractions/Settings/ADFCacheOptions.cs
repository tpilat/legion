using Legion.Clones;
using Legion.Extensions;
using Legion.Reflection.ObjectPaths;
using Legion.Validation;
using Microsoft.Extensions.DependencyInjection;

namespace Legion.ADF.Cache.Settings;

public class ADFCacheOptions
{
	public long? SizeLimit { get; set; }

	public ICloneFactory CloneFactory { get; set; }

	public class Validator : ValidatorBase<ADFCacheOptions>
	{
		public Validator() { }
		public Validator(IObjectPath objectPath) : base(objectPath) { }

		public override void SetDefaultRuels(ValidatorBuilder<ADFCacheOptions> builder)
			=> RulesBuilder(builder);

		public static void RulesBuilder(ValidatorBuilder<ADFCacheOptions> builder)
		{
			builder?
				.ForProperty(x => x.SizeLimit, v => v.GreaterThan(0));
		}
	}
}

public static class ADFCacheOptionsExtensions
{
	public static IServiceCollection AddADFCacheOptions(this IServiceCollection services)
	{
		services
			.AddOptions<ADFCacheOptions>()
			.BindConfiguration($"{AppSettings.PREFIX}:{nameof(ADFCacheOptions)}")
			.AddOptionsValidator($"{AppSettings.PREFIX}.{nameof(ADFCacheOptions)}")
			.ValidateOnStart();

		return services;
	}
}
