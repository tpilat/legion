using Legion.Extensions;
using Legion.Reflection.ObjectPaths;
using Legion.Validation;
using Microsoft.Extensions.DependencyInjection;

namespace Legion.ADF.Cache.Settings;

public class DistributedLockOptions
{
	public string CacheStoreId { get; set; }

	public class Validator : ValidatorBase<DistributedLockOptions>
	{
		public Validator() { }
		public Validator(IObjectPath objectPath) : base(objectPath) { }

		public override void SetDefaultRuels(ValidatorBuilder<DistributedLockOptions> builder)
			=> RulesBuilder(builder);

		public static void RulesBuilder(ValidatorBuilder<DistributedLockOptions> builder)
		{
			builder?
				.ForProperty(x => x.CacheStoreId, v => v.MinLength(0));
		}
	}
}

public static class DistributedLockOptionsExtensions
{
	public static IServiceCollection AddDistributedLockOptions(this IServiceCollection services)
	{
		services
			.AddOptions<DistributedLockOptions>()
			.BindConfiguration($"{AppSettings.PREFIX}:{nameof(DistributedLockOptions)}")
			.AddOptionsValidator($"{AppSettings.PREFIX}.{nameof(DistributedLockOptions)}")
			.ValidateOnStart();

		return services;
	}
}
