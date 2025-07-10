using Legion.Extensions;
using Legion.Reflection.ObjectPaths;
using Legion.Validation;
using Microsoft.Extensions.DependencyInjection;

namespace Legion.ADF.Cache.Settings;

public class DistributedLockRemoveServiceOptions
{
	public int IdleTimeoutInSeconds { get; set; } = 10;

	public class Validator : ValidatorBase<DistributedLockRemoveServiceOptions>
	{
		public Validator() { }
		public Validator(IObjectPath objectPath) : base(objectPath) { }

		public override void SetDefaultRuels(ValidatorBuilder<DistributedLockRemoveServiceOptions> builder)
			=> RulesBuilder(builder);

		public static void RulesBuilder(ValidatorBuilder<DistributedLockRemoveServiceOptions> builder)
		{
			builder?
				.ForProperty(x => x.IdleTimeoutInSeconds, v => v.GreaterThan(0));
		}
	}
}

public static class DistributedLockRemoveServiceOptionsExtensions
{
	public static IServiceCollection AddDistributedLockRemoveServiceOptions(this IServiceCollection services)
	{
		services
			.AddOptions<DistributedLockRemoveServiceOptions>()
			.BindConfiguration($"{AppSettings.PREFIX}:{nameof(DistributedLockRemoveServiceOptions)}")
			.AddOptionsValidator($"{AppSettings.PREFIX}.{nameof(DistributedLockRemoveServiceOptions)}")
			.ValidateOnStart();

		return services;
	}
}
