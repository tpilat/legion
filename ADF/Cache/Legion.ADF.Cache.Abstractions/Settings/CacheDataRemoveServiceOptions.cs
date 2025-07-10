using Legion.Extensions;
using Legion.Reflection.ObjectPaths;
using Legion.Validation;
using Microsoft.Extensions.DependencyInjection;

namespace Legion.ADF.Cache.Settings;

public class CacheDataRemoveServiceOptions
{
	public int IdleTimeoutInSeconds { get; set; } = 10;

	public class Validator : ValidatorBase<CacheDataRemoveServiceOptions>
	{
		public Validator() { }
		public Validator(IObjectPath objectPath) : base(objectPath) { }

		public override void SetDefaultRuels(ValidatorBuilder<CacheDataRemoveServiceOptions> builder)
			=> RulesBuilder(builder);

		public static void RulesBuilder(ValidatorBuilder<CacheDataRemoveServiceOptions> builder)
		{
			builder?
				.ForProperty(x => x.IdleTimeoutInSeconds, v => v.GreaterThan(0));
		}
	}
}

public static class CacheDataRemoveServiceOptionsExtensions
{
	public static IServiceCollection AddCacheDataRemoveServiceOptions(this IServiceCollection services)
	{
		services
			.AddOptions<CacheDataRemoveServiceOptions>()
			.BindConfiguration($"{AppSettings.PREFIX}:{nameof(CacheDataRemoveServiceOptions)}")
			.AddOptionsValidator($"{AppSettings.PREFIX}.{nameof(CacheDataRemoveServiceOptions)}")
			.ValidateOnStart();

		return services;
	}
}
