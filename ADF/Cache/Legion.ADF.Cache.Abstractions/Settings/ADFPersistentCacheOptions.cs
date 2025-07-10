using Legion.Clones;
using Legion.Extensions;
using Legion.Reflection.ObjectPaths;
using Legion.Validation;
using Microsoft.Extensions.DependencyInjection;

namespace Legion.ADF.Cache.Settings;

public class ADFPersistentCacheOptions
{
	public string CacheStoreId { get; set; }

	public class Validator : ValidatorBase<ADFPersistentCacheOptions>
	{
		public Validator() { }
		public Validator(IObjectPath objectPath) : base(objectPath) { }

		public override void SetDefaultRuels(ValidatorBuilder<ADFPersistentCacheOptions> builder)
			=> RulesBuilder(builder);

		public static void RulesBuilder(ValidatorBuilder<ADFPersistentCacheOptions> builder)
		{
			builder?
				.ForProperty(x => x.CacheStoreId, v => v.MinLength(0));
		}
	}
}

public static class ADFPersistentCacheOptionsExtensions
{
	public static IServiceCollection AddADFPersistentCacheOptions(this IServiceCollection services)
	{
		services
			.AddOptions<ADFPersistentCacheOptions>()
			.BindConfiguration($"{AppSettings.PREFIX}:{nameof(ADFPersistentCacheOptions)}")
			.AddOptionsValidator($"{AppSettings.PREFIX}.{nameof(ADFPersistentCacheOptions)}")
			.ValidateOnStart();

		return services;
	}
}
