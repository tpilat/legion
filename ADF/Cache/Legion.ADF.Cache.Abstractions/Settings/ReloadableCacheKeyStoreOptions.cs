using Legion.Extensions;
using Legion.Reflection.ObjectPaths;
using Legion.Validation;
using Microsoft.Extensions.DependencyInjection;

namespace Legion.ADF.Cache.Settings;

public class ReloadableCacheKeyStoreOptions
{
	public string CacheStoreId { get; set; }
	public long? SizeLimit { get; set; }

	public class Validator : ValidatorBase<ReloadableCacheKeyStoreOptions>
	{
		public Validator() { }
		public Validator(IObjectPath objectPath) : base(objectPath) { }

		public override void SetDefaultRuels(ValidatorBuilder<ReloadableCacheKeyStoreOptions> builder)
			=> RulesBuilder(builder);

		public static void RulesBuilder(ValidatorBuilder<ReloadableCacheKeyStoreOptions> builder)
		{
			builder?
				.ForProperty(x => x.CacheStoreId, v => v.MinLength(0))
				.ForProperty(x => x.SizeLimit, v => v.GreaterThan(0));
		}
	}
}

public static class ReloadableCacheKeyStoreOptionsExtensions
{
	public static IServiceCollection AddReloadableCacheKeyStoreOptions(this IServiceCollection services)
	{
		services
			.AddOptions<ReloadableCacheKeyStoreOptions>()
			.BindConfiguration($"{AppSettings.PREFIX}:{nameof(ReloadableCacheKeyStoreOptions)}")
			.AddOptionsValidator($"{AppSettings.PREFIX}.{nameof(ReloadableCacheKeyStoreOptions)}")
			.ValidateOnStart();

		return services;
	}
}