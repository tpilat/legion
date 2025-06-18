using Legion.Extensions;
using Legion.Reflection.ObjectPaths;
using Legion.Validation;
using Microsoft.Extensions.DependencyInjection;

namespace Legion.ADF.Cache.Settings;

public class CacheKeyRemoveServiceOptions
{
	public int IdleTimeoutInSeconds { get; set; } = 60;

	public class Validator : ValidatorBase<CacheKeyRemoveServiceOptions>
	{
		public Validator() { }
		public Validator(IObjectPath objectPath) : base(objectPath) { }

		public override void SetDefaultRuels(ValidatorBuilder<CacheKeyRemoveServiceOptions> builder)
			=> RulesBuilder(builder);

		public static void RulesBuilder(ValidatorBuilder<CacheKeyRemoveServiceOptions> builder)
		{
			builder?
				.ForProperty(x => x.IdleTimeoutInSeconds, v => v.GreaterThan(0));
		}
	}
}

public static class CacheKeyRemoveServiceOptionsExtensions
{
	public static IServiceCollection AddCacheKeyRemoveServiceOptions(this IServiceCollection services)
	{
		services
			.AddOptions<CacheKeyRemoveServiceOptions>()
			.BindConfiguration($"{AppSettings.PREFIX}:{nameof(CacheKeyRemoveServiceOptions)}")
			.AddOptionsValidator($"{AppSettings.PREFIX}.{nameof(CacheKeyRemoveServiceOptions)}")
			.ValidateOnStart();

		return services;
	}
}